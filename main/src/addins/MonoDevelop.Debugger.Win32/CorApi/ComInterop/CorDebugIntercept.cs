using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// from: &lt;cordebug.idl&gt;
  ///        typedef enum CorDebugIntercept
  ///        {
  ///            INTERCEPT_NONE                = 0x0 ,
  ///            INTERCEPT_CLASS_INIT          = 0x01,
  ///            INTERCEPT_EXCEPTION_FILTER    = 0x02,
  ///            INTERCEPT_SECURITY            = 0x04,
  ///            INTERCEPT_CONTEXT_POLICY      = 0x08,
  ///            INTERCEPT_INTERCEPTION        = 0x10,
  ///            INTERCEPT_ALL                 = 0xffff
  ///        } CorDebugIntercept;
  /// </code></example>
  [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugIntercept
    {
       INTERCEPT_NONE                = 0x0 ,
       INTERCEPT_CLASS_INIT          = 0x01,
       INTERCEPT_EXCEPTION_FILTER    = 0x02,
       INTERCEPT_SECURITY            = 0x04,
       INTERCEPT_CONTEXT_POLICY      = 0x08,
       INTERCEPT_INTERCEPTION        = 0x10,
       INTERCEPT_ALL                 = 0xffff
    }
}