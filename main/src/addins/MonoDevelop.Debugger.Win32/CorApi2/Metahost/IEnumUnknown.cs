using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace CorApi2.Metahost
{
    [ComImport]
    [SecurityCritical]
    [Guid ("00000100-0000-0000-C000-000000000046")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumUnknown
    {
        [PreserveSig]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Next (
            [In, MarshalAs (UnmanagedType.U4)] int elementArrayLength,
            [Out, MarshalAs (UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown, SizeParamIndex = 0)] object[] elementArray,
            [MarshalAs (UnmanagedType.U4)] out int fetchedElementCount);

        [PreserveSig]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Skip ([In, MarshalAs (UnmanagedType.U4)] int count);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Reset ();

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Clone ([MarshalAs (UnmanagedType.Interface)] out IEnumUnknown enumerator);
    }
}