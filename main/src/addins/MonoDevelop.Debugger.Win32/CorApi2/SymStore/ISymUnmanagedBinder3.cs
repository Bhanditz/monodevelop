using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("28AD3D43-B601-4d26-8A1B-25F9165AF9D7"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedBinder3 : ISymUnmanagedBinder2
    {
        // ISymUnmanagedBinder methods (need to define the base interface methods also, per COM interop requirements)
        [PreserveSig]
        new int GetReaderForFile(IntPtr importer,
            [MarshalAs(UnmanagedType.LPWStr)] String filename,
            [MarshalAs(UnmanagedType.LPWStr)] String SearchPath,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader retVal);

        [PreserveSig]
        new int GetReaderFromStream(IntPtr importer,
            IStream stream,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader retVal);

        // ISymUnmanagedBinder2 methods (need to define the base interface methods also, per COM interop requirements)
        [PreserveSig]
        new int GetReaderForFile2(IntPtr importer,
            [MarshalAs(UnmanagedType.LPWStr)] String fileName,
            [MarshalAs(UnmanagedType.LPWStr)] String searchPath,
            int searchPolicy,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader pRetVal);

        // ISymUnmanagedBinder3 methods 
        [PreserveSig]
        int GetReaderFromCallback(IntPtr importer,
            [MarshalAs(UnmanagedType.LPWStr)] String fileName,
            [MarshalAs(UnmanagedType.LPWStr)] String searchPath,
            int searchPolicy,
            IntPtr callback,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader pRetVal);
    }
}