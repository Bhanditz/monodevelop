using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef struct _CodeChunkInfo
        {
            CORDB_ADDRESS startAddr;
            ULONG32 length;
        } CodeChunkInfo;
    */
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CodeChunkInfo
    {
        public ulong startAddr;
        public uint length;
    }
}