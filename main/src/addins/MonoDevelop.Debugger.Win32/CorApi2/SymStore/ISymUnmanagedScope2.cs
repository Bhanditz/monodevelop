using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("AE932FBA-3FD8-4dba-8232-30A2309B02DB"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedScope2 : ISymUnmanagedScope
    {
        // ISymUnmanagedScope methods (need to define the base interface methods also, per COM interop requirements)
        new void GetMethod([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod pRetVal);

        new void GetParent([MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedScope pRetVal);

        new void GetChildren(int cChildren,
            out int pcChildren,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedScope[] children);

        new void GetStartOffset(out int pRetVal);

        new void GetEndOffset(out int pRetVal);

        new void GetLocalCount(out int pRetVal);

        new void GetLocals(int cLocals,
            out int pcLocals,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedVariable[] locals);

        new void GetNamespaces(int cNameSpaces,
            out int pcNameSpaces,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ISymUnmanagedNamespace[] namespaces);

        // ISymUnmanagedScope2 methods
        void GetConstantCount(out int pRetVal);

        void GetConstants(int cConstants,
            out int pcConstants,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedConstant[] constants);
    }
}