using System.Runtime.InteropServices;
using System.Text;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("F8B3534A-A46B-4980-B520-BEC4ACEABA8F"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedSymbolSearchInfo 
    {
        void GetSearchPathLength(out int pcchPath);

        void GetSearchPath(int cchPath,
            out int pcchPath,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szPath);

        void GetHRESULT(out int hr);
    }
}