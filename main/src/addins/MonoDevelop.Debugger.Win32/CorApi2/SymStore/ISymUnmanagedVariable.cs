using System.Runtime.InteropServices;
using System.Text;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("9F60EEBE-2D9A-3F7C-BF58-80BC991C60BB"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedVariable 
    {
        void GetName(int cchName,
            out int pcchName,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName);

        void GetAttributes(out int pRetVal);

        void GetSignature(int cSig,
            out int pcSig,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] byte[] sig);

        void GetAddressKind(out int  pRetVal);

        void GetAddressField1(out int  pRetVal);

        void GetAddressField2(out int  pRetVal);

        void GetAddressField3(out int  pRetVal);

        void GetStartOffset(out int  pRetVal);

        void GetEndOffset(out int  pRetVal);
    }
}