using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /// <summary>
    /// ICorDebug represents an event processing loop for a debugger process.
    ///
    /// The debugger must wait for the ExitProcess callback from all
    /// processes being debugged before releasing this interface.
    ///
    /// The ICorDebug object is the initial object to control all further managed debugging.
    /// In v1.0 + v1.1, this object was a CoClass created from COM.
    /// In v2.0, this object is no longer a CoClass and must be created from the function:
    ///     CreateDebuggingInterfaceFromVersion(
    ///         int iDebuggerVersion, // &lt;--- CorDebugVersion_2_0 if Debugger is V2.0
    ///         LPCWSTR szDebuggeeVersion, // &lt;--- version string of debuggee. Eg, "v1.1.4322"
    ///         IUnknown ** ppCordb
    ///     )
    /// declared in mscoree.idl.
    /// This new creation function is more version-aware. It allows clients to get a
    /// specific implementation (as specified by szDebuggeeVersion) of ICorDebug, which
    /// also emulates a specific version of the debugging API (as specified by iDebuggerVersion).
    /// </summary>
    /// <example><code>[
    ///     object,
    ///     local,
    ///     uuid(3d6f5f61-7538-11d3-8d5b-00104b35e7ef),
    ///     pointer_default(unique)
    /// ]</code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("3D6F5F61-7538-11D3-8D5B-00104B35E7EF")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebug
    {
        /// <summary>
        /// The debugger calls this method at creation time to initialize the debugging
        /// services, and  must be called at creation time before any other method on ICorDebug is called.
        /// </summary>
        /// <example><code>HRESULT Initialize();</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Initialize ();

        /// <summary>
        /// Terminate must be called when the ICorDebug is no longer needed.
        ///
        /// NOTE: Terminate should not be called until an ExitProcess callback has
        /// been received for all processes being debugged.
        /// </summary>
        /// <example><code>HRESULT Terminate();</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Terminate ();

        /// <summary>
        /// SetManagedHandler should be called at creation time to specify the
        /// event handler object for managed events.
        ///
        /// Returns:
        /// S_OK on success.
        /// E_NOINTERFACE - if pCallback does not implement sufficient interfaces
        ///     to receive debug events for the version of the API it requested.
        ///     Eg, if debugging a V2.0 app, pCallback must implement ICorDebugManagedCallback2.
        /// </summary>
        /// <param name="pCallback"></param>
        /// <example><code>HRESULT SetManagedHandler([in] ICorDebugManagedCallback *pCallback);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetManagedHandler ([MarshalAs (UnmanagedType.Interface), In] ICorDebugManagedCallback pCallback);

        /// <summary>
        /// SetUnmanagedHandler should be called at creation time to specify the
        /// event handler object for unmanaged events.
        ///
        /// This should be set after Initialize and before any calls to CreateProcess or DebugActiveProcess.
        ///
        /// However, for legacy purposes, it is not absolutely required to set this until
        /// before the first native debug event is fired. Specifically, if CreateProcess has the
        /// CREATE_SUSPENDED flag, native debug events will not be dispatched until the main thread
        /// is resumed.
        /// DebugActiveProcess will dispatch native debug events immediately, and so the unmanaged callback
        /// must be set before DebugActiveProcess is called.
        ///
        /// Returns:
        ///    S_OK if callback pointer is successfully updated.
        ///    failure on any failure.
        /// </summary>
        /// <param name="pCallback"></param>
        /// <example><code>HRESULT SetUnmanagedHandler([in] ICorDebugUnmanagedCallback *pCallback);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetUnmanagedHandler ([MarshalAs (UnmanagedType.Interface), In] ICorDebugUnmanagedCallback pCallback);

        /// <summary>
        /// CreateProcess launches a process under the control of the debugger
        /// All parameters are the same as the win32 CreateProcess call.
        ///
        /// To enable unmanaged (mixed-mode) debugging, pass
        /// DEBUG_PROCESS | DEBUG_ONLY_THIS_PROCESS to dwCreationFlags. DEBUG_PROCESS
        /// alone is not supported. If only managed debugging is desired, do not set
        /// these flags.
        ///
        /// The debugger and debuggee share a single console, then it's possible for
        /// the debuggee to hold "console locks" and then get stopped at a debug event.
        /// The debugger will then block trying to use the console. This is only an issue
        /// when interop debugging and if debugger + debuggee share the console.
        /// It is recommended to use the CREATE_NEW_CONSOLE flag to avoid this problem.
        /// </summary>
        /// <param name="lpApplicationName"></param>
        /// <param name="lpCommandLine"></param>
        /// <param name="lpProcessAttributes"></param>
        /// <param name="lpThreadAttributes"></param>
        /// <param name="bInheritHandles"></param>
        /// <param name="dwCreationFlags"></param>
        /// <param name="lpEnvironment"></param>
        /// <param name="lpCurrentDirectory"></param>
        /// <param name="lpStartupInfo"></param>
        /// <param name="lpProcessInformation"></param>
        /// <param name="debuggingFlags"></param>
        /// <param name="ppProcess"></param>
        /// <example><code>HRESULT CreateProcess([in] LPCWSTR lpApplicationName,
        ///                                      [in] LPWSTR lpCommandLine,
        ///                                      [in] LPSECURITY_ATTRIBUTES lpProcessAttributes,
        ///                                      [in] LPSECURITY_ATTRIBUTES lpThreadAttributes,
        ///                                      [in] BOOL bInheritHandles,
        ///                                      [in] DWORD dwCreationFlags,
        ///                                      [in] PVOID lpEnvironment,
        ///                                      [in] LPCWSTR lpCurrentDirectory,
        ///                                      [in] LPSTARTUPINFOW lpStartupInfo,
        ///                                      [in] LPPROCESS_INFORMATION lpProcessInformation,
        ///                                      [in] CorDebugCreateProcessFlags debuggingFlags,
        ///                                      [out] ICorDebugProcess **ppProcess);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateProcess (
            UInt16* lpApplicationName,
            UInt16* lpCommandLine,
            [In] SECURITY_ATTRIBUTES* lpProcessAttributes,
            [In] SECURITY_ATTRIBUTES* lpThreadAttributes,
            [In] Int32 bInheritHandles,
            [In] UInt32 dwCreationFlags,
            [In] void* lpEnvironment,
            UInt16* lpCurrentDirectory,
            [In] STARTUPINFOW* lpStartupInfo,
            [In] PROCESS_INFORMATION* lpProcessInformation,
            [In] CorDebugCreateProcessFlags debuggingFlags,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

        /// <summary>
        /// DebugActiveProcess is used to attach to an existing process.
        ///
        /// If win32Attach is TRUE, then the debugger becomes the Win32
        /// debugger for the process and will begin dispatching the
        /// unmanaged callbacks.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="win32Attach"></param>
        /// <param name="ppProcess"></param>
        /// <example>
        /// <code>HRESULT DebugActiveProcess([in] DWORD id,
        ///                                  [in] BOOL win32Attach,
        ///                                  [out] ICorDebugProcess **ppProcess);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DebugActiveProcess ([In] UInt32 id, [In] Int32 win32Attach,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

        /// <summary>
        /// EnumerateProcesses returns an enum of processes being debugged.
        /// </summary>
        /// <param name="ppProcess"></param>
        /// <example><code>HRESULT EnumerateProcesses([out] ICorDebugProcessEnum **ppProcess);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateProcesses ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcessEnum ppProcess);

        /// <summary>
        /// GetProcess returns the ICorDebugProcess with the given OS Id.
        /// </summary>
        /// <param name="dwProcessId"></param>
        /// <param name="ppProcess"></param>
        /// <example>
        /// <code>HRESULT GetProcess([in] DWORD dwProcessId, [out] ICorDebugProcess **ppProcess);</code>
        /// </example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetProcess ([In] UInt32 dwProcessId, [MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

        /// <summary>
        /// CanLaunchOrAttach returns S_OK if the debugging services believe that
        /// launching a new process or attaching to the given process is possible
        /// given what it knows about the current machine and runtime configuration.
        ///
        /// If you plan to launch with win32 debugging enabled, or to attach with
        /// win32 debugging enabled then pass in TRUE for win32DebuggineEnabled.
        /// The answer may be different if this option will be used.
        ///
        /// Note: the rest of the API will not stop you from launching or attaching
        /// to a process anyway. This function is purely informational.
        ///
        /// Possible HRESULTs: S_OK, CORDBG_E_DEBUGGING_NOT_POSSIBLE,
        /// CORDBG_E_KERNEL_DEBUGGER_PRESENT, CORDBG_E_KERNEL_DEBUGGER_ENABLED
        /// </summary>
        /// <param name="dwProcessId"></param>
        /// <param name="win32DebuggingEnabled"></param>
        /// <example>
        /// <code>HRESULT CanLaunchOrAttach([in] DWORD dwProcessId, [in] BOOL win32DebuggingEnabled);</code>
        /// </example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CanLaunchOrAttach ([In] UInt32 dwProcessId, [In] Int32 win32DebuggingEnabled);
    }
}