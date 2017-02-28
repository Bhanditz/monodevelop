﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("CC7BCAF9-8A68-11D2-983C-0000F808342D")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICorDebugReferenceValue : ICorDebugValue
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void GetType (out CorElementType elementType);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void GetSize (out uint pSize);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void GetAddress (out ulong pAddress);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsNull (out int pbNull);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetValue (out ulong pValue);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetValue ([In] ulong value);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Dereference ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DereferenceStrong ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);
    }
}