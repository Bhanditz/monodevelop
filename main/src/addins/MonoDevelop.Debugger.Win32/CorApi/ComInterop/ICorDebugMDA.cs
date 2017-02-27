using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("CC726F2F-1DB7-459B-B0EC-05F01D841B42")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public unsafe interface ICorDebugMDA
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetName ([In] uint cchName, out uint pcchName,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugMDA szName);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDescription ([In] uint cchName, out uint pcchName,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugMDA szName);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetXML ([In] uint cchName, out uint pcchName,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugMDA szName);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFlags ([In] CorDebugMDAFlags* pFlags);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetOSThreadId (out uint pOsTid);
    }
}