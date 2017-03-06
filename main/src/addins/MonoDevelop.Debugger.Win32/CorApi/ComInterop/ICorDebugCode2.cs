using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  
  /// [
  ///     object,
  ///     local,
  ///     uuid(5F696509-452F-4436-A3FE-4D11FE7E2347),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugCode2 : IUnknown
  /// {
  ///     typedef struct _CodeChunkInfo
  ///     {
  ///         CORDB_ADDRESS startAddr;
  ///         ULONG32 length;
  ///     } CodeChunkInfo;
  /// 
  ///     // The native code for a code object may be split up into multiple regions.
  ///     //
  ///     HRESULT GetCodeChunks(
  ///         [in] ULONG32 cbufSize,
  ///         [out] ULONG32 * pcnumChunks,
  ///         [out, size_is(cbufSize), length_is(*pcnumChunks)] CodeChunkInfo chunks[]);
  /// 
  /// 
  ///    // GetCompilerFlags returns the flags under which this piece of code was JITted or NGENed.
  /// 
  ///    HRESULT GetCompilerFlags( [out] DWORD *pdwFlags );
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("5F696509-452F-4436-A3FE-4D11FE7E2347")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugCode2
  {
    /// <summary>
    /// The native code for a code object may be split up into multiple regions.
    /// </summary>
    /// <param name="cbufSize"></param>
    /// <param name="pcnumChunks"></param>
    /// <param name="chunks"></param>
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCodeChunks ([In] UInt32 cbufSize, [Out] UInt32* pcnumChunks, CodeChunkInfo* chunks);

    /// <summary>
    /// GetCompilerFlags returns the flags under which this piece of code was JITted or NGENed.
    /// </summary>
    /// <param name="pdwFlags"></param>
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCompilerFlags ([Out] UInt32* pdwFlags);
  }
}