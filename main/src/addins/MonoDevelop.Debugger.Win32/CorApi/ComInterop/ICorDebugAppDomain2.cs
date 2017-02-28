﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("096E81D5-ECDA-4202-83F5-C65980A9EF75")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICorDebugAppDomain2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetArrayOrPointerType ([In] CorElementType elementType, [In] uint nRank,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugType pTypeArg,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFunctionPointerType ([In] uint nTypeArgs,
            [MarshalAs (UnmanagedType.Interface), In] ref ICorDebugType ppTypeArgs,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);
    }
}