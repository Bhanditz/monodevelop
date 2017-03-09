using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

using CorApi.Pinvoke;

using JetBrains.Annotations;

using Microsoft.Win32.SafeHandles;

namespace CorApi.ComInterop
{
    /// <summary>
    /// Knows how to create <see cref="ICorDebug" /> for various environments.
    /// </summary>
    public static class CorDebugFactory
    {
        public static ICorDebug CreateFromDebuggeeVersion([NotNull] string verDebuggee)
        {
            if(verDebuggee == null)
                throw new ArgumentNullException(nameof(verDebuggee));
            if(!verDebuggee.StartsWith("v"))
                throw new ArgumentOutOfRangeException(nameof(verDebuggee), verDebuggee, "Expecting the version in the “v”-notation, e.g. v2.0.50727.");
            if(verDebuggee.StartsWith("v1"))
                throw new ArgumentException("Can't debug a version 1 CLR process (\"" + verDebuggee + "\").  Run application in a version 2 CLR, or use a version 1 debugger instead.");

            int verDebugger = verDebuggee.StartsWith("v4") ? 4 : 3;

            return MscoreeHelpers.CreateDebuggingInterfaceFromVersion(verDebugger, verDebuggee);
        }

        [NotNull]
        public static ICorDebug CreateFromDebuggerClsid(Guid clsid)
        {
            object rawDebuggingAPI;
            Guid iidiCorDebug = typeof(ICorDebug).GUID;
            Ole32Dll.CoCreateInstance(ref clsid, null, /*pUnkOuter*/ CLSCTX.CLSCTX_INPROC_SERVER, ref iidiCorDebug, out rawDebuggingAPI);
            return Com.QueryInteface<ICorDebug>(rawDebuggingAPI);
        }

        [NotNull]
        public static unsafe string GetDebuggerVersionFromFile([NotNull] string pathToExe)
        {
            Debug.Assert(!string.IsNullOrEmpty(pathToExe));
            if(string.IsNullOrEmpty(pathToExe))
                throw new ArgumentException("Value cannot be null or empty.", "pathToExe");
            return LpwstrHelper.GetString((x, y, z) =>
            {
                fixed(char* pchPathCopy = pathToExe.ToCharArray() /*declared as a mutable string, so shan't pass the original*/)
                    return MscoreeDll.GetRequestedRuntimeVersion((ushort*)pchPathCopy, y, x, z);
            }, $"Could not get the requested runtime version from an executable file “{pathToExe}”.");
        }

        [NotNull]
        public static unsafe string GetDebuggerVersionFromPid(uint pid)
        {
            using(var hProcess = new ProcessSafeHandle(Kernel32Dll.OpenProcess((int)(PROCESS_ACCESS.PROCESS_VM_READ | PROCESS_ACCESS.PROCESS_QUERY_INFORMATION | PROCESS_ACCESS.PROCESS_DUP_HANDLE | PROCESS_ACCESS.SYNCHRONIZE), 0, /*inherit handle*/ pid), true))
            {
                if(hProcess.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return LpwstrHelper.GetString((x, y, z) => MscoreeDll.GetVersionFromProcess(hProcess.Value, y, x, z), $"Could not get the debugger version from a running process with PID {pid:N0}.");
            }
        }

        [NotNull]
        public static string GetDefaultDebuggerVersion()
        {
            return MscoreeHelpers.GetCORVersion();
        }

        [ItemNotNull]
        [NotNull]
        public static unsafe List<string> GetProcessLoadedRuntimes(uint pid)
        {
            using(var ph = new ProcessSafeHandle(Kernel32Dll.OpenProcess((int)(PROCESS_ACCESS.PROCESS_VM_READ | PROCESS_ACCESS.PROCESS_QUERY_INFORMATION | PROCESS_ACCESS.PROCESS_DUP_HANDLE | PROCESS_ACCESS.SYNCHRONIZE), 0, // inherit handle
                pid), true))
            {
                if(ph.IsInvalid)
                    return new List<string>();
                ICLRMetaHost host;
                void* pClrMetaHost = null;
                using(Com.UsingReference(&pClrMetaHost))
                {
                    Guid CLSID_CLRMetaHost = typeof(MscoreeDll.CLSID_CLRMetaHost).GUID;
                    Guid IID_ICLRMetaHost = typeof(ICLRMetaHost).GUID;
                    MscoreeDll.CLRCreateInstance(&CLSID_CLRMetaHost, &IID_ICLRMetaHost, &pClrMetaHost).AssertSucceeded("Failed to create the CLR Meta Host instance.");
                    host = Com.QueryInteface<ICLRMetaHost>(pClrMetaHost);
                }
                var result = new List<string>();
                IEnumUnknown enumRuntimes;
                void* pUnkRuntimes = null;
                using(Com.UsingReference(&pUnkRuntimes))
                {
                    host.EnumerateLoadedRuntimes(ph.Value, &pUnkRuntimes).AssertSucceeded($"Could not initiate enumeration of the loaded runtimes for the process {pid:X}.");
                    enumRuntimes = Com.QueryInteface<IEnumUnknown>(pUnkRuntimes);
                }
                var arRuntimes = new void*[0x10];
                try
                {
                    uint celtActual = 0;
                    fixed(void** ppRuntimes = arRuntimes)
                        enumRuntimes.Next(((uint)arRuntimes.Length), ppRuntimes, &celtActual).AssertSucceeded($"Could not get the list of the loaded runtimes from the runtimes enumeration for the process {pid:X}.");

                    var strbuf = new ushort[0x100];
                    fixed(ushort* pszBuf = strbuf)
                    {
                        foreach(void* pRuntime in arRuntimes)
                        {
                            var runtime = Com.QueryInteface<ICLRRuntimeInfo>(pRuntime);
                            uint cchStrBuf = (uint)strbuf.Length - 1;
                            runtime.GetVersionString(pszBuf, &cchStrBuf).AssertSucceeded($"Could not get the version string for one of the loaded runtimes from the runtimes enumeration for the process {pid:X}.");
                            strbuf[strbuf.Length - 1] = 0;
                            result.Add(new string((char*)pszBuf));
                        }
                    }
                }
                finally
                {
                    foreach(void* pRuntime in arRuntimes)
                        Com.UnknownRelease(pRuntime);
                }
                return result;
            }
        }

        private unsafe class ProcessSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private ProcessSafeHandle()
                : base(true)
            {
            }

            public ProcessSafeHandle(void* handle, bool ownsHandle)
                : base(ownsHandle)
            {
                SetHandle((IntPtr)handle);
            }

            public void* Value => (void*)DangerousGetHandle();

            protected override bool ReleaseHandle()
            {
                return Kernel32Dll.CloseHandle(Value) != 0;
            }
        }
    }
}