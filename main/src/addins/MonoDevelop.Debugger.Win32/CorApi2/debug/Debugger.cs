//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Text;
using System.Security.Permissions;
using System.Globalization;


using CorApi.ComInterop;
using Microsoft.Samples.Debugging.Extensions;
using System.Collections.Generic;

using CorApi.Pinvoke;

using Microsoft.Samples.Debugging.CorPublish.Metahost;
using Microsoft.Win32.SafeHandles;

using IStream = CorApi.ComInterop.IStream;


namespace Microsoft.Samples.Debugging.CorDebug
{
    /**
     * Wraps the native CLR Debugger.
     * Note that we don't derive the class from WrapperBase, becuase this
     * class will never be returned in any callback.
     */
    public sealed unsafe class CorDebugger : MarshalByRefObject
    {
        private const int MaxVersionStringLength = 256; // == MAX_PATH
        
        public static string GetDebuggerVersionFromFile(string pathToExe)
        {
            Debug.Assert( !string.IsNullOrEmpty(pathToExe) );
            if( string.IsNullOrEmpty(pathToExe) )
                throw new ArgumentException("Value cannot be null or empty.", "pathToExe");
            int neededSize;
            StringBuilder sb = new StringBuilder(MaxVersionStringLength);
            NativeMethods.GetRequestedRuntimeVersion(pathToExe, sb, sb.Capacity, out neededSize);
            return sb.ToString();
        }

        public static string GetDebuggerVersionFromPid(int pid)
        {
            using(ProcessSafeHandle ph = NativeMethods.OpenProcess((int)(NativeMethods.ProcessAccessOptions.PROCESS_VM_READ |
                                                                         NativeMethods.ProcessAccessOptions.PROCESS_QUERY_INFORMATION |
                                                                         NativeMethods.ProcessAccessOptions.PROCESS_DUP_HANDLE |
                                                                         NativeMethods.ProcessAccessOptions.SYNCHRONIZE),
                                                                   false, // inherit handle
                                                                   pid) )
            {
                if( ph.IsInvalid )
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                int neededSize;
                StringBuilder sb = new StringBuilder(MaxVersionStringLength);
                NativeMethods.GetVersionFromProcess(ph, sb, sb.Capacity, out neededSize);
                return sb.ToString();
            }
        }

        public static List<string> GetProcessLoadedRuntimes (int pid)
        {
            using (ProcessSafeHandle ph = NativeMethods.OpenProcess (
                (int) (NativeMethods.ProcessAccessOptions.PROCESS_VM_READ |
                       NativeMethods.ProcessAccessOptions.PROCESS_QUERY_INFORMATION |
                       NativeMethods.ProcessAccessOptions.PROCESS_DUP_HANDLE |
                       NativeMethods.ProcessAccessOptions.SYNCHRONIZE),
                false, // inherit handle
                pid)) {
                if (ph.IsInvalid)
                    return new List<string> ();
                int neededSize = MaxVersionStringLength;
                IClrMetaHost host;
                NativeMethods.CLRCreateInstance (ref NativeMethods.CLSID_CLRMetaHost,
                    ref NativeMethods.IID_ICLRMetaHost, out host);
                var result = new List<string> ();
                var runtimes = host.EnumerateLoadedRuntimes (ph);
                var array = new object[1];
                int count;
                while (runtimes.Next (1, array, out count) == 0) {
                    var info = array[0] as IClrRuntimeInfo;
                    if (info == null)
                        continue;
                    var stringBuilder = new StringBuilder (MaxVersionStringLength);
                    info.GetVersionString (stringBuilder, ref neededSize);
                    result.Add (stringBuilder.ToString ());
                }
                return result;
            }
        }

        public static string GetDefaultDebuggerVersion()
        {
            int size;
            NativeMethods.GetCORVersion(null,0,out size);
            Debug.Assert(size>0);
            StringBuilder sb = new StringBuilder(size);
            int hr = NativeMethods.GetCORVersion(sb,sb.Capacity,out size);
            Marshal.ThrowExceptionForHR(hr);
            return sb.ToString();
        }
     

        /// <summary>Creates a debugger wrapper from Guid.</summary>
        public CorDebugger(Guid debuggerGuid)
        {
            ICorDebug rawDebuggingAPI;
            NativeMethods.CoCreateInstance(ref debuggerGuid,
                                           IntPtr.Zero, // pUnkOuter
                                           1, // CLSCTX_INPROC_SERVER
                                           ref NativeMethods.IIDICorDebug,
                                           out rawDebuggingAPI);
            InitFromICorDebug(rawDebuggingAPI);
        }
        /// <summary>Creates a debugger interface that is able debug requested verison of CLR</summary>
        /// <param name="debuggerVersion">Version number of the debugging interface.</param>
        /// <remarks>The version number is usually retrieved either by calling one of following mscoree functions:
        /// GetCorVerison, GetRequestedRuntimeVersion or GetVersionFromProcess.</remarks>
        public CorDebugger (string debuggerVersion)
        {
            InitFromVersion(debuggerVersion);
        }


        [CLSCompliant(false)]
        public CorDebugger (ICorDebug corDebug)
        {
            InitFromICorDebug (corDebug);
        }


        ~CorDebugger()
        {
            if(m_debugger!=null)
                try 
                {
                    Terminate();
                } 
                catch
                {
                    // sometimes we cannot terminate because GC collects object in wrong
                    // order. But since the whole process is shutting down, we really
                    // don't care.
                    
                }
        }


        /**
         * Closes the debugger.  After this method is called, it is an error
         * to call any other methods on this object.
         */
        public void Terminate ()
        {
            Debug.Assert(m_debugger!=null);
            ICorDebug d= m_debugger;
            m_debugger = null;
            d.Terminate ();
        }

        /**
         * Specify the callback object to use for managed events.
         */
        internal void SetManagedHandler (ICorDebugManagedCallback managedCallback)
        {
            m_debugger.SetManagedHandler (managedCallback);
        }

        /**
         * Specify the callback object to use for unmanaged events.
         */
        internal void SetUnmanagedHandler (ICorDebugUnmanagedCallback nativeCallback)
        {
            m_debugger.SetUnmanagedHandler (nativeCallback);
        }

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         */
        public CorProcess CreateProcess (
                                         String applicationName,
                                         String commandLine
                                         )
        {
            return CreateProcess (applicationName, commandLine, ".");
        }

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         */
        public CorProcess CreateProcess (
                                         String applicationName,
                                         String commandLine,
                                         String currentDirectory
                                         )
		{
			// [Xamarin] ASP.NET Debugging.
			return CreateProcess (applicationName, commandLine, currentDirectory, null, 0);
        }

		/**
		 * Launch a process under the control of the debugger.
		 *
		 * Parameters are the same as the Win32 CreateProcess call.
		 */
		// [Xamarin] ASP.NET Debugging.
		public CorProcess CreateProcess (
										 String applicationName,
										 String commandLine,
										 String currentDirectory,
										 IDictionary<string,string> environment
										 )
		{
			return CreateProcess (applicationName, commandLine, currentDirectory, environment, 0);
		}

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         */
        public CorProcess CreateProcess (
                                         String applicationName,
                                         String commandLine,
                                         String currentDirectory,
										 IDictionary<string,string> environment,
                                         int    flags
                                         )
        {
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION ();

            STARTUPINFOW si = new STARTUPINFOW ();
            si.cb = (uint) Marshal.SizeOf(si);

            // initialize safe handles 
			// [Xamarin] ASP.NET Debugging and output redirection.
			SafeFileHandle outReadPipe = null, errorReadPipe = null;
            Action closehandles;
            DebuggerExtensions.SetupOutputRedirection (si, ref flags, out outReadPipe, out errorReadPipe, out closehandles);
			string env = Kernel32Dll.Helpers.GetEnvString(environment);

            CorProcess ret;

            //constrained execution region (Cer)
            System.Runtime.CompilerServices.RuntimeHelpers.PrepareConstrainedRegions();
            try 
            {
            } 
            finally
            {
                ret = CreateProcess (
                                     applicationName,
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
                if (pi.hProcess != null)
                    NativeMethods.CloseHandle ((IntPtr) pi.hProcess);
                if (pi.hThread != null)
                    NativeMethods.CloseHandle ((IntPtr) pi.hThread);
            }

			DebuggerExtensions.TearDownOutputRedirection (outReadPipe, errorReadPipe, ret, closehandles);

            return ret;
        }

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         *
         * The caller should remember to execute:
         *
         *    Microsoft.Win32.Interop.Windows.CloseHandle (
         *      processInformation.hProcess);
         *
         * after CreateProcess returns.
         */
        public CorProcess CreateProcess (
                                         string                      applicationName,
                                         string                      commandLine,
                                         SECURITY_ATTRIBUTES         processAttributes,
                                         SECURITY_ATTRIBUTES         threadAttributes,
                                         bool                        inheritHandles,
                                         int                         creationFlags,
                                         string                      environment,  
                                         string                      currentDirectory,
                                         STARTUPINFOW                 startupInfo,
                                         ref PROCESS_INFORMATION     processInformation,
                                         CorDebugCreateProcessFlags  debuggingFlags)
        {
            /*
             * If commandLine is: <c:\a b\a arg1 arg2> and c:\a.exe does not exist, 
             *    then without this logic, "c:\a b\a.exe" would be tried next.
             * To prevent this ambiguity, this forces the user to quote if the path 
             *    has spaces in it: <"c:\a b\a" arg1 arg2>
             */
            if(null == applicationName && !commandLine.StartsWith("\""))
            {
                var firstSpace = commandLine.IndexOf(" ", StringComparison.Ordinal);
                if(firstSpace != -1)
                    commandLine = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}",
                        commandLine.Substring(0,firstSpace), commandLine.Substring(firstSpace, commandLine.Length-firstSpace));
            }

            ICorDebugProcess proc;
            fixed(char* pApplicationName = applicationName)
            fixed(char* pCommandLine = commandLine)
            fixed(char* pCurrentDirectory = currentDirectory)
            fixed(char* pEnv = environment)
            {
                m_debugger.CreateProcess 
                (
                    (ushort*)pApplicationName,
                    (ushort*)pCommandLine,
                    &processAttributes,
                    &threadAttributes,
                    inheritHandles ? 1 : 0,
                    (uint) creationFlags,
                    pEnv,
                    (ushort*)pCurrentDirectory,
                    &startupInfo,
                    &processInformation,
                    debuggingFlags,
                    out proc
                );
            }

            return CorProcess.GetCorProcess(proc);
        }

        /** 
         * Attach to an active process
         */
        public CorProcess DebugActiveProcess (int processId, bool win32Attach)
        {
            ICorDebugProcess proc = null;
            m_debugger.DebugActiveProcess ((uint)processId, win32Attach ? 1 : 0, out proc);
            return CorProcess.GetCorProcess(proc);
        }

        /**
         * Enumerate all processes currently being debugged.
         */
        public IEnumerable Processes
        {
            get
            {
                ICorDebugProcessEnum eproc = null;
                m_debugger.EnumerateProcesses (out eproc);
                return new CorProcessEnumerator (eproc);
            }
        }

        /**
         * Get the Process object for the given PID.
         */
        public CorProcess GetProcess (int processId)
        {
            ICorDebugProcess proc = null;
            m_debugger.GetProcess ((uint) processId, out proc);
            return CorProcess.GetCorProcess(proc);
        }

        /**
         * Warn us of potentional problems in using debugging (eg. whether a kernel debugger is 
         * attached).  This API should probably be renamed or the warnings turned into errors
         * in CreateProcess/DebugActiveProcess
         */
        public void CanLaunchOrAttach(int processId, bool win32DebuggingEnabled)
        {
            m_debugger.CanLaunchOrAttach((uint) processId,
                                         win32DebuggingEnabled?1:0);
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        // CorDebugger private implement part
        //
        ////////////////////////////////////////////////////////////////////////////////

        // called by constructors during initialization
        private void InitFromVersion(string debuggerVersion)
        {
            if( debuggerVersion.StartsWith("v1") )
            {
                throw new ArgumentException( "Can't debug a version 1 CLR process (\"" + debuggerVersion + 
                    "\").  Run application in a version 2 CLR, or use a version 1 debugger instead." );
            }
            
            ICorDebug rawDebuggingAPI;
			// [Xamarin] .NET 4 API Version.
#if MDBG_FAKE_COM
			// TODO: Ideally, there wouldn't be any difference in the corapi code for MDBG_FAKE_COM.
			// This would require puting this initialization logic into the wrapper and interop assembly, which doesn't seem right.
			// We should also release this pUnk, but doing that here would be difficult and we aren't done with it until
			// we shutdown anyway.
			IntPtr pUnk = NativeMethods.CreateDebuggingInterfaceFromVersion((int)CorDebuggerVersion.Whidbey, debuggerVersion);
			rawDebuggingAPI = new NativeApi.CorDebugClass(pUnk);
#else
			int apiVersion = debuggerVersion.StartsWith ("v4") ? 4 : 3;
			rawDebuggingAPI = NativeMethods.CreateDebuggingInterfaceFromVersion (apiVersion, debuggerVersion);
#endif
		    InitFromICorDebug(rawDebuggingAPI);
    	}
        
        private void InitFromICorDebug(ICorDebug rawDebuggingAPI)
        {
            Debug.Assert(rawDebuggingAPI!=null);
            if( rawDebuggingAPI==null )
                throw new ArgumentException("Cannot be null.","rawDebugggingAPI");
            
            m_debugger = rawDebuggingAPI;
            m_debugger.Initialize ();
            m_debugger.SetManagedHandler (new ManagedCallback(this));
    	}            

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

        void InternalFireEvent(ManagedCallbackType callbackType,CorEventArgs e)
        {
            CorProcess owner;
            ICorDebugController c = e.Controller;
            Debug.Assert(c!=null);
            if(c is CorProcess)
                owner = (CorProcess)c ;
            else 
            {
                Debug.Assert(c is ICorDebugAppDomain);
                owner = (c as ICorDebugAppDomain).Process;
            }
            Debug.Assert(owner!=null);
            try 
            {
                owner.DispatchEvent(callbackType,e);
            }
            finally
            {
                if(e.Continue)
                {
                        e.Controller.Continue(false);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        // ManagedCallback
        //
        ////////////////////////////////////////////////////////////////////////////////

        /**
         * This is the object that gets passed to the debugger.  It's
         * the intermediate "source" of the events, which repackages
         * the event arguments into a more approprate form and forwards
         * the call to the appropriate function.
         */
        private class ManagedCallback : ManagedCallbackBase
        {
            public ManagedCallback (CorDebugger outer)
            {
                m_outer = outer;
            }
            protected override void HandleEvent(ManagedCallbackType eventId, CorEventArgs args)
            {
                m_outer.InternalFireEvent(eventId, args);
            }
            private CorDebugger m_outer;
        }

         
        
        private ICorDebug m_debugger = null;
    } /* class Debugger */


  ////////////////////////////////////////////////////////////////////////////////
    //
    // CorEvent Classes & Corresponding delegates
    //
    ////////////////////////////////////////////////////////////////////////////////

    /**
     * All of the Debugger events make a Controller available (to specify
     * whether or not to continue the program, or to stop, etc.).
     *
     * This serves as the base class for all events used for debugging.
     *
     * NOTE: If you don't want <b>Controller.Continue(false)</b> to be
     * called after event processing has finished, you need to set the
     * <b>Continue</b> property to <b>false</b>.
     */

    public class CorEventArgs : EventArgs
    {
        private ICorDebugController m_controller;

        private bool m_continue;


        private ManagedCallbackType m_callbackType;

        private ICorDebugThread m_thread;

        public CorEventArgs(ICorDebugController controller)
        {
            m_controller = controller;
            m_continue = true;
        }

        public CorEventArgs(ICorDebugController controller, ManagedCallbackType callbackType)
        {
            m_controller = controller;
            m_continue = true;
            m_callbackType = callbackType;
        }

        /** The Controller of the current event. */
        public ICorDebugController Controller
        {
            get
            {
                return m_controller;
            }
        }

        /** 
         * The default behavior after an event is to Continue processing
         * after the event has been handled.  This can be changed by
         * setting this property to false.
         */
        public virtual bool Continue
        {
            get
            {
                return m_continue;
            }
            set
            {
                m_continue = value;
            }
        }

        /// <summary>
        /// The type of callback that returned this CorEventArgs object.
        /// </summary>
        public ManagedCallbackType CallbackType
        {
            get
            {
                return m_callbackType;
            }
        }

        /// <summary>
        /// The CorThread associated with the callback event that returned
        /// this CorEventArgs object. If here is no such thread, Thread is null.
        /// </summary>
        public ICorDebugThread Thread
        {
            get
            {
                return m_thread;
            }
            protected set
            {
                m_thread = value;
            }
        }

    }


    /**
     * This class is used for all events that only have access to the 
     * CorProcess that is generating the event.
     */
    public class CorProcessEventArgs : CorEventArgs
    {
        public CorProcessEventArgs(CorProcess process)
            : base(process)
        {
        }

        public CorProcessEventArgs(CorProcess process, ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
        }

        /** The process that generated the event. */
        public CorProcess Process
        {
            get
            {
                return (CorProcess)Controller;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnCreateProcess:
                    return "Process Created";
                case ManagedCallbackType.OnProcessExit:
                    return "Process Exited";
                case ManagedCallbackType.OnControlCTrap:
                    break;
            }
            return base.ToString();
        }
    }


    /**
     * The event arguments for events that contain both a CorProcess
     * and an CorAppDomain.
     */
    public class CorAppDomainEventArgs : CorProcessEventArgs
    {
        private ICorDebugAppDomain m_ad;

        public CorAppDomainEventArgs(CorProcess process, ICorDebugAppDomain ad)
            : base(process)
        {
            m_ad = ad;
        }

        public CorAppDomainEventArgs(CorProcess process, ICorDebugAppDomain ad,
                                      ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
            m_ad = ad;
        }

        /** The AppDomain that generated the event. */
        public ICorDebugAppDomain AppDomain
        {
            get
            {
                return m_ad;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnCreateAppDomain:
                    return "AppDomain Created: " + m_ad.Name;
                case ManagedCallbackType.OnAppDomainExit:
                    return "AppDomain Exited: " + m_ad.Name;
            }
            return base.ToString();
        }
    }


    /**
     * The base class for events which take an CorAppDomain as their
     * source, but not a CorProcess.
     */
    public class CorAppDomainBaseEventArgs : CorEventArgs
    {
        public CorAppDomainBaseEventArgs(ICorDebugAppDomain ad)
            : base(ad)
        {
        }

        public CorAppDomainBaseEventArgs(ICorDebugAppDomain ad, ManagedCallbackType callbackType)
            : base(ad, callbackType)
        {
        }

        public ICorDebugAppDomain AppDomain
        {
            get
            {
                return (ICorDebugAppDomain)Controller;
            }
        }
    }


    /**
     * Arguments for events dealing with threads.
     */
    public class CorThreadEventArgs : CorAppDomainBaseEventArgs
    {
        public CorThreadEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread)
            : base(appDomain != null ? appDomain : thread.AppDomain)
        {
            Thread = thread;
        }

        public CorThreadEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ManagedCallbackType callbackType)
            : base(appDomain != null ? appDomain : thread.AppDomain, callbackType)
        {
            Thread = thread;
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnBreak:
                    return "Break";
                case ManagedCallbackType.OnCreateThread:
                    return "Thread Created";
                case ManagedCallbackType.OnThreadExit:
                    return "Thread Exited";
                case ManagedCallbackType.OnNameChange:
                    return "Name Changed";
            }
            return base.ToString();
        }
    }


    /**
     * Arguments for events involving breakpoints.
     */
    public class CorBreakpointEventArgs : CorThreadEventArgs
    {
        private ICorDebugBreakpoint m_break;

        public CorBreakpointEventArgs(ICorDebugAppDomain appDomain,
                                       ICorDebugThread thread,
                                       ICorDebugBreakpoint managedBreakpoint)
            : base(appDomain, thread)
        {
            m_break = managedBreakpoint;
        }

        public CorBreakpointEventArgs(ICorDebugAppDomain appDomain,
                                       ICorDebugThread thread,
                                       ICorDebugBreakpoint managedBreakpoint,
                                       ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_break = managedBreakpoint;
        }

        /** The breakpoint involved. */
        public ICorDebugBreakpoint Breakpoint
        {
            get
            {
                return m_break;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnBreakpoint)
            {
                return "Breakpoint Hit";
            }
            return base.ToString();
        }
    }


    /**
     * Arguments for when a Step operation has completed.
     */
    public class CorStepCompleteEventArgs : CorThreadEventArgs
    {
        private ICorDebugStepper m_stepper;
        private CorDebugStepReason m_stepReason;

        [CLSCompliant(false)]
        public CorStepCompleteEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                         ICorDebugStepper stepper, CorDebugStepReason stepReason)
            : base(appDomain, thread)
        {
            m_stepper = stepper;
            m_stepReason = stepReason;
        }

        [CLSCompliant(false)]
        public CorStepCompleteEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                         ICorDebugStepper stepper, CorDebugStepReason stepReason,
                                         ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_stepper = stepper;
            m_stepReason = stepReason;
        }

        public ICorDebugStepper Stepper
        {
            get
            {
                return m_stepper;
            }
        }

        [CLSCompliant(false)]
        public CorDebugStepReason StepReason
        {
            get
            {
                return m_stepReason;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnStepComplete)
            {
                return "Step Complete";
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with exceptions.
     */
    public class CorExceptionEventArgs : CorThreadEventArgs
    {
        bool m_unhandled;

        public CorExceptionEventArgs(ICorDebugAppDomain appDomain,
                                      ICorDebugThread thread,
                                      bool unhandled)
            : base(appDomain, thread)
        {
            m_unhandled = unhandled;
        }

        public CorExceptionEventArgs(ICorDebugAppDomain appDomain,
                                      ICorDebugThread thread,
                                      bool unhandled,
                                      ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_unhandled = unhandled;
        }

        /** Has the exception been handled yet? */
        public bool Unhandled
        {
            get
            {
                return m_unhandled;
            }
        }
    }


    /**
     * For events dealing the evaluation of something...
     */
    public class CorEvalEventArgs : CorThreadEventArgs
    {
        CorEval m_eval;

        public CorEvalEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                 CorEval eval)
            : base(appDomain, thread)
        {
            m_eval = eval;
        }

        public CorEvalEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                 CorEval eval, ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_eval = eval;
        }

        /** The object being evaluated. */
        public CorEval Eval
        {
            get
            {
                return m_eval;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnEvalComplete:
                    return "Eval Complete";
                case ManagedCallbackType.OnEvalException:
                    return "Eval Exception";
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with module loading/unloading.
     */
    public class CorModuleEventArgs : CorAppDomainBaseEventArgs
    {
        ICorDebugModule m_managedModule;

        public CorModuleEventArgs(ICorDebugAppDomain appDomain, ICorDebugModule managedModule)
            : base(appDomain)
        {
            m_managedModule = managedModule;
        }

        public CorModuleEventArgs(ICorDebugAppDomain appDomain, ICorDebugModule managedModule,
            ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            m_managedModule = managedModule;
        }

        public ICorDebugModule Module
        {
            get
            {
                return m_managedModule;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnModuleLoad:
                    return "Module loaded: " + m_managedModule.Name;
                case ManagedCallbackType.OnModuleUnload:
                    return "Module unloaded: " + m_managedModule.Name;
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with class loading/unloading.
     */
    public class CorClassEventArgs : CorAppDomainBaseEventArgs
    {
        ICorDebugClass m_class;

        public CorClassEventArgs(ICorDebugAppDomain appDomain, ICorDebugClass managedClass)
            : base(appDomain)
        {
            m_class = managedClass;
        }

        public CorClassEventArgs(ICorDebugAppDomain appDomain, ICorDebugClass managedClass,
            ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            m_class = managedClass;
        }

        public ICorDebugClass Class
        {
            get
            {
                return m_class;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnClassLoad:
                    return "Class loaded: " + m_class;
                case ManagedCallbackType.OnClassUnload:
                    return "Class unloaded: " + m_class;
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with debugger errors.
     */
    public class CorDebuggerErrorEventArgs : CorProcessEventArgs
    {
        int m_hresult;
        int m_errorCode;

        public CorDebuggerErrorEventArgs(CorProcess process, int hresult,
                                          int errorCode)
            : base(process)
        {
            m_hresult = hresult;
            m_errorCode = errorCode;
        }

        public CorDebuggerErrorEventArgs(CorProcess process, int hresult,
                                          int errorCode, ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
            m_hresult = hresult;
            m_errorCode = errorCode;
        }

        public int HResult
        {
            get
            {
                return m_hresult;
            }
        }

        public int ErrorCode
        {
            get
            {
                return m_errorCode;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnDebuggerError)
            {
                return "Debugger Error";
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with Assemblies.
     */
    public class CorAssemblyEventArgs : CorAppDomainBaseEventArgs
    {
        private ICorDebugAssembly m_assembly;
        public CorAssemblyEventArgs(ICorDebugAppDomain appDomain,
                                     ICorDebugAssembly assembly)
            : base(appDomain)
        {
            m_assembly = assembly;
        }

        public CorAssemblyEventArgs(ICorDebugAppDomain appDomain,
                                     ICorDebugAssembly assembly, ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            m_assembly = assembly;
        }

        /** The Assembly of interest. */
        public ICorDebugAssembly Assembly
        {
            get
            {
                return m_assembly;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
                case ManagedCallbackType.OnAssemblyLoad:
                    return "Assembly loaded: " + m_assembly.Name;
                case ManagedCallbackType.OnAssemblyUnload:
                    return "Assembly unloaded: " + m_assembly.Name;
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with logged messages.
     */
    public class CorLogMessageEventArgs : CorThreadEventArgs
    {
        int m_level;
        string m_logSwitchName;
        string m_message;

        public CorLogMessageEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                       int level, string logSwitchName, string message)
            : base(appDomain, thread)
        {
            m_level = level;
            m_logSwitchName = logSwitchName;
            m_message = message;
        }

        public CorLogMessageEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                       int level, string logSwitchName, string message,
                                       ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_level = level;
            m_logSwitchName = logSwitchName;
            m_message = message;
        }

        public int Level
        {
            get
            {
                return m_level;
            }
        }

        public string LogSwitchName
        {
            get
            {
                return m_logSwitchName;
            }
        }

        public string Message
        {
            get
            {
                return m_message;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnLogMessage)
            {
                return "Log message(" + m_logSwitchName + ")";
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with logged messages.
     */
    public class CorLogSwitchEventArgs : CorThreadEventArgs
    {
        int m_level;

        int m_reason;

        string m_logSwitchName;

        string m_parentName;

        public CorLogSwitchEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                      int level, int reason, string logSwitchName, string parentName)
            : base(appDomain, thread)
        {
            m_level = level;
            m_reason = reason;
            m_logSwitchName = logSwitchName;
            m_parentName = parentName;
        }

        public CorLogSwitchEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                      int level, int reason, string logSwitchName, string parentName,
                                      ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_level = level;
            m_reason = reason;
            m_logSwitchName = logSwitchName;
            m_parentName = parentName;
        }

        public int Level
        {
            get
            {
                return m_level;
            }
        }

        public int Reason
        {
            get
            {
                return m_reason;
            }
        }

        public string LogSwitchName
        {
            get
            {
                return m_logSwitchName;
            }
        }

        public string ParentName
        {
            get
            {
                return m_parentName;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnLogSwitch)
            {
                return "Log Switch" + "\n" +
                    "Level: " + m_level + "\n" +
                    "Log Switch Name: " + m_logSwitchName;
            }
            return base.ToString();
        }
    }


    /**
     * For events dealing with MDA messages.
     */
    public class CorMDAEventArgs : CorProcessEventArgs
    {
        // Thread may be null.
        public CorMDAEventArgs(CorMDA mda, ICorDebugThread thread, CorProcess proc)
            : base(proc)
        {
            m_mda = mda;
            Thread = thread;
            //m_proc = proc;
        }

        public CorMDAEventArgs(CorMDA mda, ICorDebugThread thread, CorProcess proc,
            ManagedCallbackType callbackType)
            : base(proc, callbackType)
        {
            m_mda = mda;
            Thread = thread;
            //m_proc = proc;
        }

        CorMDA m_mda;
        public CorMDA MDA { get { return m_mda; } }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnMDANotification)
            {
                return "MDANotification" + "\n" +
                    "Name=" + m_mda.Name + "\n" +
                    "XML=" + m_mda.XML;
            }
            return base.ToString();
        }

        //CorProcess m_proc;
        //CorProcess Process { get { return m_proc; } }
    }


    /**
     * For events dealing module symbol updates.
     */
    public class CorUpdateModuleSymbolsEventArgs : CorModuleEventArgs
    {
        IStream m_stream;

        [CLSCompliant(false)]
        public CorUpdateModuleSymbolsEventArgs(ICorDebugAppDomain appDomain,
                                                ICorDebugModule managedModule,
                                                IStream stream)
            : base(appDomain, managedModule)
        {
            m_stream = stream;
        }

        [CLSCompliant(false)]
        public CorUpdateModuleSymbolsEventArgs(ICorDebugAppDomain appDomain,
                                                ICorDebugModule managedModule,
                                                IStream stream,
                                                ManagedCallbackType callbackType)
            : base(appDomain, managedModule, callbackType)
        {
            m_stream = stream;
        }

        [CLSCompliant(false)]
        public IStream Stream
        {
            get
            {
                return m_stream;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnUpdateModuleSymbols)
            {
                return "Module Symbols Updated";
            }
            return base.ToString();
        }
    }

    public sealed class CorExceptionInCallbackEventArgs : CorEventArgs
    {
        public CorExceptionInCallbackEventArgs(ICorDebugController controller, Exception exceptionThrown)
            : base(controller)
        {
            m_exceptionThrown = exceptionThrown;
        }

        public CorExceptionInCallbackEventArgs(ICorDebugController controller, Exception exceptionThrown,
            ManagedCallbackType callbackType)
            : base(controller, callbackType)
        {
            m_exceptionThrown = exceptionThrown;
        }

        public Exception ExceptionThrown
        {
            get
            {
                return m_exceptionThrown;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnExceptionInCallback)
            {
                return "Callback Exception: " + m_exceptionThrown.Message;
            }
            return base.ToString();
        }

        private Exception m_exceptionThrown;
    }


    /**
     * Edit and Continue callbacks
     */
    public class CorEditAndContinueRemapEventArgs : CorThreadEventArgs
    {
        public CorEditAndContinueRemapEventArgs(ICorDebugAppDomain appDomain,
                                        ICorDebugThread thread,
                                        ICorDebugFunction managedFunction,
                                        int accurate)
            : base(appDomain, thread)
        {
            m_managedFunction = managedFunction;
            m_accurate = accurate;
        }

        public CorEditAndContinueRemapEventArgs(ICorDebugAppDomain appDomain,
                                        ICorDebugThread thread,
                                        ICorDebugFunction managedFunction,
                                        int accurate,
                                        ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_managedFunction = managedFunction;
            m_accurate = accurate;
        }

        public ICorDebugFunction Function
        {
            get
            {
                return m_managedFunction;
            }
        }

        public bool IsAccurate
        {
            get
            {
                return m_accurate != 0;
            }
        }

        private ICorDebugFunction m_managedFunction;
        private int m_accurate;
    }


    public class CorBreakpointSetErrorEventArgs : CorThreadEventArgs
    {
        public CorBreakpointSetErrorEventArgs(ICorDebugAppDomain appDomain,
                                        ICorDebugThread thread,
                                        ICorDebugBreakpoint breakpoint,
                                        int errorCode)
            : base(appDomain, thread)
        {
            m_breakpoint = breakpoint;
            m_errorCode = errorCode;
        }

        public CorBreakpointSetErrorEventArgs(ICorDebugAppDomain appDomain,
                                        ICorDebugThread thread,
                                        ICorDebugBreakpoint breakpoint,
                                        int errorCode,
                                        ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_breakpoint = breakpoint;
            m_errorCode = errorCode;
        }

        public ICorDebugBreakpoint Breakpoint
        {
            get
            {
                return m_breakpoint;
            }
        }

        public int ErrorCode
        {
            get
            {
                return m_errorCode;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnBreakpointSetError)
            {
                return "Error Setting Breakpoint";
            }
            return base.ToString();
        }

        private ICorDebugBreakpoint m_breakpoint;
        private int m_errorCode;
    }


    public sealed class CorFunctionRemapOpportunityEventArgs : CorThreadEventArgs
    {
        public CorFunctionRemapOpportunityEventArgs(ICorDebugAppDomain appDomain,
                                           ICorDebugThread thread,
                                           ICorDebugFunction oldFunction,
                                           ICorDebugFunction newFunction,
                                           int oldILoffset
                                           )
            : base(appDomain, thread)
        {
            m_oldFunction = oldFunction;
            m_newFunction = newFunction;
            m_oldILoffset = oldILoffset;
        }

        public CorFunctionRemapOpportunityEventArgs(ICorDebugAppDomain appDomain,
                                           ICorDebugThread thread,
                                           ICorDebugFunction oldFunction,
                                           ICorDebugFunction newFunction,
                                           int oldILoffset,
                                           ManagedCallbackType callbackType
                                           )
            : base(appDomain, thread, callbackType)
        {
            m_oldFunction = oldFunction;
            m_newFunction = newFunction;
            m_oldILoffset = oldILoffset;
        }

        public ICorDebugFunction OldFunction
        {
            get
            {
                return m_oldFunction;
            }
        }

        public ICorDebugFunction NewFunction
        {
            get
            {
                return m_newFunction;
            }
        }

        public int OldILOffset
        {
            get
            {
                return m_oldILoffset;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnFunctionRemapOpportunity)
            {
                return "Function Remap Opportunity";
            }
            return base.ToString();
        }

        private ICorDebugFunction m_oldFunction, m_newFunction;
        private int m_oldILoffset;
    }

    public sealed class CorFunctionRemapCompleteEventArgs : CorThreadEventArgs
    {
        public CorFunctionRemapCompleteEventArgs(ICorDebugAppDomain appDomain,
                                           ICorDebugThread thread,
                                           ICorDebugFunction managedFunction
                                           )
            : base(appDomain, thread)
        {
            m_managedFunction = managedFunction;
        }

        public CorFunctionRemapCompleteEventArgs(ICorDebugAppDomain appDomain,
                                           ICorDebugThread thread,
                                           ICorDebugFunction managedFunction,
                                           ManagedCallbackType callbackType
                                           )
            : base(appDomain, thread, callbackType)
        {
            m_managedFunction = managedFunction;
        }

        public ICorDebugFunction Function
        {
            get
            {
                return m_managedFunction;
            }
        }

        private ICorDebugFunction m_managedFunction;
    }


    public class CorExceptionUnwind2EventArgs : CorThreadEventArgs
    {

        [CLSCompliant(false)]
        public CorExceptionUnwind2EventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                            CorDebugExceptionUnwindCallbackType eventType,
                                            int flags)
            : base(appDomain, thread)
        {
            m_eventType = eventType;
            m_flags = flags;
        }

        [CLSCompliant(false)]
        public CorExceptionUnwind2EventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
                                            CorDebugExceptionUnwindCallbackType eventType,
                                            int flags,
                                            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_eventType = eventType;
            m_flags = flags;
        }

        [CLSCompliant(false)]
        public CorDebugExceptionUnwindCallbackType EventType
        {
            get
            {
                return m_eventType;
            }
        }

        public int Flags
        {
            get
            {
                return m_flags;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnExceptionUnwind2)
            {
                return "Exception unwind\n" +
                    "EventType: " + m_eventType;
            }
            return base.ToString();
        }

        CorDebugExceptionUnwindCallbackType m_eventType;
        int m_flags;
    }


    public class CorException2EventArgs : CorThreadEventArgs
    {

        [CLSCompliant(false)]
        public CorException2EventArgs(ICorDebugAppDomain appDomain,
                                      ICorDebugThread thread,
                                      ICorDebugFrame frame,
                                      int offset,
                                      CorDebugExceptionCallbackType eventType,
                                      int flags)
            : base(appDomain, thread)
        {
            m_frame = frame;
            m_offset = offset;
            m_eventType = eventType;
            m_flags = flags;
        }

        [CLSCompliant(false)]
        public CorException2EventArgs(ICorDebugAppDomain appDomain,
                                      ICorDebugThread thread,
                                      ICorDebugFrame frame,
                                      int offset,
                                      CorDebugExceptionCallbackType eventType,
                                      int flags,
                                      ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_frame = frame;
            m_offset = offset;
            m_eventType = eventType;
            m_flags = flags;
        }

        public ICorDebugFrame Frame
        {
            get
            {
                return m_frame;
            }
        }

        public int Offset
        {
            get
            {
                return m_offset;
            }
        }

        [CLSCompliant(false)]
        public CorDebugExceptionCallbackType EventType
        {
            get
            {
                return m_eventType;
            }
        }

        public int Flags
        {
            get
            {
                return m_flags;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnException2)
            {
                return "Exception Thrown";
            }
            return base.ToString();
        }

        ICorDebugFrame m_frame;
        int m_offset;
        CorDebugExceptionCallbackType m_eventType;
        int m_flags;
    }


    public enum ManagedCallbackType 
    {
        OnBreakpoint,
        OnStepComplete,
        OnBreak,
        OnException,
        OnEvalComplete,
        OnEvalException,
        OnCreateProcess,
        OnProcessExit,
        OnCreateThread,
        OnThreadExit,
        OnModuleLoad,
        OnModuleUnload,
        OnClassLoad,
        OnClassUnload,
        OnDebuggerError,
        OnLogMessage,
        OnLogSwitch,
        OnCreateAppDomain,
        OnAppDomainExit,
        OnAssemblyLoad,
        OnAssemblyUnload,
        OnControlCTrap,
        OnNameChange,
        OnUpdateModuleSymbols,
        OnFunctionRemapOpportunity,
        OnFunctionRemapComplete,
        OnBreakpointSetError,
        OnException2,
        OnExceptionUnwind2,
        OnMDANotification,
        OnExceptionInCallback,
    }

    // Helper class to convert from COM-classic callback interface into managed args.
    // Derived classes can overide the HandleEvent method to define the handling.
    abstract unsafe public class ManagedCallbackBase : ICorDebugManagedCallback, ICorDebugManagedCallback2
    {
        // Derived class overrides this methdos 
        protected abstract void HandleEvent(ManagedCallbackType eventId, CorEventArgs args);

        void ICorDebugManagedCallback.Breakpoint(CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                CorApi.ComInterop.ICorDebugThread thread,
                                CorApi.ComInterop.ICorDebugBreakpoint breakpoint)
        {
            HandleEvent(ManagedCallbackType.OnBreakpoint,
                               new CorBreakpointEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                           thread == null ? null : new ICorDebugThread(thread),
                                                           breakpoint == null ? null : new ICorDebugFunctionBreakpoint((CorApi.ComInterop.ICorDebugFunctionBreakpoint)breakpoint),
                                                           ManagedCallbackType.OnBreakpoint
                                                           ));
        }

        void ICorDebugManagedCallback.StepComplete(CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                   CorApi.ComInterop.ICorDebugThread thread,
                                   CorApi.ComInterop.ICorDebugStepper stepper,
                                   CorDebugStepReason stepReason)
        {
            HandleEvent(ManagedCallbackType.OnStepComplete,
                               new CorStepCompleteEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                            thread == null ? null : new ICorDebugThread(thread),
                                                            stepper,
                                                            stepReason,
                                                            ManagedCallbackType.OnStepComplete));
        }

        void ICorDebugManagedCallback.Break(
                           CorApi.ComInterop.ICorDebugAppDomain appDomain,
                           CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnBreak,
                               new CorThreadEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      thread == null ? null : new ICorDebugThread(thread),
                                                      ManagedCallbackType.OnBreak));
        }

        void ICorDebugManagedCallback.Exception(
                                                 CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                                 CorApi.ComInterop.ICorDebugThread thread,
                                                 int unhandled)
        {
            HandleEvent(ManagedCallbackType.OnException,
                               new CorExceptionEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                         thread == null ? null : new ICorDebugThread(thread),
                                                         !(unhandled == 0),
                                                         ManagedCallbackType.OnException));
        }
        /* pass false if ``unhandled'' is 0 -- mapping TRUE to true, etc. */

        void ICorDebugManagedCallback.EvalComplete(
                                  CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                  CorApi.ComInterop.ICorDebugThread thread,
                                  ICorDebugEval eval)
        {
            HandleEvent(ManagedCallbackType.OnEvalComplete,
                              new CorEvalEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                    thread == null ? null : new ICorDebugThread(thread),
                                                    eval == null ? null : new CorEval(eval),
                                                    ManagedCallbackType.OnEvalComplete));
        }

        void ICorDebugManagedCallback.EvalException(
                                   CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                   CorApi.ComInterop.ICorDebugThread thread,
                                   ICorDebugEval eval)
        {
            HandleEvent(ManagedCallbackType.OnEvalException,
                              new CorEvalEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                    thread == null ? null : new ICorDebugThread(thread),
                                                    eval == null ? null : new CorEval(eval),
                                                    ManagedCallbackType.OnEvalException));
        }

        void ICorDebugManagedCallback.CreateProcess(
                                   ICorDebugProcess process)
        {
            HandleEvent(ManagedCallbackType.OnCreateProcess,
                              new CorProcessEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                                                       ManagedCallbackType.OnCreateProcess));
        }

        void ICorDebugManagedCallback.ExitProcess(
                                 ICorDebugProcess process)
        {
            HandleEvent(ManagedCallbackType.OnProcessExit,
                               new CorProcessEventArgs(process == null ? null : CorProcess.GetCorProcess(process),
                                                       ManagedCallbackType.OnProcessExit) { Continue = false });
        }

        void ICorDebugManagedCallback.CreateThread(
                                  CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                  CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnCreateThread,
                              new CorThreadEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      thread == null ? null : new ICorDebugThread(thread),
                                                      ManagedCallbackType.OnCreateThread));
        }

        void ICorDebugManagedCallback.ExitThread(
                                CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnThreadExit,
                              new CorThreadEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      thread == null ? null : new ICorDebugThread(thread),
                                                      ManagedCallbackType.OnThreadExit));
        }

        void ICorDebugManagedCallback.LoadModule(
                                CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                CorApi.ComInterop.ICorDebugModule managedModule)
        {
            HandleEvent(ManagedCallbackType.OnModuleLoad,
                              new CorModuleEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      managedModule == null ? null : new ICorDebugModule(managedModule),
                                                      ManagedCallbackType.OnModuleLoad));
        }

        void ICorDebugManagedCallback.UnloadModule(
                                  CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                  CorApi.ComInterop.ICorDebugModule managedModule)
        {
            HandleEvent(ManagedCallbackType.OnModuleUnload,
                              new CorModuleEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      managedModule == null ? null : new ICorDebugModule(managedModule),
                                                      ManagedCallbackType.OnModuleUnload));
        }

        void ICorDebugManagedCallback.LoadClass(
                               CorApi.ComInterop.ICorDebugAppDomain appDomain,
                               CorApi.ComInterop.ICorDebugClass c)
        {
            HandleEvent(ManagedCallbackType.OnClassLoad,
                               new CorClassEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                     c == null ? null : new ICorDebugClass(c),
                                                     ManagedCallbackType.OnClassLoad));
        }

        void ICorDebugManagedCallback.UnloadClass(
                                 CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                 CorApi.ComInterop.ICorDebugClass c)
        {
            HandleEvent(ManagedCallbackType.OnClassUnload,
                              new CorClassEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                     c == null ? null : new ICorDebugClass(c),
                                                     ManagedCallbackType.OnClassUnload));
        }

        void ICorDebugManagedCallback.DebuggerError(
                                   ICorDebugProcess process,
                                   int errorHR,
                                   uint errorCode)
        {
            HandleEvent(ManagedCallbackType.OnDebuggerError,
                              new CorDebuggerErrorEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                                                             errorHR,
                                                             (int)errorCode,
                                                             ManagedCallbackType.OnDebuggerError));
        }

        void ICorDebugManagedCallback.LogMessage(
                                CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                CorApi.ComInterop.ICorDebugThread thread,
                                int level,
                                UInt16* pLogSwitchName,
                                UInt16* pMessage)
        {
            HandleEvent(ManagedCallbackType.OnLogMessage,
                               new CorLogMessageEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                          thread == null ? null : new ICorDebugThread(thread),
                                                          level,
                                                          pLogSwitchName == null ? null : new string((char*) pLogSwitchName),
                                                          pMessage == null ? null : new string ((char*) pMessage),
                                                          ManagedCallbackType.OnLogMessage));
        }

        void ICorDebugManagedCallback.LogSwitch(
                               CorApi.ComInterop.ICorDebugAppDomain appDomain,
                               CorApi.ComInterop.ICorDebugThread thread,
                               int level,
                               uint reason,
                               UInt16* pLogSwitchName,
                               UInt16* pParentName)
        {
            HandleEvent(ManagedCallbackType.OnLogSwitch,
                              new CorLogSwitchEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                         thread == null ? null : new ICorDebugThread(thread),
                                                         level, (int)reason,
                                                         pLogSwitchName == null ? null : new string ((char*) pLogSwitchName),
                                                         pParentName == null ? null : new string ((char*) pParentName),
                                                         ManagedCallbackType.OnLogSwitch));
        }

        void ICorDebugManagedCallback.CreateAppDomain(
                                     ICorDebugProcess process,
                                     CorApi.ComInterop.ICorDebugAppDomain appDomain)
        {
            HandleEvent(ManagedCallbackType.OnCreateAppDomain,
                              new CorAppDomainEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                                                         appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                         ManagedCallbackType.OnCreateAppDomain));
        }

        void ICorDebugManagedCallback.ExitAppDomain(
                                   ICorDebugProcess process,
                                   CorApi.ComInterop.ICorDebugAppDomain appDomain)
        {
            HandleEvent(ManagedCallbackType.OnAppDomainExit,
                              new CorAppDomainEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                                                         appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                         ManagedCallbackType.OnAppDomainExit));
        }

        void ICorDebugManagedCallback.LoadAssembly(
                                  CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                  CorApi.ComInterop.ICorDebugAssembly assembly)
        {
            HandleEvent(ManagedCallbackType.OnAssemblyLoad,
                              new CorAssemblyEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                        assembly == null ? null : new ICorDebugAssembly(assembly),
                                                        ManagedCallbackType.OnAssemblyLoad));
        }

        void ICorDebugManagedCallback.UnloadAssembly(
                                    CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                    CorApi.ComInterop.ICorDebugAssembly assembly)
        {
            HandleEvent(ManagedCallbackType.OnAssemblyUnload,
                              new CorAssemblyEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                        assembly == null ? null : new ICorDebugAssembly(assembly),
                                                        ManagedCallbackType.OnAssemblyUnload));
        }

        void ICorDebugManagedCallback.ControlCTrap(ICorDebugProcess process)
        {
            HandleEvent(ManagedCallbackType.OnControlCTrap,
                              new CorProcessEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                                                       ManagedCallbackType.OnControlCTrap));
        }

        void ICorDebugManagedCallback.NameChange(
                                CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnNameChange,
                              new CorThreadEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      thread == null ? null : new ICorDebugThread(thread),
                                                      ManagedCallbackType.OnNameChange));
        }

        
        void ICorDebugManagedCallback.UpdateModuleSymbols(
                                         CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                         CorApi.ComInterop.ICorDebugModule managedModule,
 IStream stream)
        {
            HandleEvent(ManagedCallbackType.OnUpdateModuleSymbols,
                              new CorUpdateModuleSymbolsEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                                  managedModule == null ? null : new ICorDebugModule(managedModule),
                                                                  stream,
                                                                  ManagedCallbackType.OnUpdateModuleSymbols));
        }

        void ICorDebugManagedCallback.EditAndContinueRemap(
                                         CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                         CorApi.ComInterop.ICorDebugThread thread,
                                         CorApi.ComInterop.ICorDebugFunction managedFunction,
                                         int isAccurate)
        {
            Debug.Assert(false); //OBSOLETE callback
        }


        void ICorDebugManagedCallback.BreakpointSetError(
                                       CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                       CorApi.ComInterop.ICorDebugThread thread,
                                       CorApi.ComInterop.ICorDebugBreakpoint breakpoint,
                                       UInt32 errorCode)
        {
            HandleEvent(ManagedCallbackType.OnBreakpointSetError,
                              new CorBreakpointSetErrorEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                        thread == null ? null : new ICorDebugThread(thread),
                                                        null, 
                                                        (int)errorCode,
                                                        ManagedCallbackType.OnBreakpointSetError));
        }

        void ICorDebugManagedCallback2.FunctionRemapOpportunity(CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                                                       CorApi.ComInterop.ICorDebugThread thread,
                                                                       CorApi.ComInterop.ICorDebugFunction oldFunction,
                                                                       CorApi.ComInterop.ICorDebugFunction newFunction,
                                                                       uint oldILoffset)
        {
            HandleEvent(ManagedCallbackType.OnFunctionRemapOpportunity,
                                      new CorFunctionRemapOpportunityEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                                               thread == null ? null : new ICorDebugThread(thread),
                                                                               oldFunction == null ? null : new ICorDebugFunction(oldFunction),
                                                                               newFunction == null ? null : new ICorDebugFunction(newFunction),
                                                                               (int)oldILoffset,
                                                                               ManagedCallbackType.OnFunctionRemapOpportunity));
        }

        void ICorDebugManagedCallback2.FunctionRemapComplete(CorApi.ComInterop.ICorDebugAppDomain appDomain,
                                                             CorApi.ComInterop.ICorDebugThread thread,
                                                             CorApi.ComInterop.ICorDebugFunction managedFunction)
        {
            HandleEvent(ManagedCallbackType.OnFunctionRemapComplete,
                               new CorFunctionRemapCompleteEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                                                      thread == null ? null : new ICorDebugThread(thread),
                                                      managedFunction == null ? null : new ICorDebugFunction(managedFunction),
                                                      ManagedCallbackType.OnFunctionRemapComplete));
        }

        void ICorDebugManagedCallback2.CreateConnection(ICorDebugProcess process, uint connectionId, ushort* pConnName)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        void ICorDebugManagedCallback2.ChangeConnection(ICorDebugProcess process, uint connectionId)
        {
            //  Not Implemented
            Debug.Assert(false);
        }

        void ICorDebugManagedCallback2.DestroyConnection(ICorDebugProcess process, uint connectionId)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        void ICorDebugManagedCallback2.Exception(CorApi.ComInterop.ICorDebugAppDomain ad, CorApi.ComInterop.ICorDebugThread thread,
                                                 CorApi.ComInterop.ICorDebugFrame frame, uint offset,
                                                 CorDebugExceptionCallbackType eventType, uint flags) 
        {
            HandleEvent(ManagedCallbackType.OnException2,
                                      new CorException2EventArgs(ad == null ? null : new ICorDebugAppDomain(ad),
                                                        thread == null ? null : new ICorDebugThread(thread),
                                                        frame == null ? null : new ICorDebugFrame(frame),
                                                        (int)offset,
                                                        eventType,
                                                        (int)flags,
                                                        ManagedCallbackType.OnException2));
        }

        void ICorDebugManagedCallback2.ExceptionUnwind(CorApi.ComInterop.ICorDebugAppDomain ad, CorApi.ComInterop.ICorDebugThread thread,
                                                       CorDebugExceptionUnwindCallbackType eventType, uint flags)
        {
            HandleEvent(ManagedCallbackType.OnExceptionUnwind2,
                                      new CorExceptionUnwind2EventArgs(ad == null ? null : new ICorDebugAppDomain(ad),
                                                        thread == null ? null : new ICorDebugThread(thread),
                                                        eventType,
                                                        (int)flags,
                                                        ManagedCallbackType.OnExceptionUnwind2));
        }

        // Get process from controller 
        static private CorProcess GetProcessFromController(CorApi.ComInterop.ICorDebugController pController)
        {
            CorProcess p;
            ICorDebugProcess p2 = pController as ICorDebugProcess;
            if (p2 != null)
            {
                p = CorProcess.GetCorProcess(p2);
            }
            else
            {
                CorApi.ComInterop.ICorDebugAppDomain a2 = (CorApi.ComInterop.ICorDebugAppDomain)pController;
                p = new ICorDebugAppDomain(a2).Process;
            }
            return p;
        }

        void ICorDebugManagedCallback2.MDANotification(CorApi.ComInterop.ICorDebugController pController,
                                                       CorApi.ComInterop.ICorDebugThread thread,
                                                       ICorDebugMDA pMDA)
        {
            CorMDA c = new CorMDA(pMDA);
            string szName = c.Name;
            CorDebugMDAFlags f = c.Flags;
            CorProcess p = GetProcessFromController(pController);


            HandleEvent(ManagedCallbackType.OnMDANotification,
                                      new CorMDAEventArgs(c,
                                                           thread == null ? null : new ICorDebugThread(thread),
                                                           p, ManagedCallbackType.OnMDANotification));
        }


    }

} /* namespace */
