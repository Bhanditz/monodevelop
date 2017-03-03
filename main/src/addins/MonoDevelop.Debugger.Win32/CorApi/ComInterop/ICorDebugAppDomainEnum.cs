using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  * AppDomainEnum interface
  ///  * ------------------------------------------------------------------------- */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(63ca1b24-4359-4883-bd57-13f815f58744),
  ///     pointer_default(unique)
  /// ]
  /// 
  /// interface ICorDebugAppDomainEnum : ICorDebugEnum
  /// {
  ///     /*
  ///      * Gets the next "celt" app domains in the enumeration
  ///      */
  ///     HRESULT Next([in] ULONG celt,
  ///                  [out, size_is(celt), length_is(*pceltFetched)]
  ///                     ICorDebugAppDomain *values[],
  ///                  [out] ULONG *pceltFetched);
  /// 
  /// }; </code></example>
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("63CA1B24-4359-4883-BD57-13F815F58744")]
  [ComImport]
  [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugAppDomainEnum : ICorDebugEnum
  {
    /// <summary>
    /// Moves the current position forward the given number of elements.
    /// </summary>
    /// <param name="celt">the given number of elements</param>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
    new Int32 Skip([In] UInt32 celt);

    /// <summary>
    /// Sets the position of the enumerator to the beginning of the enumeration.
    /// </summary>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
    new Int32 Reset();

    /// <summary>
    /// Creates another enumerator with the same current position as this one.
    /// </summary>
    /// <param name="ppEnum">another enumerator</param>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
    new Int32 Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);

    /// <summary>
    /// Gets the number of elements in the enumeration.
    /// </summary>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
    new Int32 GetCount([Out] UInt32* pcelt);

    /// <summary>
    /// Gets the next "celt" app domains in the enumeration
    /// </summary>
    /// <param name="celt"></param>
    /// <param name="values"></param>
    /// <param name="pceltFetched"></param>
    [MustUseReturnValue("HResult")]
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
    Int32 Next([In] UInt32 celt, void** values, [Out] UInt32* pceltFetched);
  }
}