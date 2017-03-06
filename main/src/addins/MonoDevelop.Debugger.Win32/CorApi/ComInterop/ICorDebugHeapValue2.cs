using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugHeapValue2
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugHeapValue2
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(E3AC4D6C-9CB7-43e6-96CC-B21540E5083C),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugHeapValue2 : IUnknown
  /// {
  /// 
  ///     /*
  ///       * Creates a handle of the given type for this heap value.
  ///       *
  ///       */
  ///     HRESULT CreateHandle([in] CorDebugHandleType type, [out] ICorDebugHandleValue ** ppHandle);
  /// 
  /// };
  ///  </code></example>
  [Guid ("E3AC4D6C-9CB7-43E6-96CC-B21540E5083C")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface ICorDebugHeapValue2
  {
    /// <summary>
    /// Creates a handle of the given type for this heap value.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ppHandle"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateHandle ([In] CorDebugHandleType type, [MarshalAs (UnmanagedType.Interface)] out ICorDebugHandleValue ppHandle);
  }
}