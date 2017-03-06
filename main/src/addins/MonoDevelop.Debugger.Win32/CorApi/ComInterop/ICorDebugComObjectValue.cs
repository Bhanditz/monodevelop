using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugComObjectValue applies to values which contain a COM object.
  ///  An ICorDebugComObjectValue becomes invalid after the debuggee is continued.
  /// </summary>
  /// <example><code>
  ///  
  ///  /*
  ///  * ICorDebugComObjectValue applies to values which contain a COM object.
  ///  * An ICorDebugComObjectValue becomes invalid after the debuggee is continued.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(5F69C5E5-3E12-42DF-B371-F9D761D6EE24),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugComObjectValue : IUnknown
  /// {
  ///     /*
  ///      * GetCachedInterfaceTypes returns an enum of the types of all interfaces
  ///      * that are cached by the COM object.
  ///      */
  ///     HRESULT GetCachedInterfaceTypes(
  ///                         [in] BOOL bIInspectableOnly, 
  ///                         [out] ICorDebugTypeEnum **ppInterfacesEnum);
  /// 
  ///     /*
  ///      * GetCachedInterfacePointers returns at most celt values of the
  ///      * interface pointer values cached by the COM object. It fills
  ///      * pcEltFetched with the actual number of fetched elements. 
  ///      * When called with NULL for ptrs, and 0 for celt, it simply returns
  ///      * the number of elements it needs.
  ///      */
  ///     HRESULT GetCachedInterfacePointers(
  ///                         [in] BOOL bIInspectableOnly, 
  ///                         [in] ULONG32 celt,
  ///                         [out] ULONG32 *pcEltFetched,
  ///                         [out, size_is(celt), length_is(*pcEltFetched)] CORDB_ADDRESS * ptrs);
  /// };
  /// 
  ///  </code></example>
  [Guid ("5F69C5E5-3E12-42DF-B371-F9D761D6EE24")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugComObjectValue
  {
    /// <summary>
    /// GetCachedInterfaceTypes returns an enum of the types of all interfaces
    /// that are cached by the COM object.
    /// </summary>
    /// <param name="bIInspectableOnly"></param>
    /// <param name="ppInterfacesEnum"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCachedInterfaceTypes ([In] Int32 bIInspectableOnly, [MarshalAs (UnmanagedType.Interface)] out ICorDebugTypeEnum ppInterfacesEnum);

    /// <summary>
    /// GetCachedInterfacePointers returns at most celt values of the
    /// interface pointer values cached by the COM object. It fills
    /// pcEltFetched with the actual number of fetched elements.
    /// When called with NULL for ptrs, and 0 for celt, it simply returns
    /// the number of elements it needs.
    /// </summary>
    /// <param name="bIInspectableOnly"></param>
    /// <param name="celt"></param>
    /// <param name="pceltFetched"></param>
    /// <param name="ptrs"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCachedInterfacePointers ([In] Int32 bIInspectableOnly, [In] UInt32 celt, UInt32* pceltFetched, [ComAliasName ("CORDB_ADDRESS*")] UInt64* ptrs);
  }
}