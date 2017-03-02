using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    /// ICorDebugController represents a scope at which program execution context
    /// can be controlled.  It represents either a process or an app domain.
    ///
    /// If this is the controller of a process, this controller affects all
    /// threads in the process.  Otherwise it just affects the threads of
    /// a particular app domain
    /// </summary>
    //
    // [object, local, uuid(3d6f5f62-7538-11d3-8d5b-00104b35e7ef), pointer_default(unique)]
    [Guid ("3D6F5F62-7538-11D3-8D5B-00104B35E7EF")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugController
    {
        /// <summary>
        /// Stop performs a cooperative stop on all threads running managed
        /// code in the process. When managed-only debugging, unmanaged threads may continue
        /// to run (but will be blocked when trying to call managed code). When-interop debugging,
        /// unmanaged threads will also be stopped.
        /// The timeout value is currently ignored and treated as INFINTE (-1).
        /// If the cooperative stop fails due to a deadlock, all threads are suspended (and E_TIMEOUT is returned)
        ///
        /// NOTE: This function is the one function in the debugging API
        /// that is synchronous. When Stop returns with S_OK, the process
        /// is stopped. (No callback will be given to notify of the stop.)
        /// The debugger must call Continue when it wishes to allow
        /// the process to resume running.
        ///
        /// The debugger maintains a "stop-counter". When the counter goes to zero, the
        /// Controller is resumed. Each call to Stop() or each dispatched callback will increment
        /// the counter. Each call to continue will decrement the counter.
        /// </summary>
        /// <param name="dwTimeoutIgnored"></param>
        //
        // HRESULT Stop([in] DWORD dwTimeoutIgnored);
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Stop ([In] UInt32 dwTimeoutIgnored);

        /// <summary>
        /// Continue continues the process after a call to Stop.
        ///
        /// Continue continues the process. fIsOutOfBand is set to TRUE
        /// if continuing from an unmanaged event that was sent with the
        /// fOutOfBand flag in the unmanaged callback and it is set to
        /// FALSE if continuing from a managed event or a normal
        /// unmanaged event.
        ///
        /// When doing mixed-mode debugging, Continue cannot be called on
        /// the Win32 Event Thread unless it is continuing from an
        /// out-of-band event.
        /// </summary>
        /// <param name="fIsOutOfBand"></param>
        //
        // HRESULT Continue([in] BOOL fIsOutOfBand);
        [MustUseReturnValue]
        [PreserveSig]
        Int32 Continue ([In] Int32 fIsOutOfBand);

        /// <summary>
        /// IsRunning returns TRUE if the threads in the process are running freely.
        /// </summary>
        /// <param name="pbRunning"></param>
        //
        // HRESULT IsRunning([out] BOOL *pbRunning);
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void IsRunning (Int32 *pbRunning);

        /// <summary>
        /// HasQueuedCallbacks returns TRUE if there are currently managed
        /// callbacks which are queued up for the given thread.  These
        /// callbacks will be dispatched one at a time, each time Continue
        /// is called.
        ///
        /// The debugger can check this flag if it wishes to report multiple
        /// debugging events which occur simultaneously.
        ///
        /// If NULL is given for the pThread parameter, HasQueuedCallbacks
        /// will return TRUE if there are currently managed callbacks
        /// queued for any thread.
        ///
        /// Note that once debug events have been queued, they've already occurred,
        /// and so the debugger must drain the entire queue to be sure of the state
        /// of the debuggee. For example, if the queue contains 2 debug events on thread X,
        /// and the debugger suspends thread X after the 1st debug event and then calls continue,
        /// the 2nd debug event for thread X will still be dispatched even though the thread
        /// is suspended.
        /// </summary>
        /// <param name="pThread"></param>
        /// <param name="pbQueued"></param>
        //
        // HRESULT HasQueuedCallbacks([in] ICorDebugThread *pThread, [out] BOOL *pbQueued);
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void HasQueuedCallbacks ([MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pThread, Int32 *pbQueued);

        /// <summary>
        /// EnumerateThreads returns an enum of all managed threads active in the process.
        /// A thread is considered Managed threads after the CreateThread callback has been
        /// dispatched and before the ExitThread callback has been dispatched.
        /// A managed thread may not necessarily have any managed frames on its stack.
        ///
        /// Threads can be enumerated even before the CreateProcess callback. The enumeration
        /// will naturally be empty.
        /// </summary>
        /// <param name="ppThreads"></param>
        //
        // HRESULT EnumerateThreads([out] ICorDebugThreadEnum **ppThreads);
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateThreads ([MarshalAs (UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);

        /// <summary>
        /// SetAllThreadsDebugState sets the current debug state of each thread.
        /// See ICorDebugThread::SetDebugState for details.
        ///
        /// The pExceptThisThread parameter allows you to specify one
        /// thread which is exempted from the debug state change. Pass NULL
        /// if you want to affect all threads.
        ///
        /// This may affect threads not visible via EnumerateThreads, so threads suspended
        /// via this API will need to be resumed via this API too.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pExceptThisThread"></param>
        //
        // HRESULT SetAllThreadsDebugState([in] CorDebugThreadState state, [in] ICorDebugThread *pExceptThisThread);
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetAllThreadsDebugState ([In] CorDebugThreadState state,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pExceptThisThread);

        /// <summary>
        /// Detach detaches the debugger from the process.  The process
        /// continues execution normally. The ICorDebugProcess object is
        /// no longer valid and no further callbacks will occur.  This is
        /// not implemented for AppDomains (detaching is process-wide).
        ///
        /// Note that currently if unmanaged debugging is enabled this call will
        /// fail due to OS limitations.
        ///
        /// Returns S_OK on success.
        /// </summary>
        //
        // HRESULT Detach();
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Detach ();

        /// <summary>
        /// Terminate terminates the process (with extreme prejudice, I might add).
        ///
        /// NOTE: If the process or appdomain is stopped when Terminate is called,
        /// the process or appdomain should be continued using Continue so that the
        /// ExitProcess or ExitAppDomain callback is received.
        ///
        /// NOTE: This method is not implemented by an appdomain.
        /// </summary>
        /// <param name="exitCode"></param>
        //
        // HRESULT Terminate([in] UINT exitCode);
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Terminate ([In] UInt32 exitCode);

      /// <summary>
      /// DEPRECATED
      /// </summary>
      /// <param name="cSnapshots"></param>
      /// <param name="pSnapshots"></param>
      /// <param name="pError"></param>
      //
      // HRESULT CanCommitChanges([in] ULONG cSnapshots,
      //                          [in, size_is(cSnapshots)] ICorDebugEditAndContinueSnapshot *pSnapshots[],
      //                          [out] ICorDebugErrorInfoEnum **pError);
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      void CanCommitChanges ([In] UInt32 cSnapshots, /*ICorDebugEditAndContinueSnapshot*/void** pSnapshots, /*ICorDebugErrorInfoEnum*/void** pError);

      /// <summary>
      /// DEPRECATED
      /// </summary>
      /// <param name="cSnapshots"></param>
      /// <param name="pSnapshots"></param>
      /// <param name="pError"></param>
      //
      // HRESULT CommitChanges([in] ULONG cSnapshots,
      //                       [in, size_is(cSnapshots)] ICorDebugEditAndContinueSnapshot *pSnapshots[],
      //                       [out] ICorDebugErrorInfoEnum **pError);
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      void CommitChanges ([In] UInt32 cSnapshots, /*ICorDebugEditAndContinueSnapshot*/void** pSnapshots, /*ICorDebugErrorInfoEnum*/void** pError);
    }
}