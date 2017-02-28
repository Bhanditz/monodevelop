﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("49E4A320-4A9B-4ECA-B105-229FB7D5009F")]
    [ComImport]
    public interface ICorDebugObjectValue2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetVirtualMethodAndType ([In] uint memberRef,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);
    }
}