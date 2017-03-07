using System;

using CorApi.Pinvoke;

using Mono.Debugging.Client;

namespace Mono.Debugging.Win32
{
	public class CoreClrDebuggerSession : CorDebuggerSession
	{
		private static readonly TimeSpan RuntimeLoadTimeout = TimeSpan.FromSeconds(1);

		private readonly DbgShimInterop dbgShimInterop;

		public CoreClrDebuggerSession(char[] badPathChars, string dbgShimPath)
			: base(badPathChars)
		{
			dbgShimInterop = new DbgShimInterop(dbgShimPath);
		}

		private void AttachToProcessImpl(uint procId)
		{
			attaching = true;
			MtaThread.Run(delegate
			{
				SetICorDebug(CoreClrShimUtil.CreateICorDebugForProcess(dbgShimInterop, procId, RuntimeLoadTimeout));
				CorDebugAttachPid(procId);
			});
			OnStarted();
		}

		protected override void OnAttachToProcess(long procId)
		{
			AttachToProcessImpl((uint)procId);
		}

		protected override void OnAttachToProcess(ProcessInfo processInfo)
		{
			AttachToProcessImpl((uint)processInfo.Id);
		}

		protected override void OnRun(DebuggerStartInfo startInfo)
		{
			MtaThread.Run(() =>
			{
				CoreClrShimUtil.CorDebugAndPid process = CoreClrShimUtil.CreateICorDebugForCommand(dbgShimInterop, PrepareCommandLine(startInfo), PrepareWorkingDirectory(startInfo), PrepareEnvironment(startInfo), RuntimeLoadTimeout);
				SetICorDebug(process.ICorDebug);
				CorDebugAttachPid(process.Pid);
			});
			OnStarted();
		}
	}
}