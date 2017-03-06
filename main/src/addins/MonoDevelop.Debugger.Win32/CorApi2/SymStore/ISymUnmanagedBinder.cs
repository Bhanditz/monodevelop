using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
    [
        ComImport,
        Guid("AA544d42-28CB-11d3-bd22-0000f80849bd"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)
    ]
    internal interface ISymUnmanagedBinder
    {
        // These methods will often return error HRs in common cases.
        // If there are no symbols for the given target, a failing hr is returned.
        // This is pretty common.
        //
        // Using PreserveSig and manually handling error cases provides a big performance win.
        // Far fewer exceptions will be thrown and caught.
        // Exceptions should be reserved for truely "exceptional" cases.
        [PreserveSig]
        int GetReaderForFile(IntPtr importer,
            [MarshalAs(UnmanagedType.LPWStr)] String filename,
            [MarshalAs(UnmanagedType.LPWStr)] String SearchPath,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader retVal);

        [PreserveSig]
        int GetReaderFromStream(IntPtr importer,
            IStream stream,
            [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedReader retVal);
    }
}