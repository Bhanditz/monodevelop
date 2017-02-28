﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("DBA2D8C1-E5C5-4069-8C13-10A7C6ABF43D")]
    [ComImport]
    public interface ICorDebugModule
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetProcess ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetBaseAddress (out ulong pAddress);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAssembly ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAssembly ppAssembly);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetName ([In] uint cchName, out uint pcchName,
            [MarshalAs (UnmanagedType.Interface), Out] ICorDebugModule szName);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnableJITDebugging ([In] int bTrackJITInfo, [In] int bAllowJitOpts);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnableClassLoadCallbacks ([In] int bClassLoadCallbacks);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFunctionFromToken ([In] uint methodDef,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFunctionFromRVA ([In] ulong rva,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetClassFromToken ([In] uint typeDef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugClass ppClass);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModuleBreakpoint ppBreakpoint);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEditAndContinueSnapshot (
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugEditAndContinueSnapshot ppEditAndContinueSnapshot);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetMetaDataInterface ([In] ref Guid riid, [MarshalAs (UnmanagedType.IUnknown)] out object ppObj);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetToken (out uint pToken);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsDynamic (out int pDynamic);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetGlobalVariableValue ([In] uint fieldDef,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSize (out uint pcBytes);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsInMemory (out int pInMemory);
    }
}