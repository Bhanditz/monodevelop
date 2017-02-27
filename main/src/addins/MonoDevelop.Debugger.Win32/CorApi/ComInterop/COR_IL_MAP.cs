using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        // Note that this structure is also defined in CorProf.idl - PROPOGATE CHANGES
        // BOTH WAYS, or this'll become a really insidious bug some day.
        typedef struct _COR_IL_MAP
        {
            ULONG32 oldOffset;      // Old IL offset relative to beginning of function
            ULONG32 newOffset;      // New IL offset relative to beginning of function
            BOOL    fAccurate;      // TRUE if mapping is known to be good, FALSE otherwise
        } COR_IL_MAP;
    */
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public struct COR_IL_MAP
    {
        /// <summary>
        /// Old IL offset relative to beginning of function
        /// </summary>
        public uint oldOffset;
        /// <summary>
        /// New IL offset relative to beginning of function
        /// </summary>
        public uint newOffset;
        /// <summary>
        /// TRUE if mapping is known to be good, FALSE otherwise
        /// </summary>
        public int fAccurate;
    }
}
