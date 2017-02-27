using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [StructLayout (LayoutKind.Sequential, Pack = 8)]
    public struct ULARGE_INTEGER
    {
        public ulong QuadPart;
    }
}