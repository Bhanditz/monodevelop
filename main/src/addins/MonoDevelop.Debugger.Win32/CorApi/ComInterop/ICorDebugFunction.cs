﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("CC7BCAF3-8A68-11D2-983C-0000F808342D")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public unsafe interface ICorDebugFunction
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetModule ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModule ppModule);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetClass ([MarshalAs (UnmanagedType.Interface)] out ICorDebugClass ppClass);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetToken (out uint pMethodDef);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetILCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetNativeCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunctionBreakpoint ppBreakpoint);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetLocalVarSigToken (out uint pmdSig);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCurrentVersionNumber (out uint pnCurrentVersion);
    }
}