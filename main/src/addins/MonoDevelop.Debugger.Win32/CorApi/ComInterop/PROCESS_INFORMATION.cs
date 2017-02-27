using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [StructLayout(LayoutKind.Sequential, Pack = 8), ComVisible(false)]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    [SuppressMessage ("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage ("ReSharper", "ArrangeTypeMemberModifiers")]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public unsafe struct PROCESS_INFORMATION {
        public void* hProcess;
        public void* hThread;
        public UInt32 dwProcessId;
        public UInt32 dwThreadId;
    }
}