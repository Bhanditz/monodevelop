using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("86F012BF-FF15-4372-BD30-B6F11CAAE1DD")]
    [ComImport]
    public unsafe interface ICorDebugModule3
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateReaderForInMemorySymbols ([In] ref Guid riid, out IntPtr ppObj);
    }
}