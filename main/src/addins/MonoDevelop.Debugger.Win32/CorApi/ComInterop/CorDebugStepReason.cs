using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
/// <summary>
/// 
///  </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///        typedef enum CorDebugStepReason
///        {
///            STEP_NORMAL,
///            STEP_RETURN,
///            STEP_CALL,
///            STEP_EXCEPTION_FILTER,
///            STEP_EXCEPTION_HANDLER,
///            STEP_INTERCEPT,
///            STEP_EXIT
///        } CorDebugStepReason;
/// </code></example>
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugStepReason
    {
        STEP_NORMAL,
        STEP_RETURN,
        STEP_CALL,
        STEP_EXCEPTION_FILTER,
        STEP_EXCEPTION_HANDLER,
        STEP_INTERCEPT,
        STEP_EXIT,
    }
}