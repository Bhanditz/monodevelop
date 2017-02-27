using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8), ComVisible(false)]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    [SuppressMessage ("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage ("ReSharper", "ArrangeTypeMemberModifiers")]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public unsafe struct STARTUPINFOW
    {
        public UInt32   cb;
        public UInt16*  lpReserved;
        public UInt16*  lpDesktop;
        public UInt16*  lpTitle;
        public UInt32   dwX;
        public UInt32   dwY;
        public UInt32   dwXSize;
        public UInt32   dwYSize;
        public UInt32   dwXCountChars;
        public UInt32   dwYCountChars;
        public UInt32   dwFillAttribute;
        public UInt32   dwFlags;
        public UInt16    wShowWindow;
        public UInt16    cbReserved2;
        public Byte*  lpReserved2;
        public void*  hStdInput;
        public void*  hStdOutput;
        public void*  hStdError;
    }
}