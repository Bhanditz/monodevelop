using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using CorApi.ComInterop;

using Mono.Debugging.Backend;
using Mono.Debugging.Client;
using Mono.Debugging.Evaluation;
using System.Linq;

using CorApi.ComInterop.Eventing;
using CorApi.Pinvoke;

using CorApi2.debug;
using CorApi2.Extensions;
using CorApi2.Metadata;
using CorApi2.SymStore;

using JetBrains.Annotations;

using Microsoft.Win32.SafeHandles;

using HResults = CorApi.ComInterop.HResults;

namespace Mono.Debugging.Win32
{
	public unsafe class CorDebuggerSession: DebuggerSession // TODO: make sure we call Terminate on the debugger
	{
		readonly char[] badPathChars;
		readonly object debugLock = new object ();
		readonly object terminateLock = new object ();

		protected ICorDebug _dbg;
		protected CorApi.ComInterop.ICorDebugProcess _process;
		ICorDebugThread activeThread;
		ICorDebugStepper stepper;
		bool terminated;
		bool evaluating;
		bool autoStepInto;
		bool stepInsideDebuggerHidden=false;
		protected uint _processId;
		protected bool attaching = false;

		static int evaluationTimestamp;

		readonly SymbolBinder symbolBinder = MtaThread.Run (() => new SymbolBinder ());
		readonly object appDomainsLock = new object ();

		Dictionary<uint, AppDomainInfo> appDomains = new Dictionary<uint, AppDomainInfo> ();
		Dictionary<uint, ProcessInfo> processes = new Dictionary<uint, ProcessInfo>();
		Dictionary<uint, ThreadInfo> threads = new Dictionary<uint, ThreadInfo>();
		readonly Dictionary<ICorDebugBreakpoint, BreakEventInfo> breakpoints = new Dictionary<ICorDebugBreakpoint, BreakEventInfo> ();
		readonly Dictionary<ulong, ICorDebugHandleValue> handles = new Dictionary<ulong, ICorDebugHandleValue>();

		readonly BlockingCollection<Action> helperOperationsQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());
		readonly CancellationTokenSource helperOperationsCancellationTokenSource = new CancellationTokenSource ();

		public readonly CorObjectAdaptor ObjectAdapter = new CorObjectAdaptor();

		private readonly InternalErrorDelegate _onerror;

		class AppDomainInfo
		{
			public ICorDebugAppDomain AppDomain;
			public Dictionary<string, DocInfo> Documents;
			public Dictionary<string, ModuleInfo> Modules;
		}

		class DocInfo
		{
			public ISymbolDocument Document;
			public ModuleInfo ModuleInfo;
		}

		class ModuleInfo
		{
			public ISymbolReader Reader;
			public ICorDebugModule Module;
			public CorMetadataImport Importer;
		}

		public CorDebuggerSession(char[] badPathChars, [NotNull] InternalErrorDelegate onerror)
		{
			if(onerror == null)
				throw new ArgumentNullException(nameof(onerror));
			this.badPathChars = badPathChars;
			_onerror = onerror;
			ObjectAdapter.BusyStateChanged += (sender, e) => SetBusyState (e);
			var cancellationToken = helperOperationsCancellationTokenSource.Token;
			new Thread (() => {
				try {
					while (!cancellationToken.IsCancellationRequested) {
						var action = helperOperationsQueue.Take(cancellationToken);
						try {
							action ();
						}
						catch (Exception e) {
							DebuggerLoggingService.LogError ("Exception on processing helper thread action", e);
						}
					}

				}
				catch (Exception e) {
					if (e is OperationCanceledException || e is ObjectDisposedException) {
						DebuggerLoggingService.LogMessage ("Helper thread was gracefully interrupted");
					}
					else {
						DebuggerLoggingService.LogError ("Unhandled exception in helper thread. Helper thread is terminated", e);
					}
				}
			}) {Name = "CorDebug helper thread "}.Start();
		}

		public new IDebuggerSessionFrontend Frontend {
			get { return base.Frontend; }
		}

		public static int EvaluationTimestamp {
			get { return evaluationTimestamp; }
		}

		public ICustomCorSymbolReaderFactory CustomSymbolReaderFactory { get; set; }

		internal ICorDebugProcess Process
		{
			get
			{
				return _process;
			}
		}

		public override void Dispose ( )
		{
			MtaThread.Run (delegate
			{
				TerminateDebugger ();
				ObjectAdapter.Dispose();
			});
			helperOperationsCancellationTokenSource.Dispose ();
			helperOperationsQueue.Dispose ();

			base.Dispose ();

			// There is no explicit way of disposing the metadata objects, so we have
			// to rely on the GC to do it.

			lock (appDomainsLock) {
				foreach (var appDomainInfo in appDomains) {
					foreach (var module in appDomainInfo.Value.Modules.Values) {
						var disposable = module.Reader as IDisposable;
						if (disposable != null)
							disposable.Dispose ();
					}
				}
				appDomains = null;
			}

			threads = null;
			processes = null;
			activeThread = null;

			ThreadPool.QueueUserWorkItem (delegate {
				Thread.Sleep (2000);
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
				Thread.Sleep (20000);
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
			});
		}

		void QueueToHelperThread (Action action)
		{
			helperOperationsQueue.Add (action);
		}

		void DeactivateBreakpoints ()
		{
			var breakpointsCopy = breakpoints.Keys.ToList ();
			foreach (var corBreakpoint in breakpointsCopy) {
				try {
					corBreakpoint.Activate (0).AssertSucceeded("corBreakpoint.Activate (0)");
				}
				catch (Exception e) {
					DebuggerLoggingService.LogMessage ("Exception in DeactivateBreakpoints(): {0}", e);
				}
			}
		}

		void TerminateDebugger ()
		{
			helperOperationsCancellationTokenSource.Cancel();
			DeactivateBreakpoints ();
			lock (terminateLock) {
				if (terminated)
					return;

				terminated = true;

				if (_process != null) {
					// Process already running. Stop it. In the ProcessExited event the
					// debugger engine will be terminated
					try {
						_process.Stop (0).AssertSucceeded("_process.Stop (0)");;
						if (attaching) {
							_process.Detach ().AssertSucceeded("_process.Detach ()");;
						}
						else {
							_process.Terminate (1).AssertSucceeded("_process.Terminate (1)");;
						}
					}
					catch (COMException e) {
						// process was terminated, but debugger operation thread doesn't call ProcessExit callback at the time,
						// so we just think that the process is alive but that's wrong.
						// This may happen when e.g. when target process exited and Dispose was called at the same time
						// rethrow the exception in other case
						if (e.ErrorCode != (int) HResult.CORDBG_E_PROCESS_TERMINATED) {
							throw;
						}
					}
				}
			}
		}

		protected override void OnRun (DebuggerStartInfo startInfo)
		{
			MtaThread.Run (delegate
			{
				var env = PrepareEnvironment (startInfo);
				var cmdLine = PrepareCommandLine (startInfo);
				var dir = PrepareWorkingDirectory (startInfo);
				int flags = 0;
				if (!startInfo.UseExternalConsole) {
					flags = (int)CreationFlags.CREATE_NO_WINDOW;
						flags |= CorDebugProcessOutputRedirection.CREATE_REDIRECT_STD;
				}

				// Create the debugger

				string dversion;
				try {
					dversion = CorDebugFactory.GetDebuggerVersionFromFile (startInfo.Command); // TODO: log warning
				}
				catch {
					dversion = CorDebugFactory.GetDefaultDebuggerVersion ();
				}

				SetICorDebug(CorDebugFactory.CreateFromDebuggeeVersion(dversion));
				CorProcessWithOutputRedirection processWithOutput = CorDebugCreateProcess (_dbg, startInfo.Command, cmdLine, dir, env, flags);
				SetICorDebugProcess(processWithOutput.Process);
				processWithOutput.OutputRedirection.RegisterStdOutput(OnStdOutput);
			});
			OnStarted ();
		}

		/// <summary>
		/// Assigns the newly-created <see cref="ICorDebug" />, without even Initialize called.
		/// </summary>
		protected void SetICorDebug([NotNull] ICorDebug cordbg)
		{
			if(cordbg == null)
				throw new ArgumentNullException(nameof(cordbg));
			if(_dbg != null)
				throw new InvalidOperationException("The ICorDebug has already been set.");

			_dbg = cordbg;
			cordbg.Initialize().AssertSucceeded("Could not initialize the newly-created ICorDebug.");

			// Set managed handler for events
			ManagedCallbackEventSink.HandleEventDelegate onEvent = (eventid, args) =>
			{
				// Exceptions are trapped by the event sink class
				switch(eventid)
				{
				case ManagedCallbackType.OnCreateProcess:
					OnCreateProcess(this, ((CorProcessEventArgs)args));
					break;
				case ManagedCallbackType.OnCreateAppDomain:
					OnCreateAppDomain(this, (CorAppDomainEventArgs)args);
					break;
				case ManagedCallbackType.OnAppDomainExit:
					OnAppDomainExit(this, ((CorAppDomainEventArgs)args));
					break;
				case ManagedCallbackType.OnAssemblyLoad:
					OnAssemblyLoad(this, (CorAssemblyEventArgs)args);
					break;
				case ManagedCallbackType.OnAssemblyUnload:
					OnAssemblyUnload(this, ((CorAssemblyEventArgs)args));
					break;
				case ManagedCallbackType.OnCreateThread:
					OnCreateThread(this, ((CorThreadEventArgs)args));
					break;
				case ManagedCallbackType.OnThreadExit:
					OnThreadExit(this, ((CorThreadEventArgs)args));
					break;
				case ManagedCallbackType.OnModuleLoad:
					OnModuleLoad(this, ((CorModuleEventArgs)args));
					break;
				case ManagedCallbackType.OnModuleUnload:
					OnModuleUnload(this, ((CorModuleEventArgs)args));
					break;
				case ManagedCallbackType.OnProcessExit:
					OnProcessExit(this, ((CorProcessEventArgs)args));
					break;
				case ManagedCallbackType.OnUpdateModuleSymbols:
					OnUpdateModuleSymbols(this, ((CorUpdateModuleSymbolsEventArgs)args));
					break;
				case ManagedCallbackType.OnDebuggerError:
					OnDebuggerError(this, ((CorDebuggerErrorEventArgs)args));
					break;
				case ManagedCallbackType.OnBreakpoint:
					OnBreakpoint(this, ((CorBreakpointEventArgs)args));
					break;
				case ManagedCallbackType.OnStepComplete:
					OnStepComplete(this, ((CorStepCompleteEventArgs)args));
					break;
				case ManagedCallbackType.OnBreak:
					OnBreak(this, ((CorThreadEventArgs)args));
					break;
				case ManagedCallbackType.OnNameChange:
					OnNameChange(this, ((CorThreadEventArgs)args));
					break;
				case ManagedCallbackType.OnEvalComplete:
					OnEvalComplete(this, ((CorEvalEventArgs)args));
					break;
				case ManagedCallbackType.OnEvalException:
					OnEvalException(this, ((CorEvalEventArgs)args));
					break;
				case ManagedCallbackType.OnLogMessage:
					OnLogMessage(this, ((CorLogMessageEventArgs)args));
					break;
				case ManagedCallbackType.OnException2:
					OnException2(this, ((CorException2EventArgs)args));
					break;
				}

				// NOTE: this autocontinue logic has been borrowed from the previous implementation
				/**
				 * Helper for invoking events.  Checks to make sure that handlers
				 * are hooked up to a handler before the handler is invoked.
				 *
				 * We want to allow maximum flexibility by our callers.  As such,
				 * we don't require that they call <code>e.Controller.Continue</code>,
				 * nor do we require that this class call it.  <b>Someone</b> needs
				 * to call it, however.
				 *
				 * Consequently, if an exception is thrown and the process is stopped,
				 * the process is continued automatically.
				 */
				if(args.Continue)
					args.Controller.Continue(0).AssertSucceeded("Could not Continue debugger after processing a debugger managed event callback with event's Continue status set to True.");

				return HResults.S_OK; //TODO: propagate return values where applicable in events
			};
			cordbg.SetManagedHandler(new ManagedCallbackEventSink(onEvent, _onerror)).AssertSucceeded("Could not advise sinking of the managed ICorDebug events.");
		}

		protected void CorDebugAttachPid(uint pid)
		{
			ICorDebugProcess process;
			_dbg.DebugActiveProcess(pid, 0, out process).AssertSucceeded($"Could not make the ICorDebug start debugging the process {pid:N0}.");
			SetICorDebugProcess(process);
		}

		protected void SetICorDebugProcess([NotNull] ICorDebugProcess process)
		{
			if(process == null)
				throw new ArgumentNullException(nameof(process));
			if(_process != null)
				throw new InvalidOperationException("The ICorDebugProcess has already been set.");

			_process = process;
			uint pid;
			_process.GetID(&pid).AssertSucceeded("Could not get the PID of the newly-created process.");
			_processId = pid;

			// _cordbgevents.NotNull("CorDebugEvents must have been set up with the main debugger interface.").Resume(); // NOTE: looks like the Resume were for a generic case, and we start sinking events even before we do anything about a Process in the debugger interface
			_process.Continue(0).AssertSucceeded("Could not continue the initially stopped process.");
		}

		/// <summary>
		/// Launch a process under the control of the debugger.
		/// 
		/// Parameters are the same as the Win32 CreateProcess call.
		/// </summary>
		public static CorProcessWithOutputRedirection CorDebugCreateProcess (ICorDebug mDebugger, String applicationName,
			String commandLine,
			String currentDirectory = ".",
			IDictionary<string,string> environment = null,
			int    flags = 0)
		{
			CorApi.ComInterop.PROCESS_INFORMATION pi = new CorApi.ComInterop.PROCESS_INFORMATION ();

			STARTUPINFOW si = new STARTUPINFOW ();
			si.cb = (uint) Marshal.SizeOf(si);

			// initialize safe handles 
			// [Xamarin] ASP.NET Debugging and output redirection.
			SafeFileHandle outReadPipe, errorReadPipe;
			Action closehandles;
			CorDebugProcessOutputRedirection.SetupOutputRedirection (si, ref flags, out outReadPipe, out errorReadPipe, out closehandles);
			string env = Kernel32Dll.Helpers.GetEnvString(environment);

			CorApi.ComInterop.ICorDebugProcess corprocess;

			//constrained execution region (Cer)
			System.Runtime.CompilerServices.RuntimeHelpers.PrepareConstrainedRegions();
			try 
			{
			} 
			finally
			{
				corprocess =  mDebugger.CreateProcess(applicationName,
					commandLine, 
					default(SECURITY_ATTRIBUTES),
					default(SECURITY_ATTRIBUTES),
					true,   // inherit handles
					flags,  // creation flags
					env,      // environment
					currentDirectory,
					si,     // startup info
					ref pi, // process information
					CorDebugCreateProcessFlags.DEBUG_NO_SPECIAL_OPTIONS);
				if(pi.hProcess != null)
					Kernel32Dll.CloseHandle(pi.hProcess);
				if(pi.hThread != null)
					Kernel32Dll.CloseHandle(pi.hThread);
			}

			var outputredirection = new CorDebugProcessOutputRedirection(corprocess);
			outputredirection.TearDownOutputRedirection (outReadPipe, errorReadPipe, closehandles);

			return new CorProcessWithOutputRedirection() {Process = corprocess, OutputRedirection = outputredirection};
		}


		protected static string PrepareWorkingDirectory (DebuggerStartInfo startInfo)
		{
			var dir = startInfo.WorkingDirectory;
			if (string.IsNullOrEmpty (dir))
				dir = Path.GetDirectoryName (startInfo.Command);
			return dir;
		}

		protected static string PrepareCommandLine (DebuggerStartInfo startInfo)
		{
			// The second parameter of CreateProcess is the command line, and it includes the application being launched
			string cmdLine = "\"" + startInfo.Command + "\" " + startInfo.Arguments;
			return cmdLine;
		}

		protected static Dictionary<string, string> PrepareEnvironment (DebuggerStartInfo startInfo)
		{
			Dictionary<string, string> env = new Dictionary<string, string> ();
			foreach (DictionaryEntry de in Environment.GetEnvironmentVariables ())
				env[(string) de.Key] = (string) de.Value;

			foreach (KeyValuePair<string, string> var in startInfo.EnvironmentVariables)
				env[var.Key] = var.Value;
			return env;
		}

		void OnStdOutput (object sender, CorTargetOutputEventArgs e)
		{
			OnTargetOutput (e.IsStdError, e.Text);
		}

		void OnLogMessage (object sender, CorLogMessageEventArgs e)
		{
			OnTargetDebug (e.Level, e.LogSwitchName, e.Message);
			e.Continue = true;
		}

		void OnEvalException (object sender, CorEvalEventArgs e)
		{
			evaluationTimestamp++;
		}

		void OnEvalComplete (object sender, CorEvalEventArgs e)
		{
			evaluationTimestamp++;
		}

		void OnNameChange (object sender, CorThreadEventArgs e)
		{
		}

		void OnStopped ( )
		{
			evaluationTimestamp++;
			lock (threads) {
				threads.Clear ();
			}
		}

		void OnBreak (object sender, CorThreadEventArgs e)
		{
			lock (debugLock) {
				if (evaluating) {
					e.Continue = true;
					return;
				}
			}
			OnStopped ();
			e.Continue = false;
			SetActiveThread (e.Thread);
			TargetEventArgs args = new TargetEventArgs (TargetEventType.TargetInterrupted);
			args.Process = GetProcess (_process);
			args.Thread = GetThread (e.Thread);
			args.Backtrace = new Backtrace (new CorBacktrace (e.Thread, this));
			OnTargetEvent (args);
		}

		bool StepThrough (MethodInfo methodInfo)
		{
			var m = methodInfo.GetCustomAttributes (true);
			if (Options.ProjectAssembliesOnly) {
				return methodInfo.GetCustomAttributes (true).Union (methodInfo.DeclaringType.GetCustomAttributes (true)).Any (v =>
					v is System.Diagnostics.DebuggerHiddenAttribute ||
					v is System.Diagnostics.DebuggerStepThroughAttribute ||
					v is System.Diagnostics.DebuggerNonUserCodeAttribute);
			} else {
				return methodInfo.GetCustomAttributes (true).Union (methodInfo.DeclaringType.GetCustomAttributes (true)).Any (v =>
					v is System.Diagnostics.DebuggerHiddenAttribute ||
					v is System.Diagnostics.DebuggerStepThroughAttribute);
			}
		}

		bool ContinueOnStepIn(MethodInfo methodInfo)
		{
			return methodInfo.GetCustomAttributes (true).Any (v => v is System.Diagnostics.DebuggerStepperBoundaryAttribute);
		}

		static bool IsPropertyOrOperatorMethod (MethodInfo method)
		{
			if (method == null)
				return false;
			string name = method.Name;

			return method.IsSpecialName &&
			(name.StartsWith ("get_", StringComparison.Ordinal) ||
			name.StartsWith ("set_", StringComparison.Ordinal) ||
			name.StartsWith ("op_", StringComparison.Ordinal));
		}

		static bool IsCompilerGenerated (MethodInfo method)
		{
			return method.GetCustomAttributes (true).Any (v => v is System.Runtime.CompilerServices.CompilerGeneratedAttribute);
		}

		void OnStepComplete (object sender, CorStepCompleteEventArgs e)
		{
			lock (debugLock) {
				if (evaluating) {
					e.Continue = true;
					return;
				}
			}

			bool localAutoStepInto = autoStepInto;
			autoStepInto = false;
			bool localStepInsideDebuggerHidden = stepInsideDebuggerHidden;
			stepInsideDebuggerHidden = false;

			int isQueued;
			ICorDebugProcess process;
			e.AppDomain.GetProcess(out process).AssertSucceeded("Could not get the Process of the AppDomain.");
			process.HasQueuedCallbacks (e.Thread, &isQueued).AssertSucceeded("e.AppDomain.Process.HasQueuedCallbacks (e.Thread, &isQueued)");
			if (isQueued.ToBool()) {
				e.Continue = true;
				return;
			}

			if (localAutoStepInto) {
				Step (true);
				e.Continue = true;
				return;
			}

			ICorDebugFrame activeframe;
			e.Thread.GetActiveFrame(out activeframe).AssertSucceeded("Could not get the Active Frame of a Thread.");
			ICorDebugFunction framefunction;
			activeframe.GetFunction(out framefunction).AssertSucceeded("Could not get the Function of a Frame.");
			var function = framefunction;
			if (ContinueOnStepIn (function.GetMethodInfo (this))) {
				e.Continue = true;
				return;
			}

			var currentSequence = CorBacktrace.GetSequencePoint (this, activeframe);
			if (currentSequence == null) {
				stepper.StepOut ().AssertSucceeded("stepper.StepOut ()");;
				autoStepInto = true;
				e.Continue = true;
				return;
			}

			if (StepThrough (function.GetMethodInfo (this))) {
				stepInsideDebuggerHidden = e.StepReason == CorDebugStepReason.STEP_CALL;
				RawContinue (true, true);
				e.Continue = true;
				return;
			}

			if ((Options.StepOverPropertiesAndOperators || IsCompilerGenerated(function.GetMethodInfo (this))) &&
			    IsPropertyOrOperatorMethod (function.GetMethodInfo (this)) &&
				e.StepReason == CorDebugStepReason.STEP_CALL) {
				stepper.StepOut ().AssertSucceeded("stepper.StepOut ()");;
				autoStepInto = true;
				e.Continue = true;
				return;
			}

			if (currentSequence.IsSpecial) {
				Step (false);
				e.Continue = true;
				return;
			}

			if (localStepInsideDebuggerHidden && e.StepReason == CorDebugStepReason.STEP_RETURN) {
				Step (true);
				e.Continue = true;
				return;
			}

			OnStopped ();
			e.Continue = false;
			SetActiveThread (e.Thread);
			TargetEventArgs args = new TargetEventArgs (TargetEventType.TargetStopped);
			args.Process = GetProcess (_process);
			args.Thread = GetThread (e.Thread);
			args.Backtrace = new Backtrace (new CorBacktrace (e.Thread, this));
			OnTargetEvent (args);
		}

		void OnThreadExit (object sender, CorThreadEventArgs e)
		{
			lock (threads) {
				uint dwThreadId;
				e.Thread.GetID(&dwThreadId).AssertSucceeded("Could not get the ID of a Thread.");
				threads.Remove (dwThreadId);
			}
		}

		void OnBreakpoint (object sender, CorBreakpointEventArgs e)
		{
			lock (debugLock) {
				if (evaluating) {
					e.Continue = true;
					return;
				}
			}

			// we have to stop an execution and enqueue breakpoint calculations on another thread to release debugger event thread for further events
			// we can't perform any evaluations inside this handler, because the debugger thread is busy and we won't get evaluation events there
			e.Continue = false;

			QueueToHelperThread (() => {
				BreakEventInfo binfo;
				BreakEvent breakEvent = null;
				int isRunning;
				e.Controller.IsRunning (&isRunning).AssertSucceeded("e.Controller.IsRunning (&isRunning)");
				if (isRunning.ToBool())
					throw new InvalidOperationException ("Debuggee isn't stopped to perform breakpoint calculations");

				var shouldContinue = false;
				if (breakpoints.TryGetValue (e.Breakpoint, out binfo)) {
					breakEvent = (Breakpoint) binfo.BreakEvent;
					try {
						shouldContinue = ShouldContinueOnBreakpoint (e.Thread, binfo);
					}
					catch (Exception ex) {
						DebuggerLoggingService.LogError ("ShouldContinueOnBreakpoint() has thrown an exception", ex);
					}
				}

				int isQueued;
				ICorDebugProcess process;
				e.AppDomain.GetProcess(out process).AssertSucceeded("Could not get the Process of the AppDomain.");
				process.HasQueuedCallbacks(e.Thread, &isQueued).AssertSucceeded("e.AppDomain.Process.HasQueuedCallbacks (e.Thread, &isQueued)");
				if(shouldContinue || isQueued.ToBool())
				{
					e.Controller.SetAllThreadsDebugState(CorDebugThreadState.THREAD_RUN, null).AssertSucceeded("e.Controller.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null)");
					e.Controller.Continue(0).AssertSucceeded("e.Controller.Continue (0)");
					return;
				}

				OnStopped ();
				// If a breakpoint is hit while stepping, cancel the stepping operation
				if (stepper != null)
				{
					int isStepperActive;
					stepper.IsActive(&isStepperActive).AssertSucceeded("Could not get if a Stepper is Active.");
					if(isStepperActive.ToBool())
						stepper.Deactivate ().AssertSucceeded("stepper.Deactivate ()");
				}
				autoStepInto = false;
				SetActiveThread (e.Thread);
				var args = new TargetEventArgs (TargetEventType.TargetHitBreakpoint) {
					Process = GetProcess (_process),
					Thread = GetThread (e.Thread),
					Backtrace = new Backtrace (new CorBacktrace (e.Thread, this)),
					BreakEvent = breakEvent
				};
				OnTargetEvent (args);
			});
		}

		bool ShouldContinueOnBreakpoint (ICorDebugThread thread, BreakEventInfo binfo)
		{
			var bp = (Breakpoint) binfo.BreakEvent;
			binfo.IncrementHitCount();
			if (!binfo.HitCountReached)
				return true;

			if (!string.IsNullOrEmpty (bp.ConditionExpression)) {
				try {
					string res = EvaluateExpression (thread, bp.ConditionExpression);
					if (bp.BreakIfConditionChanges) {
						if (res == bp.LastConditionValue)
							return true;
						bp.LastConditionValue = res;
					}
					else {
						if (res != null && res.ToLower () != "true")
							return true;
					}
				}
				catch (EvaluatorException e) {
					OnDebuggerOutput (false, e.Message);
					binfo.SetStatus (BreakEventStatus.Invalid, e.Message);
					return true;
				}
			}

			if ((bp.HitAction & HitAction.CustomAction) != HitAction.None) {
				// If custom action returns true, execution must continue
				if (binfo.RunCustomBreakpointAction (bp.CustomActionId))
					return true;
			}

			if ((bp.HitAction & HitAction.PrintTrace) != HitAction.None) {
				OnTargetDebug (0, "", "Breakpoint reached: " + bp.FileName + ":" + bp.Line + Environment.NewLine);
			}

			if ((bp.HitAction & HitAction.PrintExpression) != HitAction.None) {
				string exp = EvaluateTrace (thread, bp.TraceExpression);
				binfo.UpdateLastTraceValue (exp);
			}

			return (bp.HitAction & HitAction.Break) == HitAction.None;
		}

		void OnDebuggerError (object sender, CorDebuggerErrorEventArgs e)
		{
			Exception ex = Marshal.GetExceptionForHR (e.HResult);
			OnDebuggerOutput (true, string.Format ("Debugger Error: {0}\n", ex.Message));
		}

		void OnUpdateModuleSymbols (object sender, CorUpdateModuleSymbolsEventArgs e)
		{
			e.Continue = true;
		}

		void OnProcessExit (object sender, CorProcessEventArgs e)
		{
			TargetEventArgs args = new TargetEventArgs (TargetEventType.TargetExited);

			// If the main thread stopped, terminate the debugger session
			uint dwProcessIdFromEvent;
			e.Process.GetID(&dwProcessIdFromEvent).AssertSucceeded("Could not get the ID of a Process.");
			uint dwProcessIdOur;
			_process.GetID(&dwProcessIdOur).AssertSucceeded("Could not get the ID of a Process.");
			if (dwProcessIdFromEvent == dwProcessIdOur) {
				lock (terminateLock) {
					_process = null;
					ThreadPool.QueueUserWorkItem (delegate
					{
						// The Terminate call will fail if called in the event handler
						_dbg.Terminate ().AssertSucceeded("_dbg.Terminate ()");;
						_dbg = null;
						GC.Collect ();
					});
				}
			}

			OnTargetEvent (args);
		}

		void OnAssemblyUnload (object sender, CorAssemblyEventArgs e)
		{
			OnDebuggerOutput (false, string.Format ("Unloaded Module '{0}'\n", LpwstrHelper.GetString(e.Assembly.GetName, "Could not get the assembly name.")));
			e.Continue = true;
		}

		void OnModuleLoad (object sender, CorModuleEventArgs e)
		{
			var currentModule = e.Module;
			CorMetadataImport mi = new CorMetadataImport (currentModule);

			try {
				// Required to avoid the jit to get rid of variables too early
				Com.QueryInteface<ICorDebugModule2>(currentModule).SetJITCompilerFlags((uint)CorDebugJITCompilerFlags.CORDEBUG_JIT_DISABLE_OPTIMIZATION).AssertSucceeded("Could not set the JIT Compiler Flags to disable optimization.");
			}
			catch {
				// Some kind of modules don't allow JIT flags to be changed.
			}

			var currentDomain = e.AppDomain;
			uint currentDomainId=0;
			currentDomain.GetID(&currentDomainId).AssertSucceeded("Could not get the ID of an AppDomain.");
			string currentDomainName = LpwstrHelper.GetString(currentDomain.GetName, "Could not get the Name of an AppDomain.");
			string currentModuleName = LpwstrHelper.GetString(currentModule.GetName, "Could not get the Name of a Module.");
			OnDebuggerOutput (false, String.Format("Loading module {0} in application domain {1}:{2}\n", currentModuleName, currentDomainId, currentDomainName));
			ICorDebugAssembly moduleassembly;
			currentModule.GetAssembly(out moduleassembly).AssertSucceeded("Could not get the Assembly of a Module.");
			string file = LpwstrHelper.GetString(moduleassembly.GetName, "Could not get the Name of an Assembly.");
			var newDocuments = new Dictionary<string, DocInfo> ();
			var justMyCode = false;
			ISymbolReader reader = null;
			if (file.IndexOfAny (badPathChars) == -1) {
				try {
					reader = symbolBinder.GetReaderForFile (mi.RawCOMObject, file, ".",
						SymSearchPolicies.AllowOriginalPathAccess | SymSearchPolicies.AllowReferencePathAccess);

					if (reader == null && CustomSymbolReaderFactory != null) {
						reader = CustomSymbolReaderFactory.CreateCustomSymbolReader (file);
					}

					if (reader != null) {
						OnDebuggerOutput (false, string.Format ("Symbols for module {0} loaded\n", file));
						// set JMC to true only when we got the reader.
						// When module JMC is true, debugger will step into it
						justMyCode = true;
						foreach (ISymbolDocument doc in reader.GetDocuments ()) {
							if (string.IsNullOrEmpty (doc.URL))
								continue;
							string docFile = System.IO.Path.GetFullPath (doc.URL);
							DocInfo di = new DocInfo ();
							di.Document = doc;
							newDocuments[docFile] = di;
						}
					}
				}
				catch (COMException ex)
				{
					if(ex.ErrorCode != (int)PdbHResult.E_PDB_OK)
					{
						OnDebuggerOutput(false, string.Format("Failed to load pdb for assembly {0}. Error code {1}(0x{2:X})\n", file, ex.ErrorCode, ex.ErrorCode));
						if(!typeof(PdbHResult).IsEnumDefined(ex.ErrorCode))
							DebuggerLoggingService.LogError(string.Format("Loading symbols of module {0} failed", currentModuleName), ex);
					}
				}
				catch (Exception ex) {
					DebuggerLoggingService.LogError (string.Format ("Loading symbols of module {0} failed", currentModuleName), ex);
				}
			}
			try {
				Com.QueryInteface<ICorDebugModule2>(currentModule).SetJMCStatus(justMyCode.ToInt(), 0, null).AssertSucceeded("Could not set JustMyCode status for the module.");
			}
			catch (COMException ex) {
				// somewhen exceptions is thrown
				DebuggerLoggingService.LogMessage ("Exception during setting JMC: {0}", ex.Message);
			}

			lock (appDomainsLock) {
				AppDomainInfo appDomainInfo;
				if (!appDomains.TryGetValue (currentDomainId, out appDomainInfo)) {
				  DebuggerLoggingService.LogMessage ("OnCreatedAppDomain was not fired for domain {0} (id {1})", currentDomainName, currentDomainId);
					appDomainInfo = new AppDomainInfo {
						AppDomain = currentDomain,
						Documents = new Dictionary<string, DocInfo> (StringComparer.InvariantCultureIgnoreCase),
						Modules = new Dictionary<string, ModuleInfo> (StringComparer.InvariantCultureIgnoreCase)
					};
					appDomains[currentDomainId] = appDomainInfo;
				}
				var modules = appDomainInfo.Modules;
				if (modules.ContainsKey (currentModuleName)) {
				  DebuggerLoggingService.LogMessage ("Module {0} was already added for app domain {1} (id {2}). Replacing\n",
						currentModuleName, currentDomainName, currentDomainId);
				}
				var newModuleInfo = new ModuleInfo {
					Module = currentModule,
					Reader = reader,
					Importer = mi,
				};
				modules[currentModuleName] = newModuleInfo;
				var existingDocuments = appDomainInfo.Documents;
				foreach (var newDocument in newDocuments) {
					var documentFile = newDocument.Key;
					var newDocInfo = newDocument.Value;
					if (existingDocuments.ContainsKey (documentFile)) {
					  DebuggerLoggingService.LogMessage ("Document {0} was already added for module {1} in domain {2} (id {3}). Replacing\n",
							documentFile, currentModuleName, currentDomainName, currentDomainId);
					}
					newDocInfo.ModuleInfo = newModuleInfo;
					existingDocuments[documentFile] = newDocInfo;
				}

			}

			foreach (var newFile in newDocuments.Keys) {
				BindSourceFileBreakpoints (newFile);
			}

			e.Continue = true;
		}

		void OnModuleUnload (object sender, CorModuleEventArgs e)
		{
			var currentDomain = e.AppDomain;
			uint currentDomainId=0;
			currentDomain.GetID(&currentDomainId).AssertSucceeded("Could not get the ID of an AppDomain.");
			var currentModule = e.Module;
			string currentDomainName = LpwstrHelper.GetString(currentDomain.GetName, "Could not get the Name of an AppDomain.");
			string currentModuleName = LpwstrHelper.GetString(currentModule.GetName, "Could not get the Name of a Module.");
			var documentsToRemove = new List<string> ();
			lock (appDomainsLock) {
				AppDomainInfo appDomainInfo;
				if (!appDomains.TryGetValue (currentDomainId, out appDomainInfo)) {
					DebuggerLoggingService.LogMessage ("Failed unload module {0} for app domain {1} (id {2}) because app domain was not found or already unloaded\n",
							currentModuleName, currentDomainName, currentDomainId);
					return;
				}
				ModuleInfo moi;
				if (!appDomainInfo.Modules.TryGetValue (currentModuleName, out moi)) {
					DebuggerLoggingService.LogMessage ("Failed unload module {0} for app domain {1} (id {2}) because the module was not found or already unloaded\n",
						currentModuleName, currentDomainName, currentDomainId);
				}
				else {
					appDomainInfo.Modules.Remove (currentModuleName);
					var disposableReader = moi.Reader as IDisposable;
					if (disposableReader != null)
						disposableReader.Dispose ();
				}

				foreach (var docInfo in appDomainInfo.Documents) {
					if (LpwstrHelper.GetString(docInfo.Value.ModuleInfo.Module.GetName, "Could not get the Name of a Module.") == currentModuleName)
						documentsToRemove.Add (docInfo.Key);
				}
				foreach (var file in documentsToRemove) {
					appDomainInfo.Documents.Remove (file);
				}
			}
			foreach (var file in documentsToRemove) {
				UnbindSourceFileBreakpoints (file);
			}
		}

		void OnCreateAppDomain (object sender, CorAppDomainEventArgs e)
		{
			uint appDomainId=0;
			e.AppDomain.GetID(&appDomainId).AssertSucceeded("Could not get the ID of an AppDomain.");
			string appDomainName = LpwstrHelper.GetString(e.AppDomain.GetName, "Could not get the Name of an AppDomain.");
			lock (appDomainsLock) {
				if (!appDomains.ContainsKey (appDomainId)) {
					appDomains[appDomainId] = new AppDomainInfo {
						AppDomain = e.AppDomain,
						Documents = new Dictionary<string, DocInfo> (StringComparer.InvariantCultureIgnoreCase),
						Modules = new Dictionary<string, ModuleInfo> (StringComparer.InvariantCultureIgnoreCase)
					};
				}
				else {
					DebuggerLoggingService.LogMessage ("App domain {0} (id {1}) was already loaded", appDomainName, appDomainId);
				}
			}
			e.AppDomain.Attach().AssertSucceeded("e.AppDomain.Attach()");;
			e.Continue = true;
			OnDebuggerOutput (false, string.Format("Loaded application domain '{0} (id {1})'\n", appDomainName, appDomainId));
		}

		private void OnAppDomainExit (object sender, CorAppDomainEventArgs e)
		{
			uint appDomainId=0;
			e.AppDomain.GetID(&appDomainId).AssertSucceeded("Could not get the ID of an AppDomain.");
			string appDomainName = LpwstrHelper.GetString(e.AppDomain.GetName, "Could not get the Name of an AppDomain.");
			lock (appDomainsLock) {
				if (!appDomains.Remove (appDomainId)) {
				  DebuggerLoggingService.LogMessage ("Failed to unload app domain {0} (id {1}) because it's not found in map. Possibly already unloaded.", appDomainName, appDomainId);
				}
			}
			// Detach is not implemented for ICorDebugAppDomain, it's valid only for ICorDebugProcess
			//e.AppDomain.Detach ();
			e.Continue = true;
			OnDebuggerOutput (false, string.Format("Unloaded application domain '{0} (id {1})'\n", appDomainName, appDomainId));
		}

		private void OnCreateProcess(object sender, CorProcessEventArgs e)
		{
			// Required to avoid the jit to get rid of variables too early
			// not allowed in attach mode
			int hr = Com.QueryInteface<ICorDebugProcess2>(e.Process).SetDesiredNGENCompilerFlags(((uint)CorDebugJITCompilerFlags.CORDEBUG_JIT_DISABLE_OPTIMIZATION));
			Exception exErrJit = HResultHelpers.GetExceptionIfFailed(hr, "Unable to set e.Process.DesiredNGENCompilerFlags, possibly because the process was attached.");
			if(exErrJit != null)
				DebuggerLoggingService.LogMessage(exErrJit.Message);
			e.Process.EnableLogMessages(1).AssertSucceeded("Could not enable log messages on the process.");
			e.Continue = true;
		}

		void OnCreateThread (object sender, CorThreadEventArgs e)
		{
			uint dwThreadId;
			e.Thread.GetID(&dwThreadId).AssertSucceeded("Could not get the ID of a Thread.");
			OnDebuggerOutput (false, string.Format ("Started Thread {0}\n", dwThreadId));
			e.Continue = true;
		}

		void OnAssemblyLoad (object sender, CorAssemblyEventArgs e)
		{
			OnDebuggerOutput (false, string.Format ("Loaded Assembly '{0}'\n", LpwstrHelper.GetString(e.Assembly.GetName, "Could not get the assembly name.")));
			e.Continue = true;
		}
		
		void OnException2 (object sender, CorException2EventArgs e)
		{
			lock (debugLock) {
				if (evaluating) {
					e.Continue = true;
					return;
				}
			}
			
			TargetEventArgs args = null;
			
			switch (e.EventType) {
				case CorDebugExceptionCallbackType.DEBUG_EXCEPTION_FIRST_CHANCE:
					if (!this.Options.ProjectAssembliesOnly && IsCatchpoint (e))
						args = new TargetEventArgs (TargetEventType.ExceptionThrown);
					break;
				case CorDebugExceptionCallbackType.DEBUG_EXCEPTION_USER_FIRST_CHANCE:
					if (IsCatchpoint (e))
						args = new TargetEventArgs (TargetEventType.ExceptionThrown);
					break;
				case CorDebugExceptionCallbackType.DEBUG_EXCEPTION_CATCH_HANDLER_FOUND:
					break;
				case CorDebugExceptionCallbackType.DEBUG_EXCEPTION_UNHANDLED:
					args = new TargetEventArgs (TargetEventType.UnhandledException);
					break;
			}
			
			if (args != null) {
				OnStopped ();
				e.Continue = false;
				// If an exception is thrown while stepping, cancel the stepping operation
				if (stepper != null)
				{
					int isStepperActive;
					stepper.IsActive(&isStepperActive).AssertSucceeded("Could not get if a Stepper is Active.");
					if(isStepperActive.ToBool())
						stepper.Deactivate ().AssertSucceeded("stepper.Deactivate ()");
				}
				;
				autoStepInto = false;
				SetActiveThread (e.Thread);
				
				args.Process = GetProcess (_process);
				args.Thread = GetThread (e.Thread);
				args.Backtrace = new Backtrace (new CorBacktrace (e.Thread, this));
				OnTargetEvent (args);	
			}
		}

		public bool IsExternalCode (string fileName)
		{
			if (string.IsNullOrWhiteSpace (fileName))
				return true;
			lock (appDomainsLock) {
				foreach (var appDomainInfo in appDomains) {
					if (appDomainInfo.Value.Documents.ContainsKey (fileName))
						return false;
				}
			}
			return true;
		}

		private bool IsCatchpoint (CorException2EventArgs e)
		{
			// Build up the exception type hierachy
			ICorDebugValue v;
			e.Thread.GetCurrentException(out v).AssertSucceeded("e.Thread.GetCurrentException(out v)");
			List<string> exceptions = new List<string>();
			ICorDebugType t = v.GetExactType();
			while (t != null) {
				exceptions.Add(t.GetTypeInfo(this).FullName);
				ICorDebugType basetype;
				t.GetBase(out basetype).AssertSucceeded("Could not get the Base Type of a Type.");
				t = basetype;
			}
			if (exceptions.Count == 0)
				return false;
			// See if a catchpoint is set for this exception.
			foreach (Catchpoint cp in Breakpoints.GetCatchpoints()) {
				if (cp.Enabled &&
				    ((cp.IncludeSubclasses && exceptions.Contains (cp.ExceptionName)) ||
				    (exceptions [0] == cp.ExceptionName))) {
					return true;
				}
			}
			
			return false;
		}

		protected override void OnAttachToProcess(long procId)
		{
			attaching = true;
			MtaThread.Run(delegate
			{
				List<string> versions = CorDebugFactory.GetProcessLoadedRuntimes((uint)procId);
				if(!versions.Any())
					throw new InvalidOperationException(string.Format("Process {0} doesn't have .NET loaded runtimes", procId));
				SetICorDebug(CorDebugFactory.CreateFromDebuggeeVersion(versions.Last()));
				CorDebugAttachPid((uint)procId);
			});
			OnStarted();
		}

		protected override void OnAttachToProcess(ProcessInfo processInfo)
		{
			var clrProcessInfo = processInfo as ClrProcessInfo;
			string version = clrProcessInfo != null ? clrProcessInfo.Runtime : null;

			attaching = true;
			MtaThread.Run(delegate
			{
				List<string> versions = CorDebugFactory.GetProcessLoadedRuntimes((uint)processInfo.Id);
				if(!versions.Any())
					throw new InvalidOperationException(string.Format("Process {0} doesn't have .NET loaded runtimes", processInfo.Id));
				if(version == null || !versions.Contains(version))
					version = versions.Last();
				SetICorDebug(CorDebugFactory.CreateFromDebuggeeVersion(version));
				CorDebugAttachPid((uint)processInfo.Id);
			});
			OnStarted();
		}

		protected override void OnContinue ( )
		{
			MtaThread.Run (delegate
			{
				ClearEvalStatus ();
				ClearHandles ();
				_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null).AssertSucceeded("_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null)");;
				_process.Continue (0).AssertSucceeded("_process.Continue (0)");;
			});
		}

		protected override void OnDetach ( )
		{
			MtaThread.Run (delegate
			{
				TerminateDebugger ();
			});
		}

		protected override void OnEnableBreakEvent(BreakEventInfo binfo, bool enable)
		{
			MtaThread.Run(delegate
			{
				var bpList = binfo.Handle as List<ICorDebugFunctionBreakpoint>;
				if(bpList == null)
					return;
				foreach(ICorDebugFunctionBreakpoint bp in bpList)
				{
					var hrBp = (HResult)bp.Activate(enable.ToInt());
					HandleBreakpointFailure(binfo, hrBp).Assert("Failed to activate the breakpoint.");
				}
			});
		}

		protected override void OnExit ( )
		{
			MtaThread.Run (delegate
			{
				TerminateDebugger ();
			});
		}

		protected override void OnFinish ( )
		{
			MtaThread.Run (delegate
			{
				if (stepper != null) {
					stepper.StepOut ().AssertSucceeded("stepper.StepOut ()");;
					ClearEvalStatus ();
					_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null).AssertSucceeded("_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null)");;
					_process.Continue (0).AssertSucceeded("_process.Continue (0)");;
				}
			});
		}

		protected override ProcessInfo[] OnGetProcesses ( )
		{
			return MtaThread.Run (() => new ProcessInfo[] { GetProcess (_process) });
		}

		protected override Backtrace OnGetThreadBacktrace (long processId, long threadId)
		{
			return MtaThread.Run (delegate
			{
				foreach (ICorDebugThread t in _process.GetThreads()) {
					uint dwThreadId;
					t.GetID(&dwThreadId).AssertSucceeded("Could not get the ID of a Thread.");
					if (dwThreadId == threadId) {
						return new Backtrace (new CorBacktrace (t, this));
					}
				}
				return null;
			});
		}

		protected override ThreadInfo[] OnGetThreads (long processId)
		{
			return MtaThread.Run (delegate
			{
				List<ThreadInfo> list = new List<ThreadInfo> ();
				foreach (ICorDebugThread t in _process.GetThreads())
					list.Add (GetThread (t));
				return list.ToArray ();
			});
		}

		internal ISymbolReader GetReaderForModule(ICorDebugModule module)
		{
			lock(appDomainsLock)
			{
				AppDomainInfo appDomainInfo;
				uint appdomainid;
				ICorDebugAppDomain assemblyappdomain;
				ICorDebugAssembly moduleassembly;
				module.GetAssembly(out moduleassembly).AssertSucceeded("Could not get the Assembly of a Module.");
				moduleassembly.GetAppDomain(out assemblyappdomain).AssertSucceeded("Could not get the AppDomain of an Assembly.");
				assemblyappdomain.GetID(&appdomainid).AssertSucceeded("Could not get the ID of an AppDomain.");
				if(!appDomains.TryGetValue(appdomainid, out appDomainInfo))
					return null;
				ModuleInfo moduleInfo;
				if(!appDomainInfo.Modules.TryGetValue(LpwstrHelper.GetString(module.GetName, "Could not get the Name of a Module."), out moduleInfo))
					return null;
				return moduleInfo.Reader;
			}
		}

		internal CorMetadataImport GetMetadataForModule(ICorDebugModule module)
		{
			lock(appDomainsLock)
			{
				AppDomainInfo appDomainInfo;
				uint appdomainid;
				ICorDebugAppDomain assemblyappdomain;
				ICorDebugAssembly moduleassembly;
				module.GetAssembly(out moduleassembly).AssertSucceeded("Could not get the Assembly of a Module.");
				moduleassembly.GetAppDomain(out assemblyappdomain).AssertSucceeded("Could not get the AppDomain of an Assembly.");
				assemblyappdomain.GetID(&appdomainid).AssertSucceeded("Could not get the ID of an AppDomain.");
				if(!appDomains.TryGetValue(appdomainid, out appDomainInfo))
					return null;
				ModuleInfo mod;
				if(!appDomainInfo.Modules.TryGetValue(LpwstrHelper.GetString(module.GetName, "Could not get the Name of a Module."), out mod))
					return null;
				return mod.Importer;
			}
		}

		internal IEnumerable<string> GetAllDocumentPaths()
		{
			lock (appDomainsLock) {
				var documentFileNames = new HashSet<string>();
				foreach (var appDomainInfo in appDomains) {
					foreach (var fileName in appDomainInfo.Value.Documents.Keys)
					{
						documentFileNames.Add(fileName);
					}
				}
				return documentFileNames;
			}
		}

		internal IEnumerable<ICorDebugAppDomain> GetAppDomains ()
		{
			lock (appDomainsLock) {
				var corAppDomains = new List<ICorDebugAppDomain> (appDomains.Count);
				foreach (var appDomainInfo in appDomains) {
					corAppDomains.Add (appDomainInfo.Value.AppDomain);
				}
				return corAppDomains;
			}
		}

		internal IEnumerable<ICorDebugModule> GetModules (ICorDebugAppDomain appDomain)
		{
			lock (appDomainsLock) {
				List<ICorDebugModule> mods = new List<ICorDebugModule> ();
				AppDomainInfo appDomainInfo;
				uint appDomainId=0;
				appDomain.GetID(&appDomainId).AssertSucceeded("Could not get the ID of an AppDomain.");
				if (appDomains.TryGetValue (appDomainId, out appDomainInfo)) {
					foreach (ModuleInfo mod in appDomainInfo.Modules.Values) {
						mods.Add (mod.Module);
					}
				}
				return mods;
			}
		}

		internal IEnumerable<ICorDebugModule> GetAllModules ()
		{
			lock (appDomainsLock) {
				var corModules = new List<ICorDebugModule> ();
				foreach (var appDomainInfo in appDomains) {
					corModules.AddRange (GetModules (appDomainInfo.Value.AppDomain));
				}
				return corModules;
			}
		}

		internal ICorDebugHandleValue GetHandle(ICorDebugValue val)
		{
			ICorDebugHandleValue handleVal;
			ulong qwAddress = 0;
			val.GetAddress(&qwAddress).AssertSucceeded("val.GetAddress(&qwAddress)");
			if(!handles.TryGetValue(qwAddress, out handleVal))
			{
				handleVal = val as ICorDebugHandleValue;
				if(handleVal == null)
				{
					// Create a handle
					var refVal = Com.QueryInteface<ICorDebugReferenceValue>(val);
					ICorDebugValue deref;
					refVal.Dereference(out deref).AssertSucceeded("refVal.Dereference (out deref)");
					var heapVal = Com.QueryInteface<ICorDebugHeapValue2>(deref);
					heapVal.CreateHandle(CorDebugHandleType.HANDLE_STRONG, out handleVal).AssertSucceeded("heapVal.CreateHandle (CorDebugHandleType.HANDLE_STRONG, out handleVal)");
				}
				handles.Add(qwAddress, handleVal);
			}
			return handleVal;
		}

		protected override BreakEventInfo OnInsertBreakEvent (BreakEvent be)
		{
			return MtaThread.Run (delegate {
				var binfo = new BreakEventInfo ();
				var bp = be as Breakpoint;
				if (bp != null) {
					if (bp is FunctionBreakpoint) {
						// FIXME: implement breaking on function name
						binfo.SetStatus (BreakEventStatus.Invalid, "Function breakpoint is not implemented");
						return binfo;
					}
					else {
						var docInfos = new List<DocInfo> ();
						lock (appDomainsLock) {
							foreach (var appDomainInfo in appDomains) {
								var documents = appDomainInfo.Value.Documents;
								DocInfo docInfo = null;
								if (documents.TryGetValue (Path.GetFullPath (bp.FileName), out docInfo)) {
									docInfos.Add (docInfo);
								}
							}
						}

						var doc = docInfos.FirstOrDefault (); //get info about source position using SymbolReader of first DocInfo

						if (doc == null) {
							binfo.SetStatus (BreakEventStatus.NotBound, string.Format("{0} is not found among the loaded symbol documents", bp.FileName));
							return binfo;
						}
						int line;
						try {
							line = doc.Document.FindClosestLine (bp.Line);
						} catch {
							// Invalid line
							binfo.SetStatus (BreakEventStatus.Invalid, string.Format("Invalid line {0}", bp.Line));
							return binfo;
						}
						ISymbolMethod[] methods = null;
						if (doc.ModuleInfo.Reader is ISymbolReader2) {
							methods = ((ISymbolReader2)doc.ModuleInfo.Reader).GetMethodsFromDocumentPosition (doc.Document, line, 0);
						}
						if (methods == null || methods.Length == 0) {
							var met = doc.ModuleInfo.Reader.GetMethodFromDocumentPosition (doc.Document, line, 0);
							if (met != null)
								methods = new ISymbolMethod[] {met};
						}

						if (methods == null || methods.Length == 0) {
							binfo.SetStatus (BreakEventStatus.Invalid, "Unable to resolve method at position");
							return binfo;
						}

						ISymbolMethod bestMethod = null;
						ISymbolMethod bestLeftSideMethod = null;
						ISymbolMethod bestRightSideMethod = null;

						SequencePoint bestSp = null;
						SequencePoint bestLeftSideSp = null;
						SequencePoint bestRightSideSp = null;

						foreach (var met in methods) {
							foreach (SequencePoint sp in met.GetSequencePoints ()) {
								if (sp.IsInside (doc.Document.URL, line, bp.Column)) {	//breakpoint is inside current sequence point
									if (bestSp == null || bestSp.IsInside (doc.Document.URL, sp.StartLine, sp.StartColumn)) {	//and sp is inside of current candidate
										bestSp = sp;
										bestMethod = met;
										break;
									}
								} else if (sp.StartLine == line
								           && sp.Document.URL.Equals (doc.Document.URL, StringComparison.OrdinalIgnoreCase)
								           && sp.StartColumn <= bp.Column) {	//breakpoint is on the same line and on the right side of sp
									if (bestLeftSideSp == null
									    || bestLeftSideSp.EndColumn < sp.EndColumn) {
										bestLeftSideSp = sp;
										bestLeftSideMethod = met;
									}
								} else if (sp.StartLine >= line
								           && sp.Document.URL.Equals (doc.Document.URL, StringComparison.OrdinalIgnoreCase)) {	//sp is after bp
									if (bestRightSideSp == null
									    || bestRightSideSp.StartLine > sp.StartLine
									    || (bestRightSideSp.StartLine == sp.StartLine && bestRightSideSp.StartColumn > sp.StartColumn)) { //and current candidate is on the right side of it
										bestRightSideSp = sp;
										bestRightSideMethod = met;
									}
								}
							}
						}

						SequencePoint bestSameLineSp;
						ISymbolMethod bestSameLineMethod;

						if (bestRightSideSp != null
						    && (bestLeftSideSp == null
						        || bestRightSideSp.StartLine > line)) {
							bestSameLineSp = bestRightSideSp;
							bestSameLineMethod = bestRightSideMethod;
						}
						else {
							bestSameLineSp = bestLeftSideSp;
							bestSameLineMethod = bestLeftSideMethod;
						}

						if (bestSameLineSp != null) {
							if (bestSp == null) {
								bestSp = bestSameLineSp;
								bestMethod = bestSameLineMethod;
							}
							else {
								if (bp.Line != bestSp.StartLine || bestSp.StartColumn != bp.Column) {
									bestSp = bestSameLineSp;
									bestMethod = bestSameLineMethod;
								}
							}
						}

						if (bestSp == null || bestMethod == null) {
							binfo.SetStatus (BreakEventStatus.Invalid, "Unable to calculate an offset in IL code");
							return binfo;
						}

						foreach (var docInfo in docInfos) {
							ICorDebugFunction func;
							docInfo.ModuleInfo.Module.GetFunctionFromToken (((uint)bestMethod.Token.GetToken ()), out func).AssertSucceeded("docInfo.ModuleInfo.Module.GetFunctionFromToken (((uint)bestMethod.Token.GetToken ()), out func)");;

							ICorDebugCode ilcode;
							var hrIlCode = (HResult)func.GetILCode(out ilcode);
							HandleBreakpointFailure(binfo, hrIlCode).Assert("Could not get the IL Code of a Function to set a Breakpoint.");
							ICorDebugFunctionBreakpoint corBp ;
							var hrCreateBp = (HResult)ilcode.CreateBreakpoint (((uint)bestSp.Offset), out corBp);
							HandleBreakpointFailure (binfo, hrCreateBp).Assert("Could not create the breakpoint.");

							breakpoints[corBp] = binfo;

							if (binfo.Handle == null)
								binfo.Handle = new List<ICorDebugFunctionBreakpoint> ();
							((List<ICorDebugFunctionBreakpoint>)binfo.Handle).Add (corBp);

							var hrActivateBp = (HResult)corBp.Activate (bp.Enabled.ToInt());
							HandleBreakpointFailure (binfo, hrActivateBp).Assert("Could not activate the breakpoint.");

							binfo.SetStatus (BreakEventStatus.Bound, null);
						}
						return binfo;
					}
				}

				var cp = be as Catchpoint;
				if (cp != null) {
					var bound = false;
					lock (appDomainsLock) {
						foreach (var appDomainInfo in appDomains) {
							foreach (ModuleInfo mod in appDomainInfo.Value.Modules.Values) {
								CorMetadataImport mi = mod.Importer;
								if (mi != null) {
									foreach (Type t in mi.DefinedTypes)
										if (t.FullName == cp.ExceptionName) {
											bound = true;
										}
								}
							}
						}
					}
					if (bound) {
						binfo.SetStatus (BreakEventStatus.Bound, null);
						return binfo;
					}
				}

				binfo.SetStatus (BreakEventStatus.Invalid, null);
				return binfo;
			});
		}

		private static HResults HandleBreakpointFailure(BreakEventInfo binfo, HResult hr)
		{
			switch(hr)
			{
			case HResult.CORDBG_E_UNABLE_TO_SET_BREAKPOINT:
				binfo.SetStatus(BreakEventStatus.Invalid, "Invalid breakpoint position");
				return HResults.S_OK;
			case HResult.CORDBG_E_PROCESS_TERMINATED:
				binfo.SetStatus(BreakEventStatus.BindError, "Process terminated");
				return HResults.S_OK;
			case HResult.CORDBG_E_CODE_NOT_AVAILABLE:
				binfo.SetStatus(BreakEventStatus.BindError, "Module is not loaded");
				return HResults.S_OK;
			default:
				binfo.SetStatus(BreakEventStatus.BindError, HResultHelpers.GetExceptionIfFailed(((int)hr)).NotNull("It failed.").Message);
				if(!typeof(HResult).IsEnumDefined(((int)hr)))
					DebuggerLoggingService.LogError("Unknown exception when setting breakpoint.", HResultHelpers.GetExceptionIfFailed(((int)hr)));
				return HResults.S_OK;
			}
		}

		protected override void OnCancelAsyncEvaluations ()
		{
			ObjectAdapter.CancelAsyncOperations ();
		}

		protected override void OnNextInstruction ( )
		{
			MtaThread.Run (delegate {
				Step (false);
			});
		}

		protected override void OnNextLine ( )
		{
			MtaThread.Run (delegate
			{
				Step (false);
			});
		}

		void Step (bool isInto)
		{
			try {
				ObjectAdapter.CancelAsyncOperations ();
				if (stepper != null) {
					ICorDebugFrame activeframe;
					activeThread.GetActiveFrame(out activeframe).AssertSucceeded("Could not get the Active Frame of a Thread.");
					ICorDebugFunction framefunction;
					activeframe.GetFunction(out framefunction).AssertSucceeded("Could not get the Function of a Frame.");
					ICorDebugModule functionmodule;
					framefunction.GetModule(out functionmodule).AssertSucceeded("Could not get the Module of a Function.");
					ISymbolReader reader = GetReaderForModule (functionmodule);
					if (reader == null) {
						RawContinue (isInto);
						return;
					}
					uint mdMethodDef;
					framefunction.GetToken(&mdMethodDef).AssertSucceeded("framefunction.GetToken(&mdMethodDef)");
					ISymbolMethod met = reader.GetMethod (new SymbolToken (((int)mdMethodDef)));
					if (met == null) {
						RawContinue (isInto);
						return;
					}

					uint offset;
					var activeframeIl = activeframe as ICorDebugILFrame;
					if(activeframeIl == null)
						offset = 0;
					else
					{
						CorDebugMappingResult mappingResultDummy;
						activeframeIl.GetIP(&offset, &mappingResultDummy).AssertSucceeded("activeframeIl.GetIP (&offset, &mappingResult)");
					}

					// Exclude all ranges belonging to the current line
					List<COR_DEBUG_STEP_RANGE> ranges = new List<COR_DEBUG_STEP_RANGE> ();
					var sequencePoints = met.GetSequencePoints ().ToArray ();
					for (int i = 0; i < sequencePoints.Length; i++) {
						if (sequencePoints [i].Offset > offset) {
							var r = new COR_DEBUG_STEP_RANGE ();
							r.startOffset = i == 0 ? 0 : (uint)sequencePoints [i - 1].Offset;
							r.endOffset = (uint)sequencePoints [i].Offset;
							ranges.Add (r);
							break;
						}
					}
					if (ranges.Count == 0 && sequencePoints.Length > 0) {
						var r = new COR_DEBUG_STEP_RANGE ();
						r.startOffset = (uint)sequencePoints [sequencePoints.Length - 1].Offset;
						r.endOffset = uint.MaxValue;
						ranges.Add (r);
					}

					fixed(COR_DEBUG_STEP_RANGE* pranges = ranges.ToArray())
						stepper.StepRange(isInto.ToInt(), pranges, ((uint)ranges.Count)).AssertSucceeded($"Could not Step {(isInto ? "Into" : "Over")}.");

					ClearEvalStatus ();
					_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null).AssertSucceeded("_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_RUN, null)");;
					_process.Continue (0).AssertSucceeded("_process.Continue (0)");;
				}
			} catch (Exception e) {
				DebuggerLoggingService.LogError ("Exception on Step()", e);
			}
		}

		private void RawContinue(bool isInto, bool stepOverAll = false)
		{
			if(stepOverAll)
			{
				var range = new COR_DEBUG_STEP_RANGE() {startOffset = 0, endOffset = uint.MaxValue};
				stepper.StepRange(isInto.ToInt(), &range, 1).AssertSucceeded($"Could not Step {(isInto ? "Into" : "Over")}.");
			}
			else
				stepper.Step(isInto.ToInt()).AssertSucceeded($"Could not Step {(isInto ? "Into" : "Over")}.");
			ClearEvalStatus();
			_process.Continue(false.ToInt()).AssertSucceeded("_process.Continue (false.ToInt())");
		}

		protected override void OnRemoveBreakEvent(BreakEventInfo bi)
		{
			if(terminated)
				return;

			if(bi.Status != BreakEventStatus.Bound || bi.Handle == null)
				return;

			MtaThread.Run(delegate
			{
				var corBpList = (List<ICorDebugFunctionBreakpoint>)bi.Handle;
				foreach(ICorDebugFunctionBreakpoint corBp in corBpList)
				{
					var hrBp = ((HResult)corBp.Activate(false.ToInt()));
					HandleBreakpointFailure(bi, hrBp).Assert("Could not deactivate the breakpoint.");
				}
			});
		}


		protected override void OnSetActiveThread (long processId, long threadId)
		{
			MtaThread.Run (delegate
			{
				activeThread = null;
				if (stepper != null)
				{
					int isStepperActive;
					stepper.IsActive(&isStepperActive).AssertSucceeded("Could not get if a Stepper is Active.");
					if(isStepperActive.ToBool())
						stepper.Deactivate ().AssertSucceeded("stepper.Deactivate ()");
				}
				stepper = null;
				foreach (ICorDebugThread t in _process.GetThreads()) {
					uint dwThreadId;
					t.GetID(&dwThreadId).AssertSucceeded("Could not get the ID of a Thread.");
					if (dwThreadId == threadId) {
						SetActiveThread (t);
						break;
					}
				}
			});
		}

		private void SetActiveThread(ICorDebugThread t)
		{
			activeThread = t;
			if(stepper != null)
			{
				int isStepperActive;
				stepper.IsActive(&isStepperActive).AssertSucceeded("Could not get if a Stepper is Active.");
				if(isStepperActive.ToBool())
					stepper.Deactivate().AssertSucceeded("stepper.Deactivate ()");
			}
			activeThread.CreateStepper(out stepper).AssertSucceeded("activeThread.CreateStepper (out stepper)");
			stepper.SetUnmappedStopMask(CorDebugUnmappedStop.STOP_NONE).AssertSucceeded("stepper.SetUnmappedStopMask (CorDebugUnmappedStop.STOP_NONE)");
			var stepper2 = stepper as ICorDebugStepper2;
			if(stepper2 != null)
				stepper2.SetJMC(1).AssertSucceeded("Could not set Just My Code on the Stepper.");
		}

		protected override void OnStepInstruction ( )
		{
			MtaThread.Run (delegate {
				Step (true);
			});
		}

		protected override void OnStepLine ( )
		{
			MtaThread.Run (delegate
			{
				Step (true);
			});
		}

		protected override void OnStop ( )
		{
			TargetEventArgs args = new TargetEventArgs (TargetEventType.TargetStopped);

			MtaThread.Run (delegate
			{
				_process.Stop (0).AssertSucceeded("_process.Stop (0)");;
				OnStopped ();
				ICorDebugThread currentThread = null;
				foreach (ICorDebugThread t in _process.GetThreads()) {
					currentThread = t;
					break;
				}
				args.Process = GetProcess (_process);
				args.Thread = GetThread (currentThread);
				args.Backtrace = new Backtrace (new CorBacktrace (currentThread, this));
			});
			OnTargetEvent (args);
		}

		protected override void OnUpdateBreakEvent (BreakEventInfo be)
		{
		}

		public ICorDebugValue RuntimeInvoke (CorEvaluationContext ctx, ICorDebugFunction function, ICorDebugType[] typeArgs, ICorDebugValue thisObj, ICorDebugValue[] arguments)
		{
			ICorDebugValue[] args;
			if (thisObj == null)
				args = arguments;
			else {
				args = new ICorDebugValue[arguments.Length + 1];
				args[0] = thisObj;
				arguments.CopyTo (args, 1);
			}

			var methodCall = new CorMethodCall (ctx, function, typeArgs, args);
			try {
				var result = ObjectAdapter.InvokeSync (methodCall, ctx.Options.EvaluationTimeout);
				if (result.ResultIsException) {
					var vref = new CorValRef (result.Result);
					throw new EvaluatorExceptionThrownException (vref, ObjectAdapter.GetValueTypeName (ctx, vref));
				}

				WaitUntilStopped ();
				return result.Result;
			}
			catch (COMException ex) {
				// eval exception is a 'good' exception that should be shown in value box
				// all other exceptions must be thrown to error log
				var evalException = TryConvertToEvalException (ex);
				if (evalException != null)
					throw evalException;
				throw;
			}
		}

		internal void OnStartEvaluating ( )
		{
			lock (debugLock) {
				evaluating = true;
			}
		}

		internal void OnEndEvaluating ( )
		{
			lock (debugLock) {
				evaluating = false;
				Monitor.PulseAll (debugLock);
			}
		}

		private ICorDebugValue NewSpecialObject(CorEvaluationContext ctx, Action<ICorDebugEval> createCall)
		{
			var doneEvent = new ManualResetEvent(false);
			ICorDebugValue result = null;
			ICorDebugEval eval = ctx.Eval;
			DebugEventHandler<CorEvalEventArgs> completeHandler = delegate(object o, CorEvalEventArgs eargs)
			{
				try
				{
					if(eargs.Eval != eval)
						return;
					eargs.Eval.GetResult(out result).AssertSucceeded("eargs.Eval.GetResult(out result)");
				}
				finally
				{
					doneEvent.Set();
				}
				eargs.Continue = false;
			};

			DebugEventHandler<CorEvalEventArgs> exceptionHandler = delegate(object o, CorEvalEventArgs eargs)
			{
				try
				{
					if(eargs.Eval != eval)
						return;
					eargs.Eval.GetResult(out result).AssertSucceeded("eargs.Eval.GetResult(out result)");
				}
				finally
				{
					doneEvent.Set();
				}
				eargs.Continue = false;
			};
			_process.OnEvalComplete += completeHandler;
			_process.OnEvalException += exceptionHandler;

			try
			{
				createCall(eval);
				_process.SetAllThreadsDebugState(CorDebugThreadState.THREAD_SUSPEND, ctx.Thread).AssertSucceeded("_process.SetAllThreadsDebugState (CorDebugThreadState.THREAD_SUSPEND, ctx.Thread)");
				;
				OnStartEvaluating();
				ClearEvalStatus();
				_process.Continue(0).AssertSucceeded("_process.Continue (0)");
				;

				if(doneEvent.WaitOne(ctx.Options.EvaluationTimeout, false))
					return result;
				else
				{
					eval.Abort().AssertSucceeded("eval.Abort ()");
					return null;
				}
			}
			catch(COMException ex)
			{
				EvaluatorException evalException = TryConvertToEvalException(ex);
				// eval exception is a 'good' exception that should be shown in value box
				// all other exceptions must be thrown to error log
				if(evalException != null)
					throw evalException;
				throw;
			}
			finally
			{
				_process.OnEvalComplete -= completeHandler;
				_process.OnEvalException -= exceptionHandler;
				OnEndEvaluating();
			}
		}

		public ICorDebugValue NewString (CorEvaluationContext ctx, string value)
		{
			return NewSpecialObject (ctx, eval =>
			{
				fixed(char *pch = value)
					eval.NewString(((ushort*)pch)).AssertSucceeded("eval.NewString(((ushort*)pch))");
			});
		}

		public ICorDebugValue NewArray(CorEvaluationContext ctx, ICorDebugType elemType, int size)
		{
			return NewSpecialObject(ctx, eval =>
			{
				uint nRank = 1;
				uint nSingleDim = ((uint)size);
				uint nSingleLowBound = 0;
				Com.QueryInteface<ICorDebugEval2>(eval).NewParameterizedArray(elemType, nRank, &nSingleDim, &nSingleLowBound).AssertSucceeded($"Could not create a new array of size {size:N0}.");
			});
		}

		private static EvaluatorException TryConvertToEvalException(COMException ex)
		{
			string message = null;
			switch((HResult)ex.ErrorCode)
			{
			case HResult.CORDBG_E_ILLEGAL_AT_GC_UNSAFE_POINT:
				message = "The thread is not at a GC-safe point";
				break;
			case HResult.CORDBG_E_ILLEGAL_IN_PROLOG:
				message = "The thread is in the prolog";
				break;
			case HResult.CORDBG_E_ILLEGAL_IN_NATIVE_CODE:
				message = "The thread is in native code";
				break;
			case HResult.CORDBG_E_ILLEGAL_IN_OPTIMIZED_CODE:
				message = "The thread is in optimized code";
				break;
			case HResult.CORDBG_E_FUNC_EVAL_BAD_START_POINT:
				message = "Bad starting point to perform evaluation";
				break;
			}
			if(message != null)
				return new EvaluatorException("Evaluation is not allowed: {0}", message);
			return null;
		}


		public void WaitUntilStopped ()
		{
			lock (debugLock) {
				while (evaluating)
					Monitor.Wait (debugLock);
			}
		}

		internal void ClearEvalStatus()
		{
			ICorDebugProcessEnum enumProcesses;
			_dbg.EnumerateProcesses(out enumProcesses).AssertSucceeded("Could not enumerate all processes in the debugger.");
			IList<ICorDebugProcess> procs = enumProcesses.ToList<ICorDebugProcessEnum, ICorDebugProcess>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
			foreach(ICorDebugProcess p in procs)
			{
				uint dwProcessId;
				p.GetID(&dwProcessId).AssertSucceeded("Could not get the ID of a Process.");
				if(dwProcessId != _processId)
					continue;
				_process = p;
				break;
			}
		}
		
		void ClearHandles ( )
		{
			foreach (ICorDebugHandleValue handle in handles.Values) {
				// The underlying ICorDebugHandle has a  Dispose() method which will free
				// its resources (a GC handle). We call that now to free things sooner.
				// If we don't call it now, it will still get freed at some random point after
				// the final release (which the finalizer will call).
				try
				{
					// This is just a best-effort to cleanup resources early.
					// If it fails, just swallow and move on.
					// May throw if handle was already disposed, or if process is not stopped.
					handle.Dispose().AssertSucceeded("handle.Dispose()");    
				}
				catch
				{
					// swallow all
					// TODO: log to Logger
				}
			}
			handles.Clear ();
		}

		private ProcessInfo GetProcess(ICorDebugProcess proc)
		{
			ProcessInfo info;
			lock(processes)
			{
				uint dwProcessId;
				proc.GetID(&dwProcessId).AssertSucceeded("proc.GetID(&dwProcessId)");
				if(!processes.TryGetValue(dwProcessId, out info))
				{
					info = new ProcessInfo(dwProcessId, "");
					processes[dwProcessId] = info;
				}
			}
			return info;
		}

		ThreadInfo GetThread (ICorDebugThread thread)
		{
			ThreadInfo info;
			lock (threads) {
				uint dwThreadId;
				thread.GetID(&dwThreadId).AssertSucceeded("Could not get the ID of a Thread.");
				if (!threads.TryGetValue (dwThreadId, out info)) {
					string loc = string.Empty;
					try
					{
						ICorDebugFrame activeframe;
						thread.GetActiveFrame(out activeframe).AssertSucceeded("Could not get the Active Frame of a Thread.");
						if (activeframe != null) {
							StackFrame stackframe = CorBacktrace.CreateFrame (this, activeframe);
							loc = stackframe.ToString ();
						}
						else {
							loc = "<Unknown>";
						}
					}
					catch {
						loc = "<Unknown>";
					}

					uint dwProcessId;
					ICorDebugProcess process;
					thread.GetProcess(out process).AssertSucceeded("Could not get the Process of a Thread.");
					process.GetID(&dwProcessId).AssertSucceeded("Could not get the ID of a Process.");
					info = new ThreadInfo (dwProcessId, dwThreadId, GetThreadName (thread), loc);
					threads[dwThreadId] = info;
				}
				return info;
			}
		}

		public ICorDebugThread GetThread (int id)
		{
			try {
				WaitUntilStopped ();
				foreach (ICorDebugThread t in _process.GetThreads())
				{
					uint dwThreadId;
					t.GetID(&dwThreadId).AssertSucceeded("Could not get the ID of a Thread.");
					if (dwThreadId == id)
						return t;
				}
				throw new InvalidOperationException ("Invalid thread id " + id);
			}
			catch {
				throw;
			}
		}

		private string GetThreadName(ICorDebugThread thread)
		{
			// From http://social.msdn.microsoft.com/Forums/en/netfxtoolsdev/thread/461326fe-88bd-4a6b-82a9-1a66b8e65116
			try
			{
				ICorDebugValue threadobject;
				thread.GetObject(out threadobject).AssertSucceeded("thread.GetObject(out threadobject)");
				var refVal = Com.QueryInteface<ICorDebugReferenceValue>(threadobject);
				int bNull = 0;
				refVal.IsNull(&bNull).AssertSucceeded("Could not get if the Reference Value is NULL.");
				if(bNull != 0)
					return string.Empty;

				ICorDebugValue refValDereferenced;
				refVal.Dereference(out refValDereferenced).AssertSucceeded("refVal.Dereference(out refValDereferenced)");
				var val = Com.QueryInteface<ICorDebugObjectValue>(refVal);
				if(val != null)
				{
					Type classType = val.GetExactType().GetTypeInfo(this);
					// Loop through all private instance fields in the thread class 
					foreach(FieldInfo fi in classType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
					{
						if(fi.Name == "m_Name")
						{
							ICorDebugValue fieldValueRaw;
							ICorDebugClass @class;
							val.GetClass(out @class).AssertSucceeded("val.GetClass(out @class)");
							val.GetFieldValue(@class, ((uint)fi.MetadataToken), out fieldValueRaw).AssertSucceeded("val.GetFieldValue(val.Class, ((uint)fi.MetadataToken),out fieldValue)");
							var fieldValue = (ICorDebugReferenceValue)fieldValueRaw;

							int bNull1 = 0;
							fieldValue.IsNull(&bNull1).AssertSucceeded("Could not get if the Reference Value is NULL.");
							if(bNull1 != 0)
								return string.Empty;
							ICorDebugValue fieldValueDereferenced;
							fieldValue.Dereference(out fieldValueDereferenced).AssertSucceeded("fieldValue.Dereference(out dereferenced)");
							var fieldValueDereferencedString = Com.QueryInteface<ICorDebugStringValue>(fieldValueDereferenced);
							return LpwstrHelper.GetString(fieldValueDereferencedString.GetString, "Could not get the Thread Name string.");
						}
					}
				}
			}
			catch(Exception)
			{
				// TODO: write to the Logger
			}

			return string.Empty;
		}
		
		string EvaluateTrace (ICorDebugThread thread, string exp)
		{
			StringBuilder sb = new StringBuilder ();
			int last = 0;
			int i = exp.IndexOf ('{');
			while (i != -1) {
				if (i < exp.Length - 1 && exp [i+1] == '{') {
					sb.Append (exp.Substring (last, i - last + 1));
					last = i + 2;
					i = exp.IndexOf ('{', i + 2);
					continue;
				}
				int j = exp.IndexOf ('}', i + 1);
				if (j == -1)
					break;
				string se = exp.Substring (i + 1, j - i - 1);
				try {
					se = EvaluateExpression (thread, se);
				}
				catch (EvaluatorException e) {
					OnDebuggerOutput (false, e.ToString ());
					return String.Empty;
				}
				sb.Append (exp.Substring (last, i - last));
				sb.Append (se);
				last = j + 1;
				i = exp.IndexOf ('{', last);
			}
			sb.Append (exp.Substring (last, exp.Length - last));
			return sb.ToString ();
		}
		
		string EvaluateExpression (ICorDebugThread thread, string exp)
		{
			try {
				ICorDebugFrame activeframe;
				thread.GetActiveFrame(out activeframe).AssertSucceeded("Could not get the Active Frame of a Thread.");
				if (activeframe == null)
					return string.Empty;
				EvaluationOptions ops = Options.EvaluationOptions.Clone ();
				ops.AllowTargetInvoke = true;
				CorEvaluationContext ctx = new CorEvaluationContext (this, new CorBacktrace (thread, this), 0, ops);
				ctx.Thread = thread;
				ValueReference val = ctx.Evaluator.Evaluate (ctx, exp);
				return val.CreateObjectValue (false).Value;
			}
			catch (EvaluatorException e) {
				throw;
			}
			catch (Exception ex) {
				throw new EvaluatorException (ex.Message);
			}
		}

		protected override T OnWrapDebuggerObject<T> (T obj)
		{
			if (obj is IBacktrace)
				return (T) (object) new MtaBacktrace ((IBacktrace)obj);
			if (obj is IObjectValueSource)
				return (T)(object)new MtaObjectValueSource ((IObjectValueSource)obj);
			if (obj is IObjectValueUpdater)
				return (T)(object)new MtaObjectValueUpdater ((IObjectValueUpdater)obj);
			if (obj is IRawValue)
				return (T)(object)new MtaRawValue ((IRawValue)obj);
			if (obj is IRawValueArray)
				return (T)(object)new MtaRawValueArray ((IRawValueArray)obj);
			if (obj is IRawValueString)
				return (T)(object)new MtaRawValueString ((IRawValueString)obj);
			return obj;
		}

		public override bool CanSetNextStatement {
			get {
				return true;
			}
		}

		protected override void OnSetNextStatement (long threadId, string fileName, int line, int column)
		{
			if (!CanSetNextStatement)
				throw new NotSupportedException ();
			MtaThread.Run (delegate {
				var thread = GetThread ((int)threadId);
				if (thread == null)
					throw new ArgumentException ("Unknown thread.");

				ICorDebugFrame activeframe;
				thread.GetActiveFrame(out activeframe).AssertSucceeded("Could not get the Active Frame of a Thread.");
				if (activeframe == null)
					throw new NotSupportedException ();

				ICorDebugFunction framefunction;
				activeframe.GetFunction(out framefunction).AssertSucceeded("Could not get the Function of a Frame.");
				ISymbolMethod met = framefunction.GetSymbolMethod (this);
				if (met == null) {
					throw new NotSupportedException ();
				}

				int offset = -1;
				int firstSpInLine = -1;
				foreach (SequencePoint sp in met.GetSequencePoints ()) {
					if (sp.IsInside (fileName, line, column)) {
						offset = sp.Offset;
						break;
					} else if (firstSpInLine == -1
					           && sp.StartLine == line
					           && sp.Document.URL.Equals (fileName, StringComparison.OrdinalIgnoreCase)) {
						firstSpInLine = sp.Offset;
					}
				}
				if (offset == -1) {//No exact match? Use first match in that line
					offset = firstSpInLine;
				}
				if (offset == -1) {
					throw new NotSupportedException ("Cannot Set Next Statement because the current line has no Sequence Points in it.");
				}
				try {
					ICorDebugILFrame activeframeIl = (activeframe as ICorDebugILFrame).NotNull("Cannot Set Next Statement because the active Stack Frame is not an IL Frame.");
					activeframeIl.SetIP(((uint)offset)).AssertSucceeded("activeframeIl.SetIP(((uint)offset))");;
					OnStopped ();
					RaiseStopEvent ();
				} catch(Exception ex) {
					throw new NotSupportedException ("Could not Set Next Statement on an IL Stack Frame.", ex);
				}
			});
		}

		public struct CorProcessWithOutputRedirection
		{
			public CorApi.ComInterop.ICorDebugProcess Process;
			public CorDebugProcessOutputRedirection OutputRedirection;
		}
	}

	class SequencePoint
	{
		public int StartLine;
		public int EndLine;
		public int StartColumn;
		public int EndColumn;
		public int Offset;
		public bool IsSpecial;
		public ISymbolDocument Document;

		public bool IsInside (string fileUrl, int line, int column)
		{
			if (!Document.URL.Equals (fileUrl, StringComparison.OrdinalIgnoreCase))
				return false;
			if (line < StartLine || (line == StartLine && column < StartColumn))
				return false;
			if (line > EndLine || (line == EndLine && column > EndColumn))
				return false;
			return true;
		}
	}

	static unsafe class SequencePointExt
	{
		public static IEnumerable<SequencePoint> GetSequencePoints (this ISymbolMethod met)
		{
			int sc = met.SequencePointCount;
			int[] offsets = new int[sc];
			int[] lines = new int[sc];
			int[] endLines = new int[sc];
			int[] columns = new int[sc];
			int[] endColumns = new int[sc];
			ISymbolDocument[] docs = new ISymbolDocument[sc];
			met.GetSequencePoints (offsets, docs, lines, columns, endLines, endColumns);

			for (int n = 0; n < sc; n++) {
				SequencePoint sp = new SequencePoint ();
				sp.Document = docs[n];
				sp.StartLine = lines[n];
				sp.EndLine = endLines[n];
				sp.StartColumn = columns[n];
				sp.EndColumn = endColumns[n];
				sp.Offset = offsets[n];
				yield return sp;
			}
		}

		public static Type GetTypeInfo (this ICorDebugType type, CorDebuggerSession session)
		{
			Type t;
			if (MetadataHelperFunctionsExtensions.CoreTypes.TryGetValue (type.Type(), out t))
				return t;

			if (type.Type() == CorElementType.ELEMENT_TYPE_ARRAY || type.Type() == CorElementType.ELEMENT_TYPE_SZARRAY) {
				List<int> sizes = new List<int> ();
				List<int> loBounds = new List<int> ();
				uint nRank;
				type.GetRank(&nRank).AssertSucceeded("Could not get the Rank of a Type.");
				for (int n = 0; n < (int)nRank; n++) {
					sizes.Add (1);
					loBounds.Add (0);
				}
				ICorDebugType firsttypeparam;
				type.GetFirstTypeParameter(out firsttypeparam).AssertSucceeded("Could not get the First Type Parameter of a Type.");
				return MetadataExtensions.MakeArray (firsttypeparam.GetTypeInfo (session), sizes, loBounds);
			}

			if (type.Type() == CorElementType.ELEMENT_TYPE_BYREF)
			{
				ICorDebugType firsttypeparam;
				type.GetFirstTypeParameter(out firsttypeparam).AssertSucceeded("Could not get the First Type Parameter of a Type.");
				return MetadataExtensions.MakeByRef (firsttypeparam.GetTypeInfo (session));
			}

			if (type.Type() == CorElementType.ELEMENT_TYPE_PTR)
			{
				ICorDebugType firsttypeparam;
				type.GetFirstTypeParameter(out firsttypeparam).AssertSucceeded("Could not get the First Type Parameter of a Type.");
				return MetadataExtensions.MakePointer (firsttypeparam.GetTypeInfo (session));
			}

			ICorDebugClass typeclass;
			type.GetClass(out typeclass).AssertSucceeded("Could not get the Class of a Type.");
			ICorDebugModule classmodule;
			typeclass.GetModule(out classmodule).AssertSucceeded("Could not get the Module of a Class.");
			CorMetadataImport mi = session.GetMetadataForModule (classmodule);
			if (mi != null) {
				uint mdTypeDef;
				typeclass.GetToken(&mdTypeDef).AssertSucceeded("Could not get the mdTypeDef Token of a Class.");
				t = mi.GetType (mdTypeDef);
				ICorDebugType[] targs = type.TypeParameters();
				if (targs.Length > 0) {
					List<Type> types = new List<Type> ();
					foreach (ICorDebugType ct in targs)
						types.Add (ct.GetTypeInfo (session));
					return MetadataExtensions.MakeGeneric (t, types);
				}
				else
					return t;
			}
			else
				return null;
		}

		public static ISymbolMethod GetSymbolMethod (this ICorDebugFunction func, CorDebuggerSession session)
		{
			ICorDebugModule functionmodule;
			func.GetModule(out functionmodule).AssertSucceeded("Could not get the Module of a Function.");
			ISymbolReader reader = session.GetReaderForModule (functionmodule);
			if (reader == null)
				return null;
			uint mdMethodDef;
			func.GetToken(&mdMethodDef).AssertSucceeded("func.GetToken(&mdMethodDef)");
			return reader.GetMethod (new SymbolToken (((int)mdMethodDef)));
		}

		public static MethodInfo GetMethodInfo(this ICorDebugFunction func, CorDebuggerSession session)
		{
			ICorDebugModule functionmodule;
			func.GetModule(out functionmodule).AssertSucceeded("Could not get the Module of a Function.");
			CorMetadataImport mi = session.GetMetadataForModule(functionmodule);
			if(mi == null)
				return null;
			uint mdMethodDef;
			func.GetToken(&mdMethodDef).AssertSucceeded();
			return mi.GetMethodInfo(mdMethodDef);
		}

		public static void SetValue (this CorValRef thisVal, EvaluationContext ctx, CorValRef val)
		{
			CorEvaluationContext cctx = (CorEvaluationContext) ctx;
			CorObjectAdaptor actx = (CorObjectAdaptor) ctx.Adapter;
			if (actx.IsEnum (ctx, thisVal.Val.GetExactType()) && !actx.IsEnum (ctx, val.Val.GetExactType())) {
				ValueReference vr = actx.GetMember (ctx, null, thisVal, "value__");
				vr.Value = val;
				// Required to make sure that var returns an up-to-date value object
				thisVal.Invalidate ();
				return;
			}

			var s = thisVal.Val as ICorDebugReferenceValue;
			if(s != null)
			{
				var v = val.Val as ICorDebugReferenceValue;
				if(v != null)
				{
					ulong value;
					v.GetValue(&value).AssertSucceeded("v.GetValue(&value)");
					s.SetValue(value).AssertSucceeded("s.SetValue(value)");
					return;
				}
			}
			ICorDebugGenericValue gv = CorObjectAdaptor.GetRealObject (cctx, thisVal.Val) as ICorDebugGenericValue;
			if (gv != null)
				gv.SetValue (ctx.Adapter.TargetObjectToObject (ctx, val));
		}
	}

	public interface ICustomCorSymbolReaderFactory
	{
		ISymbolReader CreateCustomSymbolReader (string assemblyInfo);
	}
}
