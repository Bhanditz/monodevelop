using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using CorApi2.debug;

namespace CorApi2.Metahost
{
    [SecurityCritical]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("D332DB9E-B9B3-4125-8207-A14884F53216")]
    internal interface IClrMetaHost
    {
        [return: MarshalAs (UnmanagedType.Interface)]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        object GetRuntime (
            [In, MarshalAs (UnmanagedType.LPWStr)] string version,
            [In, MarshalAs (UnmanagedType.LPStruct)] Guid interfaceId);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetVersionFromFile (
            [In, MarshalAs (UnmanagedType.LPWStr)] string filePath,
            [Out, MarshalAs (UnmanagedType.LPWStr)] StringBuilder buffer,
            [In, Out, MarshalAs (UnmanagedType.U4)] ref uint bufferLength);

        [return: MarshalAs (UnmanagedType.Interface)]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        IEnumUnknown EnumerateInstalledRuntimes ();

        [return: MarshalAs (UnmanagedType.Interface)]
        //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        IEnumUnknown EnumerateLoadedRuntimes ([In] ProcessSafeHandle processHandle);

        [PreserveSig, MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Reserved01 ([In] IntPtr reserved1);
    }
}