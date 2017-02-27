using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef struct _COR_VERSION
        {
            DWORD dwMajor;
            DWORD dwMinor;
            DWORD dwBuild;
            DWORD dwSubBuild;
        } COR_VERSION;
    */
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public struct COR_VERSION
    {
        public uint dwMajor;
        public uint dwMinor;
        public uint dwBuild;
        public uint dwSubBuild;
    }
}