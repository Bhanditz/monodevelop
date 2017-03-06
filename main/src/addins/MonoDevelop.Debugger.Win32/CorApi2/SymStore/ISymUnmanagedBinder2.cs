using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("ACCEE350-89AF-4ccb-8B40-1C2C4C6F9434"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedBinder2 : ISymUnmanagedBinder
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

        // ISymUnmanagedBinder2 methods 
        [PreserveSig]
        int GetReaderForFile2(IntPtr importer,
            [MarshalAs(UnmanagedType.LPWStr)] String fileName,
            [MarshalAs(UnmanagedType.LPWStr)] String searchPath,
            int searchPolicy,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader pRetVal);
    }
}