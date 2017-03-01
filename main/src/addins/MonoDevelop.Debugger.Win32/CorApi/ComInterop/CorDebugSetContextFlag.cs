using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// 
  /// </summary>
  /// <example><code>
  /// from: &lt;cordebug.idl&gt;
  ///        typedef enum CorDebugSetContextFlag
  ///        {
  ///            SET_CONTEXT_FLAG_ACTIVE_FRAME = 0x1,
  ///            SET_CONTEXT_FLAG_UNWIND_FRAME = 0x2,
  ///        } CorDebugSetContextFlag;
  /// </code></example>
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugSetContextFlag
    {
        SET_CONTEXT_FLAG_ACTIVE_FRAME = 0x1,
        SET_CONTEXT_FLAG_UNWIND_FRAME = 0x2,
    }
}