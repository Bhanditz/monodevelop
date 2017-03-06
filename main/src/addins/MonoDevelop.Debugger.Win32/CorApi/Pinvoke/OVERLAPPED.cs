using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.Pinvoke
{
    [NoReorder]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct OVERLAPPED
    {
        public UIntPtr Internal;

        public UIntPtr InternalHigh;

        /// <summary>
        /// NOTE(H): this is a union of two DWORDS and a pointer, so it would always be at least a QWORD, hope pointers never get above 64bit.
        /// </summary>
        public UInt64 PointerOrOffset;

        public void* hEvent;
    }
}