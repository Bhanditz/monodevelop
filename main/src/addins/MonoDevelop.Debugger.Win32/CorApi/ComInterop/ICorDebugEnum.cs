using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugEnum is an abstract enumerator.
  /// </summary>
  /// <example><code>
  ///  * ICorDebugEnum is an abstract enumerator.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCB01-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugEnum : IUnknown
  /// {
  ///     /*
  ///      * Moves the current position forward the given number of
  ///      * elements.
  ///      */
  ///     HRESULT Skip([in] ULONG celt);
  /// 
  ///     /*
  ///      * Sets the position of the enumerator to the beginning of the
  ///      * enumeration.
  ///      */
  ///     HRESULT Reset();
  /// 
  ///     /*
  ///      * Creates another enumerator with the same current position
  ///      * as this one.
  ///      */
  ///     HRESULT Clone([out] ICorDebugEnum **ppEnum);
  /// 
  ///     /*
  ///      * Gets the number of elements in the enumeration
  ///      */
  ///     HRESULT GetCount([out] ULONG *pcelt);
  /// };
  ///  </code></example>
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("CC7BCB01-8A68-11D2-983C-0000F808342D")]
  [ComImport]
  [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugEnum
  {
    /// <summary>
    /// Moves the current position forward the given number of elements.
    /// </summary>
    /// <param name="celt">the given number of elements</param>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall|MethodImplOptions.PreserveSig,MethodCodeType = MethodCodeType.Runtime)]
    Int32 Skip([In] UInt32 celt);

    /// <summary>
    /// Sets the position of the enumerator to the beginning of the enumeration.
    /// </summary>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall|MethodImplOptions.PreserveSig,MethodCodeType = MethodCodeType.Runtime)]
    Int32 Reset();

    /// <summary>
    /// Creates another enumerator with the same current position as this one.
    /// </summary>
    /// <param name="ppEnum">another enumerator</param>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall|MethodImplOptions.PreserveSig,MethodCodeType = MethodCodeType.Runtime)]
    Int32 Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);

    /// <summary>
    /// Gets the number of elements in the enumeration.
    /// </summary>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall|MethodImplOptions.PreserveSig,MethodCodeType = MethodCodeType.Runtime)]
    Int32 GetCount([Out] UInt32* pcelt);
  }
}