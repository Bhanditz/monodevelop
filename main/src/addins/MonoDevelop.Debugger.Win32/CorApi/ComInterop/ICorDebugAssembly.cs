﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("DF59507C-D47A-459E-BCE2-6427EAC8FD06")]
    [ComImport]
    public unsafe interface ICorDebugAssembly
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetProcess ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAppDomain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAppDomain ppAppDomain);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateModules ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModuleEnum ppModules);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCodeBase ([In] uint cchName, out uint pcchName,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugAssembly szName);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetName ([In] uint cchName, out uint pcchName,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugAssembly szName);
    }
}