using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("E502D2DD-8671-4338-8F2A-FC08229628C4"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedEncUpdate
    {

        void UpdateSymbolStore2(IStream stream,
            [MarshalAs(UnmanagedType.LPArray)] SymbolLineDelta[] iSymbolLineDeltas,
            int cDeltaLines);
    
        void GetLocalVariableCount(SymbolToken mdMethodToken,
            out int pcLocals);
    
        void GetLocalVariables(SymbolToken mdMethodToken,
            int cLocals,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] ISymUnmanagedVariable[] rgLocals,
            out int pceltFetched);
    }
}