using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("5D88A994-6C30-479B-890F-BCEF88B129A5")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public unsafe interface ICorDebugILFrame2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void RemapFunction ([In] uint newILOffset);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateTypeParameters ([MarshalAs (UnmanagedType.Interface)] out ICorDebugTypeEnum ppTyParEnum);
    }
}