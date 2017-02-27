using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugUnmappedStop
        {
            STOP_NONE               = 0x0,
            STOP_PROLOG             = 0x01,
            STOP_EPILOG             = 0x02,
            STOP_NO_MAPPING_INFO    = 0x04,
            STOP_OTHER_UNMAPPED     = 0x08,
            STOP_UNMANAGED          = 0x10,

            STOP_ALL                = 0xffff,

        } CorDebugUnmappedStop;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugUnmappedStop
    {
        STOP_NONE = 0,
        STOP_PROLOG = 1,
        STOP_EPILOG = 2,
        STOP_NO_MAPPING_INFO = 4,
        STOP_OTHER_UNMAPPED = 8,
        STOP_UNMANAGED = 16,
        STOP_ALL = 65535,
    }
}