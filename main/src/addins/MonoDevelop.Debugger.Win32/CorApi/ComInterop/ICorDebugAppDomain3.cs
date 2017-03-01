using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /// <summary>
    /// ICorDebugAppDomain3
    /// </summary>
    /// <example><code>[
    ///     object,
    ///     local,
    ///     uuid(8CB96A16-B588-42E2-B71C-DD849FC2ECCC),
    ///     pointer_default(unique)
    /// ]</code></example>
    [Guid ("8CB96A16-B588-42E2-B71C-DD849FC2ECCC")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugAppDomain3
    {
        /// <summary>
        /// Returns an enumeration of types corresponding to the IIDs passed in
        /// guidsToResolve. The enumeration will have the same cReqTypes elements
        /// with NULL values corresponding to unknown IIDs.
        /// </summary>
        /// <param name="cReqTypes"></param>
        /// <param name="iidsToResolve"></param>
        /// <param name="ppTypesEnum"></param>
        /// <example><code>
        /// HRESULT GetCachedWinRTTypesForIIDs(
        ///      [in]                      ULONG32              cReqTypes,
        ///      [in, size_is(cReqTypes)]  GUID               * iidsToResolve,
        ///      [out]                     ICorDebugTypeEnum ** ppTypesEnum);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCachedWinRTTypesForIIDs (
            [In] UInt32 cReqTypes,
            [In] Guid *iidsToResolve,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugTypeEnum ppTypesEnum);

        /// <summary>
        /// Returns an enumeration of IID / Type pairs. This is the exhaustive
        /// list of pairs as they were cached in the current app domain.
        /// </summary>
        /// <param name="ppGuidToTypeEnum"></param>
        /// <example><code>
        /// HRESULT GetCachedWinRTTypes(
        ///      [out] ICorDebugGuidToTypeEnum ** ppGuidToTypeEnum);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCachedWinRTTypes ([MarshalAs (UnmanagedType.Interface)] out ICorDebugGuidToTypeEnum ppGuidToTypeEnum);
    }
}