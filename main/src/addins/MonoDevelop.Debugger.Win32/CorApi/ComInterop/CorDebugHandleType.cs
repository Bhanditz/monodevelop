using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        /* ICorDebugHeapValue::CreateHandle takes a handle flavor.
        * A strong handle will keep an object alive while a weak track resurrection
        * will not.
        * /
        typedef enum CorDebugHandleType
        {
            HANDLE_STRONG = 1,
            HANDLE_WEAK_TRACK_RESURRECTION = 2
        } CorDebugHandleType;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugHandleType
    {
        HANDLE_STRONG = 1,
        HANDLE_WEAK_TRACK_RESURRECTION = 2,
    }
}