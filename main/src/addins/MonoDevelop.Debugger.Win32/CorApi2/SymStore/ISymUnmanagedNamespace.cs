using System.Runtime.InteropServices;
using System.Text;

namespace CorApi2.SymStore
{
    /// <include file='doc\ISymNamespace.uex' path='docs/doc[@for="ISymbolNamespace"]/*' />
    [
        ComImport,
        Guid("0DFF7289-54F8-11d3-BD28-0000F80849BD"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedNamespace
    {
        void GetName(int cchName,
            out int pcchName,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName);
    
        void GetNamespaces(int cNameSpaces,
            out int pcNameSpaces,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedNamespace[] namespaces);
    
        void GetVariables(int cVars,
            out int pcVars,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedVariable[] pVars);
    }
}