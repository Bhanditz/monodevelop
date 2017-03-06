using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("0000000C-0000-0000-C000-000000000046")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface IStream : ISequentialStream
    {
        /// <inheritdoc cref="ISequentialStream.RemoteRead" />
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 RemoteRead (void *pv, [In] UInt32 cb, UInt32* pcbRead);

        /// <inheritdoc cref="ISequentialStream.RemoteWrite" />
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 RemoteWrite ([In] void *pv, [In] UInt32 cb, UInt32* pcbWritten);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="dlibMove"></param>
      /// <param name="dwOrigin"><see cref="STREAM_SEEK"/></param>
      /// <param name="plibNewPosition"></param>
      /// <example><code>    [local]
      ///    HRESULT Seek(
      ///        [in] LARGE_INTEGER dlibMove,
      ///        [in] DWORD dwOrigin,
      ///        [annotation("_Out_opt_")] ULARGE_INTEGER *plibNewPosition);
      ///
      ///    [call_as(Seek)]
      ///    HRESULT RemoteSeek(
      ///        [in] LARGE_INTEGER dlibMove,
      ///        [in] DWORD dwOrigin,
      ///        [out] ULARGE_INTEGER *plibNewPosition);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 RemoteSeek ([In] LARGE_INTEGER dlibMove, [In] UInt32 dwOrigin, ULARGE_INTEGER *plibNewPosition);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="libNewSize"></param>
      /// <example><code>
      ///     HRESULT SetSize(
      ///        [in] ULARGE_INTEGER libNewSize);
      /// </code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetSize ([In] ULARGE_INTEGER libNewSize);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="pstm"></param>
      /// <param name="cb"></param>
      /// <param name="pcbRead"></param>
      /// <param name="pcbWritten"></param>
      /// <example><code>
      ///     [local]
      ///    HRESULT CopyTo(
      ///        [in, unique, annotation("_In_")] IStream *pstm,
      ///        [in] ULARGE_INTEGER cb,
      ///        [annotation("_Out_opt_")] ULARGE_INTEGER *pcbRead,
      ///        [annotation("_Out_opt_")] ULARGE_INTEGER *pcbWritten);
      ///
      ///    [call_as(CopyTo)]
      ///    HRESULT RemoteCopyTo(
      ///        [in, unique] IStream *pstm,
      ///        [in] ULARGE_INTEGER cb,
      ///        [out] ULARGE_INTEGER *pcbRead,
      ///        [out] ULARGE_INTEGER *pcbWritten);
      ///</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 RemoteCopyTo ([MarshalAs (UnmanagedType.Interface)] [In] IStream pstm, [In] ULARGE_INTEGER cb, ULARGE_INTEGER* pcbRead, ULARGE_INTEGER* pcbWritten);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="grfCommitFlags"></param>
      /// <example><code>
      ///     HRESULT Commit(
      ///        [in] DWORD grfCommitFlags);
      ///</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Commit ([In] UInt32 grfCommitFlags);

      /// <summary>
      /// 
      /// </summary>
      /// <example><code>
      ///     HRESULT Revert();
      /// </code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Revert ();

      /// <summary>
      /// 
      /// </summary>
      /// <param name="libOffset"></param>
      /// <param name="cb"></param>
      /// <param name="dwLockType"></param>
      /// <example><code>
      ///    HRESULT LockRegion(
      ///        [in] ULARGE_INTEGER libOffset,
      ///        [in] ULARGE_INTEGER cb,
      ///        [in] DWORD dwLockType);
      /// </code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LockRegion ([In] ULARGE_INTEGER libOffset, [In] ULARGE_INTEGER cb, [In] UInt32 dwLockType);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="libOffset"></param>
      /// <param name="cb"></param>
      /// <param name="dwLockType"></param>
      /// <example><code>
      ///    HRESULT UnlockRegion(
      ///        [in] ULARGE_INTEGER libOffset,
      ///        [in] ULARGE_INTEGER cb,
      ///        [in] DWORD dwLockType);
      /// </code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 UnlockRegion ([In] ULARGE_INTEGER libOffset, [In] ULARGE_INTEGER cb, [In] UInt32 dwLockType);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="pstatstg"></param>
      /// <param name="grfStatFlag"></param>
      /// <example><code>
      ///    HRESULT Stat(
      ///        [out] STATSTG *pstatstg,
      ///        [in] DWORD grfStatFlag);
      /// </code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Stat (STATSTG *pstatstg, [In] UInt32 grfStatFlag);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="ppstm"></param>
      /// <example><code>
      ///    HRESULT Clone(
      ///        [out] IStream **ppstm);
      /// </code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Clone ([MarshalAs (UnmanagedType.Interface)] out IStream ppstm);
    }
}