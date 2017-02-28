﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("AD1B3588-0EF0-4744-A496-AA09A9F80371")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public unsafe interface ICorDebugProcess2
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetThreadForTaskID ([In] ulong taskid,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugThread2 ppThread);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetVersion (out COR_VERSION version);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetUnmanagedBreakpoint ([In] ulong address, [In] uint bufsize,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugProcess2 buffer, out uint bufLen);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ClearUnmanagedBreakpoint ([In] ulong address);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetDesiredNGENCompilerFlags ([In] uint pdwFlags);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDesiredNGENCompilerFlags (out uint pdwFlags);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetReferenceValueFromGCHandle ([ComAliasName ("CORDBLib.UINT_PTR"), In] uint handle,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugReferenceValue pOutValue);
    }
}