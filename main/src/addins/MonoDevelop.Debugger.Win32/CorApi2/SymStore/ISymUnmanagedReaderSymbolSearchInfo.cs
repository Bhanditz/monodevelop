using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("20D9645D-03CD-4e34-9C11-9848A5B084F1"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedReaderSymbolSearchInfo
    {
        void GetSymbolSearchInfoCount(out int pcSearchInfo);
    
        void GetSymbolSearchInfo(int cSearchInfo,
            out int pcSearchInfo,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedSymbolSearchInfo[] searchInfo);
    }
}