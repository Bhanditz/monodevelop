using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugProcess represents a process running some managed code. 
  /// </summary>
  /// <example><code>
  /// /*
  /// * ICorDebugProcess represents a process running some managed code.
  /// */
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(3d6f5f64-7538-11d3-8d5b-00104b35e7ef),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugProcess : ICorDebugController
  ///{
  ///    /*
  ///     * GetID returns the OS ID of the process.
  ///     */
  ///
  ///    HRESULT GetID([out] DWORD *pdwProcessId);
  ///
  ///    /*
  ///     * GetHandle returns a handle to the process. This handle is owned
  ///     * by the debugging API; the debugger should duplicate it before
  ///     * using it.
  ///     */
  ///
  ///    HRESULT GetHandle([out] HPROCESS *phProcessHandle);
  ///
  ///    /*
  ///     * GetThread returns the ICorDebugThread with the given OS Id.
  ///     *
  ///     * Note that eventually there will not be a one to one correspondence
  ///     * between OS threads and runtime threads, so this entry point will
  ///     * go away.
  ///     */
  ///
  ///    HRESULT GetThread([in] DWORD dwThreadId, [out] ICorDebugThread **ppThread);
  ///
  ///    /*
  ///     * NOT YET IMPLEMENTED
  ///     */
  ///
  ///    HRESULT EnumerateObjects([out] ICorDebugObjectEnum **ppObjects);
  ///
  ///    /*
  ///     * IsTransitionStub tests whether an address is inside of a transition stub
  ///     * which will cause a transition to managed code.  This can be used by
  ///     * unmanaged stepping code to decide when to return stepping control to
  ///     * the managed stepper.
  ///     *
  ///     * Note that, tentatively, these stubs may also be able to be identified
  ///     * ahead of time by looking at information in the PE file.
  ///     *
  ///     */
  ///
  ///    HRESULT IsTransitionStub([in] CORDB_ADDRESS address,
  ///                             [out] BOOL *pbTransitionStub);
  ///
  ///
  ///    /*
  ///     * IsOSSuspended returns whether or not the thread has been
  ///     * suspended as part of the debugger logic of stopping the process.
  ///     * (that is, it has had its Win32 suspend count incremented by
  ///     * one.)  The debugger UI may want to take this into account if
  ///     * it shows the user the OS suspend count of the thread.
  ///     *
  ///     * This function only makes sense in the context of
  ///     * unmanaged debugging - during managed debugging threads are not
  ///     * OS suspended. (They are cooperatively suspended.)
  /// */
  ///
  ///    HRESULT IsOSSuspended([in] DWORD threadID, [out] BOOL *pbSuspended);
  ///
  ///    /*
  ///     * GetThreadContext returns the context for the given thread.  The
  ///     * debugger should call this function rather than the Win32
  ///     * GetThreadContext, because the thread may actually be in a "hijacked"
  ///     * state where its context has been temporarily changed.
  ///     *
  ///     * This should only be used on when a thread is in native code. Use ICorDebugRegisterSet
  ///     * for threads in managed code.
  ///     *
  ///     * The data returned is a CONTEXT structure for the current platform.
  ///     * (CONTEXT is typically declared in winnt.h) Just as with a call
  ///     * to Win32's GetThreadContext, the caller should initialize the
  ///     * CONTEXT struct before calling.
  ///     *
  ///     */
  ///
  ///    HRESULT GetThreadContext([in] DWORD threadID,
  ///                             [in] ULONG32 contextSize,
  ///                             [in, out, length_is(contextSize),
  ///                             size_is(contextSize)] BYTE context[]);
  ///
  ///    /*
  ///     * SetThreadContext sets the context for the given thread.  The
  ///     * debugger should call this function rather than the Win32
  ///     * SetThreadContext, because the thread may actually be in a "hijacked"
  ///     * state where its context has been temporarily changed.
  ///     *
  ///     * This should only be used on when a thread is in native code. Use ICorDebugRegisterSet
  ///     * for threads in managed code.
  ///     *
  ///     * This should never be needed to modify the context of a thread during an oob-debug
  ///     * event.
  ///     *
  ///     * The data passed should be a CONTEXT structure for the current platform.
  ///     * (CONTEXT is typically declared in winnt.h)
  ///     *
  ///     * This is a dangerous call which can corrupt the runtime if used
  ///     * improperly.
  ///     *
  ///     */
  ///
  ///    HRESULT SetThreadContext([in] DWORD threadID,
  ///                             [in] ULONG32 contextSize,
  ///                             [in, length_is(contextSize),
  ///                             size_is(contextSize)] BYTE context[]);
  ///
  ///    /*
  ///     * ReadMemory reads memory from the process.
  ///     * This is primarily intended to be used by interop-debugging to inspect memory
  ///     * regions used by the unmanaged portion of the debuggee.
  ///     *
  ///     * This can also be used to read IL and native jitted code.
  ///     * Any managed breakpoints will be automatically stripped from the returned buffer.
  ///     * No adjustments will be made for Native breakpoints set by ICorDebugProcess2::SetUnmanagedBreakpoint
  ///     *
  ///     * No caching of process memory is peformed.
  ///     * These parameters have the same semantics as kernel32!ReadProcessMemory.
  ///     * The entire range must be read for the function to return success.
  ///     */
  ///
  ///    HRESULT ReadMemory([in] CORDB_ADDRESS address, [in] DWORD size,
  ///                       [out, size_is(size), length_is(size)] BYTE buffer[],
  ///                       [out] SIZE_T *read);
  ///
  ///    /*
  ///     * WriteMemory writes memory in the process.
  ///     * In v2.0, Native debuggers should *not* use this to inject breakpoints
  ///     * into the instruction stream. Use ICorDebugProcess2::SetUnamangedBreakpoint
  ///     * instead.
  ///     *
  ///     * This is a dangerous call which can corrupt the runtime if used
  ///     * improperly. It is highly recommended that this is only used outside
  ///     * of managed code.
  ///     *
  ///     * These parameters have the same semantics as kernel32!WriteProcessMemory.
  ///     */
  ///
  ///    HRESULT WriteMemory([in] CORDB_ADDRESS address, [in] DWORD size,
  ///                        [in, size_is(size)] BYTE buffer[],
  ///                        [out]SIZE_T *written);
  ///
  ///
  ///    /*
  ///     * ClearCurrentException clears the current unmanaged exception on
  ///     * the given thread. Call this before calling Continue when a
  ///     * thread has reported an unmanaged exception that should be
  ///     * ignored by the debuggee.
  ///     *
  ///     * This will clear both the outstanding IB and OOB events on the given thread.
  ///     * Out-of-band Breakpoint and single-step exceptions are automatically cleared.
  ///     *
  ///     * See ICorDebugThread2::InterceptCurrentException for continuing managed exceptions.
  ///     *
  ///     */
  ///
  ///    HRESULT ClearCurrentException([in] DWORD threadID);
  ///
  ///    /*
  ///     * EnableLogMessages enables/disables sending of log messages to the
  ///     * debugger for logging.
  ///     * This is only valid after the CreateProcess callback.
  ///     *
  ///     */
  ///
  ///    HRESULT EnableLogMessages([in]BOOL fOnOff);
  ///
  ///    /*
  ///     * ModifyLogSwitch modifies the specified switch's severity level.
  ///     * This is only valid after the CreateProcess callback.
  ///     *
  ///     */
  ///    HRESULT ModifyLogSwitch([in, annotation("_In_")] WCHAR *pLogSwitchName,
  ///                            [in]LONG lLevel);
  ///
  ///    /*
  ///     * EnumerateAppDomains enumerates all app domains in the process.
  ///     * This can be used before the CreateProcess callback.
  ///     *
  ///     */
  ///
  ///    HRESULT EnumerateAppDomains([out] ICorDebugAppDomainEnum **ppAppDomains);
  ///
  ///    /*
  ///     * NOT YET IMPLEMENTED
  ///     */
  ///
  ///    HRESULT GetObject([out] ICorDebugValue **ppObject);
  ///
  ///    /*
  ///     * DEPRECATED
  ///     */
  ///
  ///    HRESULT ThreadForFiberCookie([in] DWORD fiberCookie,
  ///                                 [out] ICorDebugThread **ppThread);
  ///
  ///    /*
  ///     * Returns the OS thread id of the debugger's internal helper thread.
  ///     * During managed/unmanaged debugging, it is the debugger's
  ///     * responsibility to ensure that the thread with this ID remains running
  ///     * if it hits a breakpoint placed by the debugger. A debugger may also
  ///     * wish to hide this thread from the user.
  ///     *
  ///     * If there is no helper thread in the process yet, then this method
  ///     * will return zero as the thread id.
  ///     *
  ///     * Note: you cannot cache this value. The ID of the helper thread may
  ///     * change over time, so this value must be re-queried at every stopping
  ///     * event.
  ///     *
  ///     * Note: this value will be correct on every unmanaged CreateThread event.
  ///     * This will allow a debugger to determine the TID of the helper thread
  ///     * and hide it from the user. A thread identified as a helper thread during
  ///     * an unmanaged CreateThread event will never run managed user code.
  ///     */
  ///
  ///    HRESULT GetHelperThreadID([out] DWORD *pThreadID);
  ///};
  /// </code></example>
    [Guid ("3D6F5F64-7538-11D3-8D5B-00104B35E7EF")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugProcess : ICorDebugController
    {
        /// <inheritdoc cref="ICorDebugController.Stop"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Stop ([In] UInt32 dwTimeoutIgnored);

        /// <inheritdoc cref="ICorDebugController.Continue"/>
        [MustUseReturnValue]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new Int32 Continue([In] Int32 fIsOutOfBand);

        /// <inheritdoc cref="ICorDebugController.IsRunning"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 IsRunning (Int32 *pbRunning);

        /// <inheritdoc cref="ICorDebugController.HasQueuedCallbacks"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 HasQueuedCallbacks ([MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pThread, Int32 *pbQueued);

        /// <inheritdoc cref="ICorDebugController.EnumerateThreads"/>
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        [MustUseReturnValue]
        new Int32 EnumerateThreads ([MarshalAs (UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);

        /// <inheritdoc cref="ICorDebugController.SetAllThreadsDebugState"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 SetAllThreadsDebugState ([In] CorDebugThreadState state,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pExceptThisThread);

        /// <inheritdoc cref="ICorDebugController.Detach"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Detach ();

        /// <inheritdoc cref="ICorDebugController.Terminate"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Terminate ([In] UInt32 exitCode);

        /// <inheritdoc cref="ICorDebugController.CanCommitChanges"/>
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CanCommitChanges ([In] UInt32 cSnapshots, /*ICorDebugEditAndContinueSnapshot*/void** pSnapshots, /*ICorDebugErrorInfoEnum*/void** pError);

        /// <inheritdoc cref="ICorDebugController.CommitChanges"/>
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CommitChanges ([In] UInt32 cSnapshots, /*ICorDebugEditAndContinueSnapshot*/void** pSnapshots, /*ICorDebugErrorInfoEnum*/void** pError);

      /// <summary>
      /// GetID returns the OS ID of the process.
      /// </summary>
      /// <param name="pdwProcessId"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetID (UInt32* pdwProcessId);

      /// <summary>
      /// GetHandle returns a handle to the process. This handle is owned
      /// by the debugging API; the debugger should duplicate it before
      /// using it.
      /// </summary>
      /// <param name="phProcessHandle"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetHandle ([ComAliasName("HPROCESS *")]void** phProcessHandle);

      /// <summary>
      /// GetThread returns the ICorDebugThread with the given OS Id.
      /// 
      /// Note that eventually there will not be a one to one correspondence
      /// between OS threads and runtime threads, so this entry point will
      /// go away.
      /// </summary>
      /// <param name="dwThreadId"></param>
      /// <param name="ppThread"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetThread ([In] UInt32 dwThreadId, [MarshalAs (UnmanagedType.Interface)] out ICorDebugThread ppThread);

      /// <summary>
      /// NOT YET IMPLEMENTED
      /// </summary>
      /// <param name="ppObjects"></param>
      [Obsolete("NOT YET IMPLEMENTED")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EnumerateObjects ([MarshalAs (UnmanagedType.Interface)] out ICorDebugObjectEnum ppObjects);

      /// <summary>
      /// IsTransitionStub tests whether an address is inside of a transition stub
      /// which will cause a transition to managed code.  This can be used by
      /// unmanaged stepping code to decide when to return stepping control to
      /// the managed stepper.
      /// 
      /// Note that, tentatively, these stubs may also be able to be identified
      /// ahead of time by looking at information in the PE file.
      /// </summary>
      /// <param name="address"></param>
      /// <param name="pbTransitionStub"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsTransitionStub ([In][ComAliasName("CORDB_ADDRESS")] UInt64 address, Int32* pbTransitionStub);

      /// <summary>
      /// IsOSSuspended returns whether or not the thread has been
      /// suspended as part of the debugger logic of stopping the process.
      /// (that is, it has had its Win32 suspend count incremented by
      /// one.)  The debugger UI may want to take this into account if
      /// it shows the user the OS suspend count of the thread.
      /// 
      /// This function only makes sense in the context of
      /// unmanaged debugging - during managed debugging threads are not
      /// OS suspended. (They are cooperatively suspended.)
      /// </summary>
      /// <param name="threadID"></param>
      /// <param name="pbSuspended"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsOSSuspended ([In] UInt32 threadID, Int32* pbSuspended);

      /// <summary>
      /// GetThreadContext returns the context for the given thread.  The
      /// debugger should call this function rather than the Win32
      /// GetThreadContext, because the thread may actually be in a "hijacked"
      /// state where its context has been temporarily changed.
      /// 
      /// This should only be used on when a thread is in native code. Use ICorDebugRegisterSet
      /// for threads in managed code.
      /// 
      /// The data returned is a CONTEXT structure for the current platform.
      /// (CONTEXT is typically declared in winnt.h) Just as with a call
      /// to Win32's GetThreadContext, the caller should initialize the
      /// CONTEXT struct before calling. 
      /// </summary>
      /// <param name="threadID"></param>
      /// <param name="contextSize"></param>
      /// <param name="context"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetThreadContext ([In] UInt32 threadID, [In] UInt32 contextSize, [In, Out] Byte* context);

      /// <summary>
      /// SetThreadContext sets the context for the given thread.  The
      /// debugger should call this function rather than the Win32
      /// SetThreadContext, because the thread may actually be in a "hijacked"
      /// state where its context has been temporarily changed.
      /// 
      /// This should only be used on when a thread is in native code. Use ICorDebugRegisterSet
      /// for threads in managed code.
      /// 
      /// This should never be needed to modify the context of a thread during an oob-debug
      /// event.
      /// 
      /// The data passed should be a CONTEXT structure for the current platform.
      /// (CONTEXT is typically declared in winnt.h)
      /// 
      /// This is a dangerous call which can corrupt the runtime if used
      /// improperly. 
      /// </summary>
      /// <param name="threadID"></param>
      /// <param name="contextSize"></param>
      /// <param name="context"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetThreadContext ([In] UInt32 threadID, [In] UInt32 contextSize, [In, Out] Byte* context);

      /// <summary>
      /// ReadMemory reads memory from the process.
      /// This is primarily intended to be used by interop-debugging to inspect memory
      /// regions used by the unmanaged portion of the debuggee.
      /// 
      /// This can also be used to read IL and native jitted code.
      /// Any managed breakpoints will be automatically stripped from the returned buffer.
      /// No adjustments will be made for Native breakpoints set by ICorDebugProcess2::SetUnmanagedBreakpoint
      /// 
      /// No caching of process memory is peformed.
      /// These parameters have the same semantics as kernel32!ReadProcessMemory.
      /// The entire range must be read for the function to return success.
      /// </summary>
      /// <param name="address"></param>
      /// <param name="size"></param>
      /// <param name="buffer"></param>
      /// <param name="read"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ReadMemory ([In][ComAliasName("CORDB_ADDRESS")] UInt64 address, [In] UInt32 size, [Out] Byte* buffer,
            UIntPtr* read);

      /// <summary>
      /// WriteMemory writes memory in the process.
      /// In v2.0, Native debuggers should *not* use this to inject breakpoints
      /// into the instruction stream. Use ICorDebugProcess2::SetUnamangedBreakpoint
      /// instead.
      /// 
      /// This is a dangerous call which can corrupt the runtime if used
      /// improperly. It is highly recommended that this is only used outside
      /// of managed code.
      /// 
      /// These parameters have the same semantics as kernel32!WriteProcessMemory. 
      /// </summary>
      /// <param name="address"></param>
      /// <param name="size"></param>
      /// <param name="buffer"></param>
      /// <param name="written"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 WriteMemory ([In] UInt64 address, [In] UInt32 size, [In] Byte* buffer,
            UIntPtr* written);

      /// <summary>
      /// ClearCurrentException clears the current unmanaged exception on
      /// the given thread. Call this before calling Continue when a
      /// thread has reported an unmanaged exception that should be
      /// ignored by the debuggee.
      /// 
      /// This will clear both the outstanding IB and OOB events on the given thread.
      /// Out-of-band Breakpoint and single-step exceptions are automatically cleared.
      /// 
      /// See ICorDebugThread2::InterceptCurrentException for continuing managed exceptions.
      /// </summary>
      /// <param name="threadID"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ClearCurrentException ([In] UInt32 threadID);

      /// <summary>
      /// EnableLogMessages enables/disables sending of log messages to the
      /// debugger for logging.
      /// This is only valid after the CreateProcess callback.
      /// </summary>
      /// <param name="fOnOff"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EnableLogMessages ([In] Int32 fOnOff);

      /// <summary>
      /// ModifyLogSwitch modifies the specified switch's severity level.
      /// This is only valid after the CreateProcess callback.
      /// </summary>
      /// <param name="pLogSwitchName"></param>
      /// <param name="lLevel"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ModifyLogSwitch ([In] UInt16* pLogSwitchName, [In] Int32 lLevel);

      /// <summary>
      /// EnumerateAppDomains enumerates all app domains in the process.
      /// This can be used before the CreateProcess callback.
      /// </summary>
      /// <param name="ppAppDomains"></param>
      [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
      [PreserveSig]
      [MustUseReturnValue]
      Int32 EnumerateAppDomains([MarshalAs(UnmanagedType.Interface)] out ICorDebugAppDomainEnum ppAppDomains);

      /// <summary>
      /// NOT YET IMPLEMENTED
      /// </summary>
      /// <param name="ppObject"></param>
      [Obsolete("NOT YET IMPLEMENTED")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetObject ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppObject);

      /// <summary>
      /// DEPRECATED
      /// </summary>
      /// <param name="fiberCookie"></param>
      /// <param name="ppThread"></param>
      [Obsolete("DEPRECATED")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ThreadForFiberCookie ([In] UInt32 fiberCookie,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugThread ppThread);

      /// <summary>
      /// Returns the OS thread id of the debugger's internal helper thread.
      /// During managed/unmanaged debugging, it is the debugger's
      /// responsibility to ensure that the thread with this ID remains running
      /// if it hits a breakpoint placed by the debugger. A debugger may also
      /// wish to hide this thread from the user.
      /// 
      /// If there is no helper thread in the process yet, then this method
      /// will return zero as the thread id.
      /// 
      /// Note: you cannot cache this value. The ID of the helper thread may
      /// change over time, so this value must be re-queried at every stopping
      /// event.
      /// 
      /// Note: this value will be correct on every unmanaged CreateThread event.
      /// This will allow a debugger to determine the TID of the helper thread
      /// and hide it from the user. A thread identified as a helper thread during
      /// an unmanaged CreateThread event will never run managed user code.
      /// </summary>
      /// <param name="pThreadID"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetHelperThreadID (UInt32* pThreadID);
    }
}