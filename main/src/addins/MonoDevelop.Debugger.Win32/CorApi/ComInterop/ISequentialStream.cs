using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  // [
  //    object,
  //    uuid(0c733a30-2a1c-11ce-ade5-00aa0044773d),
  //    pointer_default(unique)
  // ]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ISequentialStream
    {
      /// <summary>
      ///     [local]
      ///    HRESULT Read(
      ///        [annotation("_Out_writes_bytes_to_(cb, *pcbRead)")]
      ///        void *pv,
      ///        [in, annotation("_In_")] ULONG cb,
      ///        [annotation("_Out_opt_")] ULONG *pcbRead);
      ///
      ///    [call_as(Read)]
      ///    HRESULT RemoteRead(
      ///        [out, size_is(cb), length_is(*pcbRead)]
      ///        byte *pv,
      ///        [in] ULONG cb,
      ///        [out] ULONG *pcbRead);
      /// </summary>
      /// <param name="pv"></param>
      /// <param name="cb"></param>
      /// <param name="pcbRead"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void RemoteRead (void *pv, [In] UInt32 cb, UInt32* pcbRead);

      /// <summary>
      ///     [local]
      ///    HRESULT Write(
      ///        [annotation("_In_reads_bytes_(cb)")] void const *pv,
      ///        [in, annotation("_In_")] ULONG cb,
      ///        [annotation("_Out_opt_")] ULONG *pcbWritten);
      ///
      ///    [call_as(Write)]
      ///    HRESULT RemoteWrite(
      ///        [in, size_is(cb)] byte const *pv,
      ///        [in] ULONG cb,
      ///        [out] ULONG *pcbWritten);
      /// </summary>
      /// <param name="pv"></param>
      /// <param name="cb"></param>
      /// <param name="pcbWritten"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void RemoteWrite ([In] void *pv, [In] UInt32 cb, UInt32* pcbWritten);
    }

}