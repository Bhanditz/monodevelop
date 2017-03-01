using System;
using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
/// <summary>
/// 
/// </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///        typedef enum CorDebugMDAFlags
///        {
///            // If this flag is high, then the thread may have slipped since the MDA was fired.
///            MDA_FLAG_SLIP = 0x2
///        } CorDebugMDAFlags;
/// </code></example>
    [CLSCompliant(true)]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugMDAFlags
    {
        /// <summary>
        /// If this flag is high, then the thread may have slipped since the MDA was fired.
        /// </summary>
        MDA_FLAG_SLIP = 2,
    }
}