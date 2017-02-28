using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugHeapValue::CreateHandle takes a handle flavor.
  ///        A strong handle will keep an object alive while a weak track resurrection
  ///        will not.
  /// </summary>
  /// <example><code>
  /// from: &lt;cordebug.idl&gt;
  ///        /* ICorDebugHeapValue::CreateHandle takes a handle flavor.
  ///        * A strong handle will keep an object alive while a weak track resurrection
  ///        * will not.
  ///        * /
  ///        typedef enum CorDebugHandleType
  ///        {
  ///            HANDLE_STRONG = 1,
  ///            HANDLE_WEAK_TRACK_RESURRECTION = 2
  ///        } CorDebugHandleType;
  /// </code></example>
  [SuppressMessage ("ReSharper", "InconsistentNaming")]
  public enum CorDebugHandleType
  {
    HANDLE_STRONG = 1,

    HANDLE_WEAK_TRACK_RESURRECTION = 2
  }
}