using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /// <summary>
    /// ICorDebugAppDomain2
    /// </summary>
    /// <example>
    /// <code>[
    ///     object,
    ///     local,
    ///     uuid(096E81D5-ECDA-4202-83F5-C65980A9EF75),
    ///     pointer_default(unique)
    /// ]</code></example>
    [Guid ("096E81D5-ECDA-4202-83F5-C65980A9EF75")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugAppDomain2
    {
        /// <summary>
        /// GetArrayOrPointerType returns an array, pointer, byref or function pointer type.
        /// elementType indicated the kind of type to be created and
        /// must be one of ELEMENT_TYPE_PTR, ELEMENT_TYPE_BYREF,
        /// ELEMENT_TYPE_ARRAY or ELEMENT_TYPE_SZARRAY.  If used with
        /// ELEMENT_TYPE_PTR or ELEMENT_TYPE_BYREF then nRank must be zero.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="nRank"></param>
        /// <param name="pTypeArg"></param>
        /// <param name="ppType"></param>
        /// <example><code>HRESULT GetArrayOrPointerType([in] CorElementType elementType,
        ///                                              [in] ULONG32 nRank,
        ///                                              [in] ICorDebugType *pTypeArg,
        ///                                              [out] ICorDebugType **ppType);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetArrayOrPointerType (
            [In] CorElementType elementType,
            [In] UInt32 nRank,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugType pTypeArg,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);

        /// <summary>
        /// GetFunctionPointerType returns a function pointer type.
        /// This corresponds to ELEMENT_TYPE_FNPTR.  The first type in the type arguments
        /// must be the return type and the remainder the argument types.
        /// </summary>
        /// <param name="nTypeArgs"></param>
        /// <param name="ppTypeArgs"></param>
        /// <param name="ppType"></param>
        /// <example><code>HRESULT GetFunctionPointerType([in] ULONG32 nTypeArgs,
        ///                                               [in, size_is(nTypeArgs)] ICorDebugType *ppTypeArgs[],
        ///                                               [out] ICorDebugType **ppType);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunctionPointerType (
            [In] UInt32 nTypeArgs,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugType[] ppTypeArgs,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);
    }
}