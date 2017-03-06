using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.Pinvoke
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public static unsafe class MscoreeDll
    {
        /// <summary>
        ///  This should be the only flat public API exposed from mscoree going forward.
        ///  The returned interface is likely to be implemented in a separate versioned DLL
        ///  (mscorhst.dll living in the versioned directory for instance). Acceptable values
        ///  for riid in v4.0 are IID_ICLRMetaHost, IID_ICLRMetaHostPolicy and
        ///  IID_ICLRDebugging.
        /// </summary>
        /// <example><code>
        ///  /**************************************************************************************
        ///  ** This should be the only flat public API exposed from mscoree going forward.      **
        ///  ** The returned interface is likely to be implemented in a separate versioned DLL   **
        ///  ** (mscorhst.dll living in the versioned directory for instance). Acceptable values **
        ///  ** for riid in v4.0 are IID_ICLRMetaHost, IID_ICLRMetaHostPolicy and                **
        ///  ** IID_ICLRDebugging.                                                               **
        ///  **************************************************************************************/
        /// cpp_quote("STDAPI CLRCreateInstance(REFCLSID clsid, REFIID riid, /*iid_is(riid)*/ LPVOID *ppInterface);")
        /// 
        ///  </code></example>
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [PreserveSig]
        [MustUseReturnValue]
        public static extern Int32 CLRCreateInstance(Guid* clsid, Guid* riid, void**ppInterface);

        /// <summary>
        /// </summary>
        /// <example><code>
        /// #pragma midl_echo("DEPRECATED_CLR_STDAPI CreateDebuggingInterfaceFromVersion(int iDebuggerVersion, LPCWSTR szDebuggeeVersion, IUnknown ** ppCordb);")
        /// </code></example>
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [PreserveSig]
        [MustUseReturnValue]
        public static extern Int32 CreateDebuggingInterfaceFromVersion(Int32 iDebuggerVersion, UInt16* szDebuggeeVersion, void**ppCordb);

        /// <summary>
        /// </summary>
        /// <example><code>
        /// #pragma midl_echo("DEPRECATED_CLR_STDAPI GetCORVersion(_Out_writes_to_(cchBuffer, *dwLength) LPWSTR pbBuffer, DWORD cchBuffer, DWORD* dwLength);")
        /// </code></example>
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [PreserveSig]
        [MustUseReturnValue]
        public static extern Int32 GetCORVersion(UInt16*pbBuffer, UInt32 cchBuffer, UInt32* dwLength);

        /// <summary>
        /// </summary>
        /// <example><code>
        /// #pragma midl_echo("DEPRECATED_CLR_STDAPI GetRequestedRuntimeVersion(_In_ LPWSTR pExe, _Out_writes_to_(cchBuffer, *dwLength) LPWSTR pVersion, DWORD cchBuffer, _Out_ DWORD* dwLength);")
        /// </code></example>
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [PreserveSig]
        [MustUseReturnValue]
        public static extern Int32 GetRequestedRuntimeVersion(UInt16* pExe, UInt16* pVersion, UInt32 cchBuffer, UInt32* dwLength);

        /// <summary>
        /// </summary>
        /// <example><code>
        /// #pragma midl_echo("DEPRECATED_CLR_STDAPI GetVersionFromProcess(HANDLE hProcess, _Out_writes_to_(cchBuffer, *dwLength) LPWSTR pVersion, DWORD cchBuffer, _Out_ DWORD* dwLength);")
        /// </code></example>
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [PreserveSig]
        [MustUseReturnValue]
        public static extern Int32 GetVersionFromProcess(void* hProcess, UInt16* pVersion, UInt32 cchBuffer, UInt32* dwLength);

        /// <example><code>
        ///  // CLSID_CLRMetaHost : uuid(9280188D-0E8E-4867-B30C-7FA83884E8DE)
        /// cpp_quote("EXTERN_GUID(CLSID_CLRMetaHost, 0x9280188d, 0xe8e, 0x4867, 0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde);")
        ///  </code></example>
        [Guid("9280188D-0E8E-4867-B30C-7FA83884E8DE")]
        public static class CLSID_CLRMetaHost
        {
        }
    }
}