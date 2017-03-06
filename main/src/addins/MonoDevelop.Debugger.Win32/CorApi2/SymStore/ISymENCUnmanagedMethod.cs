using System.Runtime.InteropServices;
using System.Text;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("85E891DA-A631-4c76-ACA2-A44A39C46B8C"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymENCUnmanagedMethod
    {
        void GetFileNameFromOffset(int dwOffset,
            int cchName,
            out int pcchName,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name);
    
        void GetLineFromOffset(int dwOffset,
            out int pline,
            out int pcolumn,
            out int pendLine,
            out int pendColumn,
            out int pdwStartOffset);
    }
}