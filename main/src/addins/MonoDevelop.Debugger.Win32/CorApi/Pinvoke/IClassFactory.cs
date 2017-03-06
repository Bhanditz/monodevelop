using System;
using System.Runtime.InteropServices;

namespace CorApi.Pinvoke
{
    [ComImport]
    [ComVisible(false)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000001-0000-0000-C000-000000000046")]
    public interface IClassFactory
    {
        [PreserveSig] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] [Out] out object ppvObject);
    
        [PreserveSig] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);
    }
}