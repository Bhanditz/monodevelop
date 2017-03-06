using System;
using System.Runtime.InteropServices;

namespace CorApi2.SymStore
{
    [Guid("809c652e-7396-11d2-9771-00a0c9b4d50c"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    internal interface IMetaDataDispenserPrivate
    {
        // We need to be able to call OpenScope, which is the 2nd vtable slot.
        // Thus we need this one placeholder here to occupy the first slot..
        void DefineScope_Placeholder();

        //STDMETHOD(OpenScope)(                 // Return code.
        //  LPCWSTR     szScope,                // [in] The scope to open.
        //  DWORD       dwOpenFlags,            // [in] Open mode flags.
        //  REFIID      riid,                   // [in] The interface desired.
        //  IUnknown    **ppIUnk) PURE;         // [out] Return interface on success.
        void OpenScope([In, MarshalAs(UnmanagedType.LPWStr)] String szScope, [In] Int32 dwOpenFlags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out Object punk);

        // There are more methods in this interface, but we don't need them.
    }
}