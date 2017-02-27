using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugStepReason
        {
            STEP_NORMAL,
            STEP_RETURN,
            STEP_CALL,
            STEP_EXCEPTION_FILTER,
            STEP_EXCEPTION_HANDLER,
            STEP_INTERCEPT,
            STEP_EXIT
        } CorDebugStepReason;
    */
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