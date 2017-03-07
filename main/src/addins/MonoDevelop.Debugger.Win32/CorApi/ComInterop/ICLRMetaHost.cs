using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    ///  ICLRMetaHost
    ///  Activated using mscoree!CLRCreateInstance. Does not do any policy decisions, get
    ///  ICLRMetaHostPolicy if you need that.
    /// </summary>
    /// <example><code>
    ///  /**************************************************************************************
    ///  ** ICLRMetaHost                                                                     **
    ///  ** Activated using mscoree!CLRCreateInstance. Does not do any policy decisions, get **
    ///  ** ICLRMetaHostPolicy if you need that.                                             **
    ///  **************************************************************************************/
    /// [
    ///     uuid(D332DB9E-B9B3-4125-8207-A14884F53216),
    ///     version(1.0),
    ///     helpstring("CLR meta hosting interface"),
    ///     local
    /// ]
    /// interface ICLRMetaHost : IUnknown
    /// {
    ///     /**********************************************************************************
    ///      ** Returns installed runtime with the specific version. Fails if not found.     **
    ///      ** NULL or any other wildcard is not accepted as pwzVersion                     **
    ///      ** Supersedes: CorBindToRuntimeEx with STARTUP_LOADER_SAFEMODE                  **
    ///      **********************************************************************************/
    ///     HRESULT GetRuntime(
    ///         [in]  LPCWSTR pwzVersion,
    ///         [in]  REFIID  riid,   // IID_ICLRRuntimeInfo
    ///         [out, iid_is(riid), retval] LPVOID *ppRuntime);
    /// 
    ///     /**********************************************************************************
    ///      ** Returns runtime version burned into a file's metadata.                       **
    ///      ** Supersedes: GetFileVersion                                                   **
    ///      **********************************************************************************/
    ///     HRESULT GetVersionFromFile(
    ///         [in]      LPCWSTR pwzFilePath,
    ///         [out, size_is(*pcchBuffer), annotation("_Out_writes_all_(*pcchBuffer)")] 
    ///                   LPWSTR pwzBuffer, 
    ///         [in, out] DWORD *pcchBuffer);
    /// 
    ///     /**********************************************************************************
    ///      ** Returns an enumerator of runtimes installed on the machine.                  **
    ///      ** Supersedes: (none)                                                           **
    ///      **********************************************************************************/
    ///     HRESULT EnumerateInstalledRuntimes(
    ///         [out, retval] IEnumUnknown **ppEnumerator);
    /// 
    ///     /**********************************************************************************
    ///      ** Provides an enumerator of runtimes loaded into the given process.            **
    ///      ** Supersedes: GetVersionFromProcess                                            **
    ///      **********************************************************************************/
    ///     HRESULT EnumerateLoadedRuntimes(
    ///         [in]  HANDLE hndProcess,
    ///         [out, retval] IEnumUnknown **ppEnumerator);
    /// 
    ///     /**********************************************************************************
    ///      ** Provides a callback when a new runtime version has just been loaded, but not **
    ///      ** started.                                                                     **
    ///      **                                                                              **
    ///      ** The callback works in the following way:                                     **
    ///      **   - the callback is invoked only when a runtime is loaded for the first time **
    ///      **   - the callback will not be invoked for reentrant loads of the same runtime **
    ///      **   - the callback will be for reentrant loads of other runtimes               **
    ///      **   - it is guaranteed that no other thread may load the runtime until the     **
    ///      **     callback returns; any thread that does so blocks until this time.        **
    ///      **   - for non-reentrant multi-threaded runtime loads, callbacks are serialized **
    ///      **   - if the host intends to load (or cause to be loaded) another runtime in a **
    ///      **     reentrant fashion, or intends to perform any operations on the runtime   **
    ///      **     corresponding to the callback instance, the pfnCallbackThreadSet and     **
    ///      **     pfnCallbackThreadUnset arguments provided in the callback must be used   **
    ///      **     in the following way:                                                    **
    ///      **     - pfnCallbackThreadSet must be called by the thread that might cause a   **
    ///      **       runtime load before such a load is attempted                           **
    ///      **     - pfnCallbackThreadUnset must be called when the thread will no longer   **
    ///      **       cause such a runtime load (and before returning from the initial       **
    ///      **       callback)                                                              **
    ///      **     - pfnCallbackThreadSet and pfnCallbackThreadUnset are non-reentrant.     **
    ///      **                                                                              **
    ///      **   pCallbackFunction: This function is invoked when a new runtime has just    **
    ///      **                      been loaded. A value of NULL results in a failure       **
    ///      **                      return value of E_POINTER.                              **
    ///      **                                                                              **
    ///      ** Supersedes: LockClrVersion                                                   **
    ///      **********************************************************************************/
    ///     HRESULT RequestRuntimeLoadedNotification(
    ///         [in] RuntimeLoadedCallbackFnPtr pCallbackFunction);
    /// 
    ///     /**********************************************************************************
    ///      ** Returns interface representing the runtime to which the legacy activation    **
    ///      ** policy has been bound (for example, by a useLegacyV2RuntimeActivationPolicy  **
    ///      ** config entry or by a call to ICLRRuntimeInfo::BindAsLegacyV2Runtime).        **
    ///      **********************************************************************************/
    ///     HRESULT QueryLegacyV2RuntimeBinding(
    ///         [in] REFIID riid,
    ///         [out, iid_is(riid), retval] LPVOID *ppUnk);
    /// 
    ///     /**********************************************************************************
    ///      ** Shuts down the current process.                                              **
    ///      ** Supersedes: CorExitProcess                                                   **
    ///      **********************************************************************************/
    ///     HRESULT ExitProcess(
    ///         [in] INT32 iExitCode);
    /// } // interface ICLRMetaHost
    /// 
    ///  
    ///   </code></example>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("D332DB9E-B9B3-4125-8207-A14884F53216")]
    [ComImport]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICLRMetaHost
    {
        /// <summary>
        /// Returns installed runtime with the specific version. Fails if not found.
        /// NULL or any other wildcard is not accepted as pwzVersion
        /// Supersedes: CorBindToRuntimeEx with STARTUP_LOADER_SAFEMODE
        /// </summary>
        /// <param name="pwzVersion"></param>
        /// <param name="riid"></param>
        /// <param name="ppRuntime"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 GetRuntime([In] UInt16* pwzVersion, [In] Guid* riid, // IID_ICLRRuntimeInfo
            [Out] void**ppRuntime);

        /// <summary>
        /// Returns runtime version burned into a file's metadata.
        /// Supersedes: GetFileVersion
        /// </summary>
        /// <param name="pwzFilePath"></param>
        /// <param name="pwzBuffer"></param>
        /// <param name="pcchBuffer"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 GetVersionFromFile([In] UInt16* pwzFilePath, [Out] UInt16* pwzBuffer, [In] [Out] UInt32*pcchBuffer);

        /// <summary>
        /// Returns an enumerator of runtimes installed on the machine.
        /// Supersedes: (none)
        /// </summary>
        /// <param name="ppEnumerator"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 EnumerateInstalledRuntimes([Out] void**ppEnumerator);

        /// <summary>
        /// Provides an enumerator of runtimes loaded into the given process.
        /// Supersedes: GetVersionFromProcess
        /// </summary>
        /// <param name="hndProcess"></param>
        /// <param name="ppEnumerator"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 EnumerateLoadedRuntimes([In] void* hndProcess, [Out] void**ppEnumerator);

        /// <summary>
        /// Provides a callback when a new runtime version has just been loaded, but not
        /// started.
        /// The callback works in the following way:
        ///   - the callback is invoked only when a runtime is loaded for the first time
        ///   - the callback will not be invoked for reentrant loads of the same runtime
        ///   - the callback will be for reentrant loads of other runtimes
        ///   - it is guaranteed that no other thread may load the runtime until the
        ///     callback returns; any thread that does so blocks until this time.
        ///   - for non-reentrant multi-threaded runtime loads, callbacks are serialized
        ///   - if the host intends to load (or cause to be loaded) another runtime in a
        ///     reentrant fashion, or intends to perform any operations on the runtime
        ///     corresponding to the callback instance, the pfnCallbackThreadSet and
        ///     pfnCallbackThreadUnset arguments provided in the callback must be used
        ///     in the following way:
        ///     - pfnCallbackThreadSet must be called by the thread that might cause a
        ///       runtime load before such a load is attempted
        ///     - pfnCallbackThreadUnset must be called when the thread will no longer
        ///       cause such a runtime load (and before returning from the initial
        ///       callback)
        ///     - pfnCallbackThreadSet and pfnCallbackThreadUnset are non-reentrant.
        ///   pCallbackFunction: This function is invoked when a new runtime has just
        ///                      been loaded. A value of NULL results in a failure
        ///                      return value of E_POINTER.
        /// Supersedes: LockClrVersion
        /// </summary>
        /// <param name="pCallbackFunction"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 RequestRuntimeLoadedNotification([In] [ComAliasName("RuntimeLoadedCallbackFnPtr")] void* pCallbackFunction);

        /// <summary>
        /// Returns interface representing the runtime to which the legacy activation
        /// policy has been bound (for example, by a useLegacyV2RuntimeActivationPolicy
        /// config entry or by a call to ICLRRuntimeInfo::BindAsLegacyV2Runtime).
        /// </summary>
        /// <param name="riid"></param>
        /// <param name="ppUnk"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 QueryLegacyV2RuntimeBinding([In] Guid* riid, [Out] void**ppUnk);

        /// <summary>
        /// Shuts down the current process.
        /// Supersedes: CorExitProcess
        /// </summary>
        /// <param name="iExitCode"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        Int32 ExitProcess([In] Int32 iExitCode);
    }
}