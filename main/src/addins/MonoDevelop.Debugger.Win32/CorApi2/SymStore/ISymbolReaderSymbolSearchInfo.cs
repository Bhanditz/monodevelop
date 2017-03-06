using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [
        ComVisible(false)
    ]
    public interface ISymbolReaderSymbolSearchInfo
    {
        int GetSymbolSearchInfoCount();
    
        ISymbolSearchInfo[] GetSymbolSearchInfo();
    }
}