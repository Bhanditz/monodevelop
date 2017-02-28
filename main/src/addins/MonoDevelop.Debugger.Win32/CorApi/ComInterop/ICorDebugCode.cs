﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCAF4-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    public unsafe interface ICorDebugCode
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsIL (out int pbIL);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAddress (out ulong pStart);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSize (out uint pcBytes);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateBreakpoint ([In] uint offset,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunctionBreakpoint ppBreakpoint);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCode ([In] uint startOffset, [In] uint endOffset, [In] uint cBufferAlloc,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugCode buffer, out uint pcBufferSize);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetVersionNumber (out uint nVersion);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetILToNativeMapping ([In] uint cMap, out uint pcMap,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugCode map);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEnCRemapSequencePoints ([In] uint cMap, out uint pcMap,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugCode offsets);
    }
}