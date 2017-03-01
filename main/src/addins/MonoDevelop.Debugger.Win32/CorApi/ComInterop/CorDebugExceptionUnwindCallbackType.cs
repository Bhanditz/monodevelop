using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
/// <summary>
/// 
/// </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///         typedef enum CorDebugExceptionUnwindCallbackType
///         {
///             DEBUG_EXCEPTION_UNWIND_BEGIN = 1, /* Fired at the beginning of the unwind * /
///             DEBUG_EXCEPTION_INTERCEPTED = 2   /* Fired after an exception has been intercepted * /
///         } CorDebugExceptionUnwindCallbackType;
/// </code></example>
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugExceptionUnwindCallbackType
    {
        /// <summary>
        /// Fired at the beginning of the unwind
        /// </summary>
        DEBUG_EXCEPTION_UNWIND_BEGIN = 1,
        /// <summary>
        /// Fired after an exception has been intercepted
        /// </summary>
        DEBUG_EXCEPTION_INTERCEPTED = 2,
    }
}