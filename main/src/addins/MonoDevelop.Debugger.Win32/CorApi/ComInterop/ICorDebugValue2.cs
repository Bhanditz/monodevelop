using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("5E0B54E7-D88A-4626-9420-A691E0A78B49")]
    [ComImport]
    public interface ICorDebugValue2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetExactType ([MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);
    }
}