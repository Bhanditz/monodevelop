using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("48B25ED8-5BAD-41bc-9CEE-CD62FABC74E9"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedConstant
    {
        void GetName(int cchName,
            out int pcchName,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name);
        
        void GetValue(out Object pValue);
         
        void GetSignature(int cSig,
            out int pcSig,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] byte[] sig);
    }
}