using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Samples.Debugging.CorDebug.NativeApi;
using Microsoft.Samples.Debugging.CorPublish.Metahost;
// ReSharper disable InconsistentNaming

namespace Microsoft.Samples.Debugging.CorDebug
{
  internal enum ProcessAccessOptions : int
  {
    PROCESS_TERMINATE         = 0x0001,
    PROCESS_CREATE_THREAD     = 0x0002,
    PROCESS_SET_SESSIONID     = 0x0004,
    PROCESS_VM_OPERATION      = 0x0008,
    PROCESS_VM_READ           = 0x0010,
    PROCESS_VM_WRITE          = 0x0020,
    PROCESS_DUP_HANDLE        = 0x0040,
    PROCESS_CREATE_PROCESS    = 0x0080,
    PROCESS_SET_QUOTA         = 0x0100,
    PROCESS_SET_INFORMATION   = 0x0200,
    PROCESS_QUERY_INFORMATION = 0x0400,
    PROCESS_SUSPEND_RESUME    = 0x0800,
    SYNCHRONIZE               = 0x100000,
  }

  internal static class Kernel32
  {
    private const string Kernel32LibraryName = "kernel32";

    [
      System.Runtime.ConstrainedExecution.ReliabilityContract(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.Success),
      DllImport(Kernel32LibraryName)
    ]
    public static extern bool CloseHandle(IntPtr handle);


    [
      DllImport(Kernel32LibraryName, PreserveSig=true)
    ]
    public static extern ProcessSafeHandle OpenProcess(Int32 dwDesiredAccess, bool bInheritHandle, Int32 dwProcessId);

  }

  internal static class MsCorEE
  {
    private const string MsCorEELibraryName = "mscoree";

    [
      DllImport(MsCorEELibraryName, CharSet=CharSet.Unicode, PreserveSig=false)
    ]
    public static extern ICorDebug CreateDebuggingInterfaceFromVersion(int iDebuggerVersion
      ,string szDebuggeeVersion);

    [
      DllImport(MsCorEELibraryName, CharSet=CharSet.Unicode)
    ]
    public static extern int GetCORVersion([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder  szName
      ,Int32 cchBuffer
      ,out Int32 dwLength);

    [
      DllImport(MsCorEELibraryName, CharSet=CharSet.Unicode, PreserveSig=false)
    ]
    public static extern void GetVersionFromProcess(ProcessSafeHandle hProcess, StringBuilder versionString,
      Int32 bufferSize, out Int32 dwLength);

    [DllImport(MsCorEELibraryName, CharSet = CharSet.Auto, SetLastError = true, PreserveSig = false)]
    public static extern void CLRCreateInstance(
        ref Guid clsid,
        ref Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out IClrMetaHost metahostInterface);

    public static Guid CLSID_CLRMetaHost = new Guid("9280188D-0E8E-4867-B30C-7FA83884E8DE");
    public static Guid IID_ICLRMetaHost = new Guid("D332DB9E-B9B3-4125-8207-A14884F53216");
    public static Guid IIDICorDebug = new Guid("3d6f5f61-7538-11d3-8d5b-00104b35e7ef");


    [
      DllImport(MsCorEELibraryName, CharSet=CharSet.Unicode, PreserveSig=false)
    ]
    public static extern void GetRequestedRuntimeVersion(string pExe, StringBuilder pVersion,
      Int32 cchBuffer, out Int32 dwLength);
  }

  internal static class DbgShim
  {
    private const string DbgShimLibraryName = "dbgshim";


    [DllImport(DbgShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern unsafe Int32 EnumerateCLRs(UInt32 debuggeePID, void*** ppHandleArrayOut,
      char*** ppStringArrayOut, UInt32* pdwArrayLengthOut);

    /// <summary>Opens an existing local process object.</summary>
    /// <param name="dwDesiredAccess">The access to the process object.</param>
    /// <param name="bInheritHandle">If this value is TRUE, processes created by this process will inherit the handle.</param>
    /// <param name="dwProcessId">The identifier of the local process to be opened. If the specified process is the System Process (0x00000000), the function fails and the last error code is ERROR_INVALID_PARAMETER. If the specified process is the Idle process or one of the CSRSS processes, this function fails and the last error code is ERROR_ACCESS_DENIED because their access restrictions prevent user-level code from opening them.</param>
    /// <returns>If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true,
       ExactSpelling = true)]
    public static extern unsafe void* OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

    /// <summary>
    /// CreateProcessForLaunch - a stripped down version of the Windows CreateProcess
    /// that can be supported cross-platform.
    /// </summary>
    /// <param name="lpCommandLine"></param>
    /// <param name="bSuspendProcess"></param>
    /// <param name="lpEnvironment"></param>
    /// <param name="lpCurrentDirectory"></param>
    /// <param name="pProcessId"></param>
    /// <param name="pResumeHandle"></param>
    /// <returns></returns>
    [DllImport(DbgShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern unsafe Int32 CreateProcessForLaunch(
      string lpCommandLine,
      bool bSuspendProcess,
      void* lpEnvironment,
      string lpCurrentDirectory,
      UInt32* pProcessId,
      void** pResumeHandle);

    /// <summary>
    /// ResumeProcess - to be used with the CreateProcessForLaunch resume handle
    /// </summary>
    /// <param name="hResumeHandle"></param>
    /// <returns></returns>
    [DllImport(DbgShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern unsafe Int32 ResumeProcess(
      void* hResumeHandle);

    /// <summary>
    /// CloseResumeHandle - to be used with the CreateProcessForLaunch resume handle
    /// </summary>
    /// <param name="hResumeHandle"></param>
    /// <returns></returns>
    [DllImport(DbgShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern unsafe Int32 CloseResumeHandle(
      void* hResumeHandle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void RuntimeStartupCallback(void* pCordb, void* parameter, Int32 hr);

    /// <summary>
    ///  RegisterForRuntimeStartup -- executes the callback when the coreclr runtime
    ///      starts in the specified process. The callback is passed the proper ICorDebug
    ///      instance for the version of the runtime or an error if something fails. This
    ///      API works for launch and attach (and even the attach scenario if the runtime
    ///      hasn't been loaded yet) equally on both xplat and Windows. The callback is
    ///      always called on a separate thread. This API returns immediately.
    ///
    ///      The callback is invoked when the coreclr runtime module is loaded during early
    ///      initialization. The runtime is blocked during initialization until the callback
    ///      returns.
    ///
    ///      If the runtime is already loaded in the process (as in the normal attach case),
    ///      the callback is executed and the runtime is not blocked.
    ///
    ///      The callback is always invoked on a separate thread and this API returns immediately.
    ///
    ///      Only the first coreclr module instance found in the target process is currently
    ///      supported.
    ///
    /// </summary>
    /// <param name="dwProcessId">process id of the target process</param>
    /// <param name="callback">invoked when coreclr runtime starts</param>
    /// <param name="parameter">data to pass to callback</param>
    /// <param name="ppUnregisterToken">pointer to put the UnregisterForRuntimeStartup token</param>
    /// <returns></returns>
    [DllImport(DbgShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = false)]
    public static extern unsafe Int32 RegisterForRuntimeStartup(
      UInt32 dwProcessId,
      IntPtr callback,
      void* parameter,
      void** ppUnregisterToken);

  }
}