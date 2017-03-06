using System;
using System.Runtime.InteropServices;

namespace CorApi.Pinvoke
{
    public static class Ole32Dll
    {
        [DllImport("ole32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = false, ExactSpelling = true)]
        public static extern int CoCreateInstance([In] ref Guid rclsid, [In] [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [In] uint dwClsContext, [In] ref Guid riid, [Out] [MarshalAs(UnmanagedType.Interface)] out object ppv);
    }
}