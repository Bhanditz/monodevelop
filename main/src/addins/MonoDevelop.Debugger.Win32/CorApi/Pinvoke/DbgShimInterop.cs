using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using PinvokeKit;

namespace CorApi.Pinvoke
{
    [CLSCompliant(false)]
    public unsafe class DbgShimInterop
    {
        public DbgShimInterop (string dbgShimPath)
        {
            var dll = NativeDllsLoader.LoadDll(dbgShimPath);
            CreateProcessForLaunch = dll.ImportMethod<CreateProcessForLaunchDelegate> ("CreateProcessForLaunch");
            RegisterForRuntimeStartup = dll.ImportMethod<RegisterForRuntimeStartupDelegate>("RegisterForRuntimeStartup");
            ResumeProcess = dll.ImportMethod<ResumeProcessDelegate>("ResumeProcess");
            CloseResumeHandle = dll.ImportMethod<CloseResumeHandleDelegate>("CloseResumeHandle");
        }

        /// <summary>
        /// HRESULT CreateProcessForLaunch(
        /// __in LPWSTR lpCommandLine,
        /// __in BOOL bSuspendProcess,
        /// __in LPVOID lpEnvironment,
        /// __in LPCWSTR lpCurrentDirectory,
        /// __out PDWORD pProcessId,
        /// __out HANDLE *pResumeHandle)
        /// </summary>
        /// <param name="lpCommandLine">lpCommandLine</param>
        /// <param name="bSuspendProcess">bSuspendProcess</param>
        /// <param name="lpEnvironment">lpEnvironment</param>
        /// <param name="lpCurrentDirectory">lpCurrentDirectory</param>
        /// <param name="pProcessId">pProcessId</param>
        /// <param name="pResumeHandle">pResumeHandle</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate Int32 CreateProcessForLaunchDelegate(
            [In, MarshalAs(UnmanagedType.LPWStr)]string lpCommandLine,
            [In]Int32 bSuspendProcess,
            [In]IntPtr lpEnvironment,
            [In, MarshalAs(UnmanagedType.LPWStr)]string lpCurrentDirectory,
            [Out]UInt32* pProcessId,
            [Out]void** pResumeHandle);
        public readonly CreateProcessForLaunchDelegate CreateProcessForLaunch;

        /// <summary>
        /// typedef VOID (*PSTARTUP_CALLBACK)(IUnknown *pCordb, PVOID parameter, HRESULT hr);
        /// </summary>
        /// <param name="pCordb">pCordb</param>
        /// <param name="parameter">parameter</param>
        /// <param name="hr">hr</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [SuppressMessage ("ReSharper", "InconsistentNaming")]
        public delegate void PSTARTUP_CALLBACK([In]void* pCordb, [In]void* parameter, [In]Int32 hr);

        /// <summary>
        /// HRESULT
        /// RegisterForRuntimeStartup(
        ///   __in DWORD dwProcessId,
        ///   __in PSTARTUP_CALLBACK pfnCallback,
        ///   __in PVOID parameter,
        ///   __out PVOID *ppUnregisterToken)
        /// </summary>
        /// <param name="dwProcessId">dwProcessId</param>
        /// <param name="callback">callback</param>
        /// <param name="parameter">parameter</param>
        /// <param name="ppUnregisterToken">ppUnregisterToken</param>

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate Int32 RegisterForRuntimeStartupDelegate(
            [In]UInt32 dwProcessId,
            [In]IntPtr callback,
            [In]void* parameter,
            [In]void** ppUnregisterToken);
        public readonly RegisterForRuntimeStartupDelegate RegisterForRuntimeStartup;

        /// <summary>
        /// HRESULT
        /// ResumeProcess(
        ///   __in HANDLE hResumeHandle)
        /// </summary>
        /// <param name="hResumeHandle">hResumeHandle</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate Int32 ResumeProcessDelegate([In]void* hResumeHandle);
        public readonly ResumeProcessDelegate ResumeProcess;

        /// <summary>
        /// HRESULT
        /// CloseResumeHandle(
        ///   __in HANDLE hResumeHandle)
        /// </summary>
        /// <param name="hResumeHandle">hResumeHandle</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate Int32 CloseResumeHandleDelegate([In]void* hResumeHandle);
        public readonly CloseResumeHandleDelegate CloseResumeHandle;
    }
}