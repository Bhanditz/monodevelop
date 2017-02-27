using System;
using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugMDAFlags
        {
            // If this flag is high, then the thread may have slipped since the MDA was fired.
            MDA_FLAG_SLIP = 0x2
        } CorDebugMDAFlags;
    */
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