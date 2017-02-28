using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("5F696509-452F-4436-A3FE-4D11FE7E2347")]
    [ComImport]
    public unsafe interface ICorDebugCode2
    {
        
        void GetCodeChunks([In] uint cbufSize, [Out] out uint pcnumChunks, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] CodeChunkInfo[] chunks);
        
        void GetCompilerFlags([Out] out uint pdwFlags);
    }
}