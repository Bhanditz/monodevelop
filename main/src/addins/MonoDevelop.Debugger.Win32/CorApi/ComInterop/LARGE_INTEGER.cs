using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [StructLayout (LayoutKind.Sequential, Pack = 8)]
    public struct LARGE_INTEGER
    {
        public long QuadPart;
    }
}