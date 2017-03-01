using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
/// <summary>
/// 
/// </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///        typedef enum CorDebugExceptionCallbackType
///        {
///            DEBUG_EXCEPTION_FIRST_CHANCE = 1,        /* Fired when exception thrown * /
///            DEBUG_EXCEPTION_USER_FIRST_CHANCE = 2,   /* Fired when search reaches first user code * /
///            DEBUG_EXCEPTION_CATCH_HANDLER_FOUND = 3, /* Fired if &amp; when search finds a handler * /
///            DEBUG_EXCEPTION_UNHANDLED = 4            /* Fired if search doesnt find a handler * /
///        } CorDebugExceptionCallbackType;
/// </code></example>
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugExceptionCallbackType
    {
        /// <summary>
        /// Fired when exception thrown
        /// </summary>
        DEBUG_EXCEPTION_FIRST_CHANCE = 1,
        /// <summary>
        /// Fired when search reaches first user code
        /// </summary>
        DEBUG_EXCEPTION_USER_FIRST_CHANCE = 2,
        /// <summary>
        /// Fired if & when search finds a handler
        /// </summary>
        DEBUG_EXCEPTION_CATCH_HANDLER_FOUND = 3,
        /// <summary>
        /// Fired if search doesnt find a handler
        /// </summary>
        DEBUG_EXCEPTION_UNHANDLED = 4,
    }
}