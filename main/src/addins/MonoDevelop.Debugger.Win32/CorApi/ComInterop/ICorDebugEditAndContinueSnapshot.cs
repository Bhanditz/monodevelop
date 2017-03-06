using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  DEPRECATED
  /// </summary>
  /// <example><code>
  ///  *
  ///  * DEPRECATED
  ///  *
  ///  * ICorDebugEditAndContinueSnapshot
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(6DC3FA01-D7CB-11d2-8A95-0080C792E5D8),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugEditAndContinueSnapshot : IUnknown
  /// {
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT CopyMetaData([in] IStream *pIStream, [out] GUID *pMvid);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT GetMvid([out] GUID *pMvid);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT GetRoDataRVA([out] ULONG32 *pRoDataRVA);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT GetRwDataRVA([out] ULONG32 *pRwDataRVA);
  /// 
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT SetPEBytes([in] IStream *pIStream);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT SetILMap([in] mdToken mdFunction, [in] ULONG cMapSize,
  ///                      [in, size_is(cMapSize)] COR_IL_MAP map[]);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  ///     HRESULT SetPESymbolBytes([in] IStream *pIStream);
  /// };
  ///  </code></example>
  [Obsolete ("DEPRECATED")]
  [Guid ("6DC3FA01-D7CB-11D2-8A95-0080C792E5D8")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugEditAndContinueSnapshot
  {
    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CopyMetaData (void* pIStream, Guid* pMvid);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetMvid (Guid* pMvid);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetRoDataRVA (UInt32* pRoDataRVA);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetRwDataRVA (UInt32* pRwDataRVA);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetPEBytes (void* pIStream);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetILMap ([In] UInt32 mdFunction, [In] UInt32 cMapSize, [In] COR_IL_MAP* map);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetPESymbolBytes (void* pIStream);
  }
}