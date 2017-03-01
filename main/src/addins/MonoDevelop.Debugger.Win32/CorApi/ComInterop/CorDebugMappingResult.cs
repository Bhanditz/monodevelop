using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
/// <summary>
/// 
/// </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///        typedef enum CorDebugMappingResult
///        {
///            MAPPING_PROLOG              = 0x1,
///            MAPPING_EPILOG              = 0x2,
///            MAPPING_NO_INFO             = 0x4,
///            MAPPING_UNMAPPED_ADDRESS    = 0x8,
///            MAPPING_EXACT               = 0x10,
///            MAPPING_APPROXIMATE         = 0x20,
///        } CorDebugMappingResult;
/// </code></example>
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugMappingResult
    {
        MAPPING_PROLOG              = 0x1,
        MAPPING_EPILOG              = 0x2,
        MAPPING_NO_INFO             = 0x4,
        MAPPING_UNMAPPED_ADDRESS    = 0x8,
        MAPPING_EXACT               = 0x10,
        MAPPING_APPROXIMATE         = 0x20,
    }
}