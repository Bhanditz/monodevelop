using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    [Guid ("3D6F5F64-7538-11D3-8D5B-00104B35E7EF")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public unsafe interface ICorDebugProcess : ICorDebugController
    {
        /// <inheritdoc cref="ICorDebugController.Stop"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Stop ([In] uint dwTimeoutIgnored);

        /// <inheritdoc cref="ICorDebugController.Continue"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Continue ([In] int fIsOutOfBand);

        /// <inheritdoc cref="ICorDebugController.IsRunning"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void IsRunning (out int pbRunning);

        /// <inheritdoc cref="ICorDebugController.HasQueuedCallbacks"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void HasQueuedCallbacks ([MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pThread,
            out int pbQueued);

        /// <inheritdoc cref="ICorDebugController.EnumerateThreads"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void EnumerateThreads ([MarshalAs (UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);

        /// <inheritdoc cref="ICorDebugController.SetAllThreadsDebugState"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void SetAllThreadsDebugState ([In] CorDebugThreadState state,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pExceptThisThread);

        /// <inheritdoc cref="ICorDebugController.Detach"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Detach ();

        /// <inheritdoc cref="ICorDebugController.Terminate"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Terminate ([In] uint exitCode);

        /// <inheritdoc cref="ICorDebugController.CanCommitChanges"/>
        [Obsolete]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void CanCommitChanges ([In] uint cSnapshots,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugEditAndContinueSnapshot[] pSnapshots,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);

        /// <inheritdoc cref="ICorDebugController.CommitChanges"/>
        [Obsolete]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void CommitChanges ([In] uint cSnapshots,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugEditAndContinueSnapshot[] pSnapshots,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetID (out uint pdwProcessId);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetHandle (void** phProcessHandle);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetThread ([In] uint dwThreadId, [MarshalAs (UnmanagedType.Interface)] out ICorDebugThread ppThread);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateObjects ([MarshalAs (UnmanagedType.Interface)] out ICorDebugObjectEnum ppObjects);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsTransitionStub ([In] ulong address, out int pbTransitionStub);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsOSSuspended ([In] uint threadID, out int pbSuspended);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetThreadContext ([In] uint threadID, [In] uint contextSize, [In, Out] byte* context);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetThreadContext ([In] uint threadID, [In] uint contextSize, [In, Out] byte* context);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ReadMemory ([In] ulong address, [In] uint size, [Out] byte* buffer,
            UIntPtr* read);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void WriteMemory ([In] ulong address, [In] uint size, [In] byte* buffer,
            UIntPtr* written);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ClearCurrentException ([In] uint threadID);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnableLogMessages ([In] int fOnOff);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ModifyLogSwitch ([In] UInt16* pLogSwitchName, [In] int lLevel);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateAppDomains ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAppDomainEnum ppAppDomains);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetObject ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppObject);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ThreadForFiberCookie ([In] uint fiberCookie,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugThread ppThread);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetHelperThreadID (out uint pThreadID);
    }
}