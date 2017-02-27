using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef struct COR_DEBUG_STEP_RANGE
        {
            ULONG32 startOffset, endOffset;
        } COR_DEBUG_STEP_RANGE;
    */
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public struct COR_DEBUG_STEP_RANGE
    {
        public uint startOffset;
        public uint endOffset;
    }
}