using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;

using CorApi.ComInterop;

using JetBrains.Annotations;

namespace CorApi.Pinvoke
{
    public static unsafe class CoreClrShimUtil
    {
        [CLSCompliant(false)]
        public static CorDebugAndPid CreateICorDebugForCommand(DbgShimInterop dbgShimInterop, string command, string workingDir, IDictionary<string, string> env, TimeSpan runtimeLoadTimeout)
        {
            string sEnv = Kernel32Dll.Helpers.GetEnvString(env);
            void* hResume;
            uint processId;

            fixed(char* pchCommand = command)
            fixed(char* pchworkingDir = workingDir)
            fixed(char* pchEnv = sEnv ?? "")
                dbgShimInterop.CreateProcessForLaunch((ushort*)pchCommand, 1, (sEnv != null ? pchEnv : null), (ushort*)pchworkingDir, &processId, &hResume).AssertSucceeded("Failed call CreateProcessForLaunch.");

            ComInterop.HResults hrCloseHandle;
            ICorDebug cordbg;
            try
            {
                cordbg = CreateICorDebugImpl(dbgShimInterop, processId, runtimeLoadTimeout, hResume);
            }
            finally
            {
                hrCloseHandle = hResume != null ? ((ComInterop.HResults)dbgShimInterop.CloseResumeHandle(hResume)) : ComInterop.HResults.S_OK; // Don't want to throw on HRESULT here because if another exception is in progress we'd suppress it with our less important one
            }
            hrCloseHandle.Assert("dbgShimInterop.CloseResumeHandle");
            return new CorDebugAndPid() {ICorDebug = cordbg, Pid = processId};
        }

        [CLSCompliant(false)]
        public static ICorDebug CreateICorDebugForProcess(DbgShimInterop dbgShimInterop, uint processId, TimeSpan runtimeLoadTimeout)
        {
            return CreateICorDebugImpl(dbgShimInterop, processId, runtimeLoadTimeout, null);
        }

        private static ICorDebug CreateICorDebugImpl(DbgShimInterop dbgShimInterop, uint processId, TimeSpan runtimeLoadTimeout, void* resumeHandle)
        {
            var waiter = new ManualResetEvent(false);
            ICorDebug corDebug = null;
            Exception callbackException = null;
            void* token;
            DbgShimInterop.PSTARTUP_CALLBACK callback = (pCordb, parameter, hrLoad) => CreateICorDebugImpl_Callback /*extracted func to catch natives*/(hrLoad, pCordb, waiter, ref corDebug, ref callbackException);
            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);

            dbgShimInterop.RegisterForRuntimeStartup(processId, callbackPtr, null, &token).AssertSucceeded("RegisterForRuntimeStartup.");

            ComInterop.HResults hrUnregister;
            try
            {
                if(resumeHandle != null)
                    dbgShimInterop.ResumeProcess(resumeHandle).AssertSucceeded("RegisterForRuntimeStartup has given us a resume handle, but we failed to use it with the ResumeProcess call.");

                if(!waiter.WaitOne(runtimeLoadTimeout))
                    throw new TimeoutException(string.Format(".NET core load awaiting timed out for {0}", runtimeLoadTimeout));
            }
            finally
            {
                // Release the callback
                hrUnregister = (ComInterop.HResults)dbgShimInterop.UnregisterForRuntimeStartup(token);
                // NOTE: do not want to check the HRESULT here because if an exception is in progress we do not want to overwrite it with another exception which is less important
                // This keeps valid the callbackPtr pointer up until the native side keeps it, ie until we call unregister, which might happen at any moment if we abandon waiting by timeout
                // If the callback is in progress, this would wait for the callback to complete, note in case we're aborting by timeout (however, we do not expect the callback to get stuck)
                GC.KeepAlive(callback);
            }
            if(Volatile.Read(ref callbackException) != null)
                throw Volatile.Read(ref callbackException);
            hrUnregister.Assert("dbgShimInterop.UnregisterForRuntimeStartup");
            return Volatile.Read(ref corDebug);
        }

        [HandleProcessCorruptedStateExceptions]
        private static void CreateICorDebugImpl_Callback(int hr, void* pCordb, [NotNull] ManualResetEvent waiter, [CanBeNull] ref ICorDebug corDebug, [CanBeNull] ref Exception callbackException)
        {
            try
            {
                hr.AssertSucceeded("RegisterForRuntimeStartup async result.");
                Volatile.Write(ref corDebug, Com.QueryInteface<ICorDebug>(pCordb));
            }
            catch(Exception e)
            {
                Volatile.Write(ref callbackException, e);
            }
            finally
            {
                waiter.Set();
            }
        }

        public struct CorDebugAndPid
        {
            public ICorDebug ICorDebug;

            public uint Pid;
        }
    }
}