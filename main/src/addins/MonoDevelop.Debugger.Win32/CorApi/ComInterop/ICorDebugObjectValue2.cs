using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// [
  ///    object,
  ///    local,
  ///    uuid(49E4A320-4A9B-4eca-B105-229FB7D5009F),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugObjectValue2 : IUnknown
  ///{
  ///    /*
  ///     * GetVirtualMethodForType returns the most derived function
  ///     * for the given ref on this object.
  ///     *
  ///     * Note: not yet implemented.
  ///     */
  ///
  ///    HRESULT GetVirtualMethodAndType([in] mdMemberRef memberRef,
  ///                                    [out] ICorDebugFunction **ppFunction,
  ///                                    [out] ICorDebugType **ppType);
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("49E4A320-4A9B-4ECA-B105-229FB7D5009F")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugObjectValue2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetVirtualMethodAndType ([In][ComAliasName("mdMemberRef")] UInt32 memberRef,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);
    }
}