using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using CorApi.ComInterop;
using HResults = PinvokeKit.HResults;

namespace CorApi.Pinvoke
{
    public static class CoreClrShimUtil
    {
        [CLSCompliant (false)]
        public static ICorDebug CreateICorDebugForCommand (DbgShimInterop dbgShimInterop, string command, string workingDir,
            IDictionary<string, string> env, TimeSpan runtimeLoadTimeout, out uint procId)
        {
            unsafe {
                 {
                    var sEnv = GetEnvString (env);
                    void* resumeHandle;
                    uint processId;
                    fixed(char* pchCommand = command)
                    fixed(char* pchworkingDir = workingDir)
                    fixed(char* pchEnv = sEnv??"")
                        dbgShimInterop.CreateProcessForLaunch((ushort*)pchCommand, 1, (sEnv != null ? pchEnv : null), (ushort*)pchworkingDir, &processId, &resumeHandle).AssertSucceeded("Failed call CreateProcessForLaunch.");
                    procId = processId;
                    return CreateICorDebugImpl (dbgShimInterop, processId, runtimeLoadTimeout, resumeHandle);
                } 
            }
        }

        [CLSCompliant(false)]
        public static ICorDebug CreateICorDebugForProcess (DbgShimInterop dbgShimInterop, int processId, TimeSpan runtimeLoadTimeout)
        {
            unsafe {
                return CreateICorDebugImpl (dbgShimInterop, (uint) processId, runtimeLoadTimeout, null);
            }
        }

        private static unsafe ICorDebug CreateICorDebugImpl (DbgShimInterop dbgShimInterop, uint processId, TimeSpan runtimeLoadTimeout, void* resumeHandle)
        {
            var waiter = new ManualResetEvent (false);
            ICorDebug corDebug = null;
            Exception callbackException = null;
            void* token;
            DbgShimInterop.PSTARTUP_CALLBACK callback = delegate (void* pCordb, void* parameter, int hr) {
                try {
                    if (hr < 0) {
                        Marshal.ThrowExceptionForHR (hr);
                    }
                    var unknown = Marshal.GetObjectForIUnknown ((IntPtr) pCordb);
                    corDebug = (ICorDebug) unknown;
                } catch (Exception e) {
                    callbackException = e;
                }
                waiter.Set ();
            };
            var callbackPtr = Marshal.GetFunctionPointerForDelegate (callback);

            var hret =  (HResults)dbgShimInterop.RegisterForRuntimeStartup (processId, callbackPtr, null, &token);

            if (hret != HResults.S_OK)
                throw new COMException(string.Format ("Failed call RegisterForRuntimeStartup: {0}", hret), (int)hret);

            if (resumeHandle != null)
                dbgShimInterop.ResumeProcess (resumeHandle);

            if (!waiter.WaitOne (runtimeLoadTimeout)) {
                throw new TimeoutException (string.Format (".NET core load awaiting timed out for {0}", runtimeLoadTimeout));
            }
            GC.KeepAlive (callback);
            if (callbackException != null)
                throw callbackException;
            return corDebug;
        }

        internal static string GetEnvString (IDictionary<string, string> environment)
        {
            if (environment != null) {
                string senv = null;
                foreach (KeyValuePair<string, string> var in environment) {
                    senv += var.Key + "=" + var.Value + "\0";
                }
                senv += "\0";
                return senv;
            }
            return null;
        }
    }
}