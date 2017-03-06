using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("B62B923C-B500-3158-A543-24F307A8B7E1"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    interface ISymUnmanagedMethod
    {
        void GetToken(out SymbolToken pToken);
        void GetSequencePointCount(out int retVal);
        void GetRootScope([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope retVal);
        void GetScopeFromOffset(int offset, [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope retVal);
        void GetOffset(ISymUnmanagedDocument document,
            int line,
            int column,
            out int retVal);
        void GetRanges(ISymUnmanagedDocument document,
            int line,
            int column,
            int cRanges,
            out int pcRanges,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] ranges);
        void GetParameters(int cParams,
            out int pcParams,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedVariable[] parms);
        void GetNamespace([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedNamespace retVal);
        void GetSourceStartEnd(ISymUnmanagedDocument[] docs,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] lines,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] columns,
            out Boolean retVal);
        void GetSequencePoints(int cPoints,
            out int pcPoints,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] offsets,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedDocument[] documents,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] lines,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] columns,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] endLines,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] endColumns);
    }
}