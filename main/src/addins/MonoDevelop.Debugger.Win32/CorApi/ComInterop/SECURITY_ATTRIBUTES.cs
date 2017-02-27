using System;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    public struct SECURITY_ATTRIBUTES
    {
        public uint nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }
}