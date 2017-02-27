using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugMappingResult
        {
            MAPPING_PROLOG              = 0x1,
            MAPPING_EPILOG              = 0x2,
            MAPPING_NO_INFO             = 0x4,
            MAPPING_UNMAPPED_ADDRESS    = 0x8,
            MAPPING_EXACT               = 0x10,
            MAPPING_APPROXIMATE         = 0x20,
        } CorDebugMappingResult;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugMappingResult
    {
        MAPPING_PROLOG = 1,
        MAPPING_EPILOG = 2,
        MAPPING_NO_INFO = 4,
        MAPPING_UNMAPPED_ADDRESS = 8,
        MAPPING_EXACT = 16,
        MAPPING_APPROXIMATE = 32,
    }
}