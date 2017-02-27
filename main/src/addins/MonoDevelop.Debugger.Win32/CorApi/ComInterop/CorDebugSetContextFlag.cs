using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugSetContextFlag
        {
            SET_CONTEXT_FLAG_ACTIVE_FRAME = 0x1,
            SET_CONTEXT_FLAG_UNWIND_FRAME = 0x2,
        } CorDebugSetContextFlag;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugSetContextFlag
    {
        SET_CONTEXT_FLAG_ACTIVE_FRAME = 1,
        SET_CONTEXT_FLAG_UNWIND_FRAME = 2,
    }
}