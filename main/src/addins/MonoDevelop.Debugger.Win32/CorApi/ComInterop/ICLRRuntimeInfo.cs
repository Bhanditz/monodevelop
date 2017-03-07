using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    ///   ICLRRuntimeInfo
    ///   Represents a runtime - installed on the machine and/or loaded into a process.
    ///   Includes functionality for obtaining various properties and for loading the
    ///   runtime into the current process. The same installed runtime can be loaded
    ///   multiple times in the same process (may not be supported in Dev10).
    /// </summary>
    /// <example><code>
    ///   /**************************************************************************************
    ///  ** ICLRRuntimeInfo                                                                  **
    ///  ** Represents a runtime - installed on the machine and/or loaded into a process.    **
    ///  ** Includes functionality for obtaining various properties and for loading the      **
    ///  ** runtime into the current process. The same installed runtime can be loaded       **
    ///  ** multiple times in the same process (may not be supported in Dev10).              **
    ///  **************************************************************************************/
    /// [
    ///     uuid(BD39D1D2-BA2F-486a-89B0-B4B0CB466891),
    ///     version(1.0),
    ///     helpstring("CLR runtime instance"),
    ///     local
    /// ]
    /// interface ICLRRuntimeInfo : IUnknown
    /// {
    ///     // Methods that query information:
    /// 
    ///     /**********************************************************************************
    ///      ** Returns the version of this runtime in the usual v-prefixed dotted form.     **
    ///      ** Supersedes: GetRequestedRuntimeInfo, GetRequestedRuntimeVersion,             **
    ///      **     GetCORVersion                                                            **
    ///      **********************************************************************************/
    ///     HRESULT GetVersionString(
    ///         [out, size_is(*pcchBuffer), annotation("_Out_writes_all_opt_(*pcchBuffer)")] 
    ///                    LPWSTR pwzBuffer, 
    ///         [in, out]  DWORD *pcchBuffer);
    /// 
    ///     /**********************************************************************************
    ///      ** Returns the directory where this runtime is installed.                       **
    ///      ** Supersedes: GetCORSystemDirectory                                            **
    ///      **********************************************************************************/
    ///     HRESULT GetRuntimeDirectory(
    ///         [out, size_is(*pcchBuffer), annotation("_Out_writes_all_(*pcchBuffer)")] 
    ///                    LPWSTR pwzBuffer, 
    ///         [in, out]  DWORD *pcchBuffer);
    /// 
    ///     /**********************************************************************************
    ///      ** Returns TRUE if this runtime is loaded into the specified process.           **
    ///      ** Supersedes: GetCORSystemDirectory                                            **
    ///      **********************************************************************************/
    ///     HRESULT IsLoaded(
    ///         [in]  HANDLE hndProcess,
    ///         [out, retval] BOOL *pbLoaded);
    /// 
    ///     // Methods that may load the runtime:
    /// 
    ///     /**********************************************************************************
    ///      ** Translates an HRESULT value into an error message. Use iLocaleID -1 for the  **
    ///      ** default culture of the current thread.                                       **
    ///      ** Supersedes: LoadStringRC, LoadStringRCEx                                     **
    ///      **********************************************************************************/
    ///     HRESULT LoadErrorString(
    ///         [in]       UINT iResourceID, 
    ///         [out, size_is(*pcchBuffer), annotation("_Out_writes_all_(*pcchBuffer)")] 
    ///                    LPWSTR pwzBuffer, 
    ///         [in, out]  DWORD *pcchBuffer,
    ///         [in, lcid] LONG iLocaleID);
    /// 
    ///     /**********************************************************************************
    ///      ** Loads a library located alongside this runtime.                              **
    ///      ** Supersedes: LoadLibraryShim                                                  **
    ///      **********************************************************************************/
    ///     HRESULT LoadLibrary(
    ///         [in]  LPCWSTR pwzDllName, 
    ///         [out, retval] HMODULE *phndModule);
    /// 
    ///     /**********************************************************************************
    ///      ** Gets the address of the specified function exported from this runtime.       **
    ///      ** It should NOT be documented what module the function lives in. We may want   **
    ///      ** to implement some forwarding policy here. The reason for exposing            **
    ///      ** GetProcAddress are functions like mscorwks!GetCLRIdentityManager.            **
    ///      ** Supersedes: GetRealProcAddress                                               **
    ///      **********************************************************************************/
    ///     HRESULT GetProcAddress(
    ///         [in]  LPCSTR pszProcName, 
    ///         [out, retval] LPVOID *ppProc);
    /// 
    ///     /**********************************************************************************
    ///      ** Loads the runtime into the current process and returns an interface through  **
    ///      ** which runtime functionality is provided.                                     **
    ///      **                                                                              **
    ///      ** Supported CLSIDs/IIDs:                                                       **
    ///      ** CLSID_CorMetaDataDispenser   IID_IMetaDataDispenser,IID_IMetaDataDispenserEx **
    ///      ** CLSID_CorMetaDataDispenserRuntime  dtto                                      **
    ///      ** CLSID_CorRuntimeHost         IID_ICorRuntimeHost                             **
    ///      ** CLSID_CLRRuntimeHost         IID_ICLRRuntimeHost                             **
    ///      ** CLSID_TypeNameFactory        IID_ITypeNameFactory                            **
    ///      ** CLSID_CLRStrongName          IID_ICLRStrongName                              **
    ///      ** CLSID_CLRDebuggingLegacy     IID_ICorDebug                                   **
    ///      ** CLSID_CLRProfiling           IID_ICLRProfiling                               **
    ///      **                                                                              **
    ///      ** Supersedes: CorBindTo* and others                                            **
    ///      **********************************************************************************/
    ///     HRESULT GetInterface(
    ///         [in]  REFCLSID rclsid,
    ///         [in]  REFIID   riid,
    ///         [out, iid_is(riid), retval] LPVOID *ppUnk);
    /// 
    ///     // Does not load runtime
    /// 
    ///     /**********************************************************************************
    ///      ** Returns TRUE if this runtime could be loaded into the current process. Note  **
    ///      ** that this method is side-effect free, and thus does not represent a			 **
    ///      ** commitment to be able to load this runtime if it sets *pbLoadable to be TRUE.**
    ///      ** Supersedes: none                                                             **
    ///      **********************************************************************************/
    ///     HRESULT IsLoadable(
    ///         [out, retval] BOOL *pbLoadable);
    /// 
    ///     /**********************************************************************************
    ///      ** Sets startup flags and host config file that will be used at startup.        **
    ///      ** Supersedes: The startupFlags parameter in CorBindToRuntimeEx/Host            **
    ///      **********************************************************************************/
    ///     HRESULT SetDefaultStartupFlags(
    ///         [in]  DWORD dwStartupFlags,
    ///         [in]  LPCWSTR pwzHostConfigFile);
    /// 
    ///     /**********************************************************************************
    ///      ** Gets startup flags and host config file that will be used at startup.        **
    ///      ** Supersedes: GetStartupFlags, GetHostConfigurationFile                        **
    ///      **********************************************************************************/
    ///     HRESULT GetDefaultStartupFlags(
    ///         [out]      DWORD *pdwStartupFlags,
    ///         [out, size_is(*pcchHostConfigFile), annotation("_Out_writes_all_opt_(*pcchHostConfigFile)")]
    ///                    LPWSTR pwzHostConfigFile, 
    ///         [in, out]  DWORD *pcchHostConfigFile);
    /// 
    ///     /**********************************************************************************
    ///      ** If not already bound (for example, with a useLegacyV2RuntimeActivationPolicy **
    ///      ** config setting), binds this runtime for all legacy V2 activation policy      **
    ///      ** decisions. If a different runtime was already bound to the legacy V2         **
    ///      ** activation policy, returns CLR_E_SHIM_LEGACYRUNTIMEALREADYBOUND. If this     **
    ///      ** runtime was already bound as the legacy V2 activation policy runtime,        **
    ///      ** returns S_OK.                                                                **
    ///      **********************************************************************************/
    ///     HRESULT BindAsLegacyV2Runtime();
    /// 
    ///     /**********************************************************************************
    ///      ** Returns TRUE if the runtime has been started, i.e. Start() has been called.  **
    ///      ** If it has been started, its STARTUP_FLAGS are returned.                      **
    ///      **                                                                              **
    ///      ** IMPORTANT: This method is valid only for v4+ runtimes. This method will      **
    ///      ** return E_NOTIMPL for all pre-v4 runtimes.                                    **
    ///      **********************************************************************************/
    ///     HRESULT IsStarted(
    ///         [out] BOOL *pbStarted,
    ///         [out] DWORD *pdwStartupFlags);
    /// };
    /// 
    ///   </code></example>
    [ComImport]
    [Guid("BD39D1D2-BA2F-486a-89B0-B4B0CB466891")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public unsafe interface ICLRRuntimeInfo
    {
        /// <summary>
        /// Returns the version of this runtime in the usual v-prefixed dotted form.
        /// Supersedes: GetRequestedRuntimeInfo, GetRequestedRuntimeVersion,
        ///     GetCORVersion
        /// </summary>
        /// <param name="pwzBuffer"></param>
        /// <param name="pcchBuffer"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int GetVersionString([Out] ushort* pwzBuffer, [In] [Out] uint*pcchBuffer);

        /// <summary>
        /// Returns the directory where this runtime is installed.
        /// Supersedes: GetCORSystemDirectory
        /// </summary>
        /// <param name="pwzBuffer"></param>
        /// <param name="pcchBuffer"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int GetRuntimeDirectory([Out] ushort* pwzBuffer, [In] [Out] uint*pcchBuffer);

        /// <summary>
        /// Returns TRUE if this runtime is loaded into the specified process.
        /// Supersedes: GetCORSystemDirectory
        /// </summary>
        /// <param name="hndProcess"></param>
        /// <param name="pbLoaded"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int IsLoaded([In] void* hndProcess, [Out] int*pbLoaded);

        /// <summary>
        /// Translates an HRESULT value into an error message. Use iLocaleID -1 for the
        /// default culture of the current thread.
        /// Supersedes: LoadStringRC, LoadStringRCEx
        /// </summary>
        /// <param name="iResourceID"></param>
        /// <param name="pwzBuffer"></param>
        /// <param name="pcchBuffer"></param>
        /// <param name="iLocaleID"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int LoadErrorString([In] uint iResourceID, [Out] ushort* pwzBuffer, [In] [Out] uint*pcchBuffer, [In] int iLocaleID);

        /// <summary>
        /// Loads a library located alongside this runtime.
        /// Supersedes: LoadLibraryShim
        /// </summary>
        /// <param name="pwzDllName"></param>
        /// <param name="phndModule"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int LoadLibrary([In] ushort* pwzDllName, [Out] void**phndModule);

        /// <summary>
        /// Gets the address of the specified function exported from this runtime.
        /// It should NOT be documented what module the function lives in. We may want
        /// to implement some forwarding policy here. The reason for exposing
        /// GetProcAddress are functions like mscorwks!GetCLRIdentityManager.
        /// Supersedes: GetRealProcAddress
        /// </summary>
        /// <param name="pszProcName"></param>
        /// <param name="ppProc"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int GetProcAddress([In] byte* pszProcName, [Out] void**ppProc);

        /// <summary>
        /// Loads the runtime into the current process and returns an interface through
        /// which runtime functionality is provided.
        /// Supported CLSIDs/IIDs:
        /// CLSID_CorMetaDataDispenser   IID_IMetaDataDispenser,IID_IMetaDataDispenserEx
        /// CLSID_CorMetaDataDispenserRuntime  dtto
        /// CLSID_CorRuntimeHost         IID_ICorRuntimeHost
        /// CLSID_CLRRuntimeHost         IID_ICLRRuntimeHost
        /// CLSID_TypeNameFactory        IID_ITypeNameFactory
        /// CLSID_CLRStrongName          IID_ICLRStrongName
        /// CLSID_CLRDebuggingLegacy     IID_ICorDebug
        /// CLSID_CLRProfiling           IID_ICLRProfiling
        /// Supersedes: CorBindTo* and others
        /// </summary>
        /// <param name="rclsid"></param>
        /// <param name="riid"></param>
        /// <param name="ppUnk"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int GetInterface([In] Guid* rclsid, [In] Guid* riid, [Out] void**ppUnk);

        /// <summary>
        /// Returns TRUE if this runtime could be loaded into the current process. Note
        /// that this method is side-effect free, and thus does not represent a
        /// commitment to be able to load this runtime if it sets *pbLoadable to be TRUE.
        /// Supersedes: none
        /// </summary>
        /// <param name="pbLoadable"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int IsLoadable([Out] int*pbLoadable);

        /// <summary>
        /// Sets startup flags and host config file that will be used at startup.
        /// Supersedes: The startupFlags parameter in CorBindToRuntimeEx/Host
        /// </summary>
        /// <param name="dwStartupFlags"></param>
        /// <param name="pwzHostConfigFile"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int SetDefaultStartupFlags([In] uint dwStartupFlags, [In] ushort* pwzHostConfigFile);

        /// <summary>
        /// Gets startup flags and host config file that will be used at startup.
        /// Supersedes: GetStartupFlags, GetHostConfigurationFile
        /// </summary>
        /// <param name="pdwStartupFlags"></param>
        /// <param name="pwzHostConfigFile"></param>
        /// <param name="pcchHostConfigFile"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int GetDefaultStartupFlags([Out] uint*pdwStartupFlags, [Out] ushort* pwzHostConfigFile, [In] [Out] uint*pcchHostConfigFile);

        /// <summary>
        /// If not already bound (for example, with a useLegacyV2RuntimeActivationPolicy
        /// config setting), binds this runtime for all legacy V2 activation policy
        /// decisions. If a different runtime was already bound to the legacy V2
        /// activation policy, returns CLR_E_SHIM_LEGACYRUNTIMEALREADYBOUND. If this
        /// runtime was already bound as the legacy V2 activation policy runtime,
        /// returns S_OK.
        /// </summary>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int BindAsLegacyV2Runtime();

        /// <summary>
        /// Returns TRUE if the runtime has been started, i.e. Start() has been called.
        /// If it has been started, its STARTUP_FLAGS are returned.
        /// IMPORTANT: This method is valid only for v4+ runtimes. This method will
        /// return E_NOTIMPL for all pre-v4 runtimes.
        /// </summary>
        /// <param name="pbStarted"></param>
        /// <param name="pdwStartupFlags"></param>
        /// <returns></returns>
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int IsStarted([Out] int*pbStarted, [Out] uint*pdwStartupFlags);
    }
}