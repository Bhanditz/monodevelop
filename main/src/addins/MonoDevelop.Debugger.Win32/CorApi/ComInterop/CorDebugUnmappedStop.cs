using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{

  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// from: &lt;cordebug.idl&gt;
  ///        typedef enum CorDebugUnmappedStop
  ///        {
  ///            STOP_NONE               = 0x0,
  ///            STOP_PROLOG             = 0x01,
  ///            STOP_EPILOG             = 0x02,
  ///            STOP_NO_MAPPING_INFO    = 0x04,
  ///            STOP_OTHER_UNMAPPED     = 0x08,
  ///            STOP_UNMANAGED          = 0x10,
  ///
  ///            STOP_ALL                = 0xffff,
  ///
  ///        } CorDebugUnmappedStop;
  /// </code></example>
  [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugUnmappedStop
    {
        STOP_NONE               = 0x0,
        STOP_PROLOG             = 0x01,
        STOP_EPILOG             = 0x02,
        STOP_NO_MAPPING_INFO    = 0x04,
        STOP_OTHER_UNMAPPED     = 0x08,
        STOP_UNMANAGED          = 0x10,
    
        STOP_ALL                = 0xffff,
    }
}