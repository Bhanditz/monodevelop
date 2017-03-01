using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  [
  ///     object,
  ///     local,
  ///     uuid(C0815BDC-CFAB-447e-A779-C116B454EB5B),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugInternalFrame2 : IUnknown
  /// {
  ///     /*
  ///      * Returns the stack address of the internal frame marker.
  ///      */
  ///     HRESULT GetAddress([out] CORDB_ADDRESS *pAddress);
  /// 
  ///      * Check if an internal frame is closer to the leaf than pFrameToCompare.
  ///      */
  ///     HRESULT IsCloserToLeaf([in] ICorDebugFrame * pFrameToCompare, 
  ///                            [out] BOOL * pIsCloser);
  /// };
  ///  </code></example>
  [Guid ("C0815BDC-CFAB-447E-A779-C116B454EB5B")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugInternalFrame2
  {
    /// <summary>
    /// Returns the stack address of the internal frame marker.
    /// </summary>
    /// <param name="pAddress"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAddress (UInt64* pAddress);

    /// <summary>
    /// Check if an internal frame is closer to the leaf than pFrameToCompare.
    /// </summary>
    /// <param name="pFrameToCompare"></param>
    /// <param name="pIsCloser"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsCloserToLeaf ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFrame pFrameToCompare, Int32* pIsCloser);
  }
}