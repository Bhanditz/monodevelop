using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCAEA-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    public interface ICorDebugModuleBreakpoint : ICorDebugBreakpoint
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Activate ([In] int bActive);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void IsActive (out int pbActive);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetModule ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModule ppModule);
    }
}