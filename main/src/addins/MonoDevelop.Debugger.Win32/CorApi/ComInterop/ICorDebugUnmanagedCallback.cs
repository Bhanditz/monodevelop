using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("5263E909-8CB5-11D3-BD2F-0000F80849BD")]
    [ComImport]
    public interface ICorDebugUnmanagedCallback
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DebugEvent ([ComAliasName ("CORDBLib.ULONG_PTR"), In] uint pDebugEvent, [In] int fOutOfBand);
    }
}