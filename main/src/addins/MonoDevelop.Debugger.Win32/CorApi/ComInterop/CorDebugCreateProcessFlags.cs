using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugCreateProcessFlags
        {
            DEBUG_NO_SPECIAL_OPTIONS        = 0x0000
        } CorDebugCreateProcessFlags;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugCreateProcessFlags
    {
        DEBUG_NO_SPECIAL_OPTIONS,
    }
}