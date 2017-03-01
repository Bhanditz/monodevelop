using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
/// <summary>
/// 
/// </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///        typedef struct COR_DEBUG_STEP_RANGE
///        {
///            ULONG32 startOffset, endOffset;
///        } COR_DEBUG_STEP_RANGE;
/// </code></example>
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public struct COR_DEBUG_STEP_RANGE
    {
        public UInt32 startOffset;
        public UInt32 endOffset;
    }
}