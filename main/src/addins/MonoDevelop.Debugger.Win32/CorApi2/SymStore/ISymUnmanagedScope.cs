using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("68005D0F-B8E0-3B01-84D5-A11A94154942"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedScope
    {
        void GetMethod([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod pRetVal);
    
        void GetParent([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope pRetVal);
    
        void GetChildren(int cChildren,
            out int pcChildren,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedScope[] children);
    
        void GetStartOffset(out int pRetVal);
    
        void GetEndOffset(out int pRetVal);
    
        void GetLocalCount(out int pRetVal);
    
        void GetLocals(int cLocals,
            out int pcLocals,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedVariable[] locals);
    
        void GetNamespaces(int cNameSpaces,
            out int pcNameSpaces,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedNamespace[] namespaces);
    };
}