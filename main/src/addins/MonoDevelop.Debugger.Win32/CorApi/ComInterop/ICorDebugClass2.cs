﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("B008EA8D-7AB1-43F7-BB20-FBB5A04038AE")]
    [ComImport]
    public interface ICorDebugClass2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetParameterizedType ([In] CorElementType elementType, [In] uint nTypeArgs,
            [MarshalAs (UnmanagedType.Interface), In] ref ICorDebugType ppTypeArgs,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetJMCStatus ([In] int bIsJustMyCode);
    }
}