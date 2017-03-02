using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugThread represents a thread in the process.  The lifetime of a thread object is equal to the lifetime of the thread it represents.
  /// </summary>
  /// <example><code>
  /// /*
  /// * ICorDebugThread represents a thread in the process.  The lifetime of a
  /// * thread object is equal to the lifetime of the thread it represents.
  /// */
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(938c6d66-7fb6-4f69-b389-425b8987329b),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugThread : IUnknown
  ///{
  ///    /*
  ///     * GetProcess returns the process of which this thread is a part.
  ///     */
  ///
  ///    HRESULT GetProcess([out] ICorDebugProcess **ppProcess);
  ///
  ///    /*
  ///     * GetID returns the current OS ID of the active part of the thread.
  ///     * Note that this may theoretically change as the process executes,
  ///     * and even be different for different parts of the thread.
  ///     */
  ///
  ///    HRESULT GetID([out] DWORD *pdwThreadId);
  ///
  ///    /*
  ///     * GetHandle returns the current Handle of the active part of the thread.
  ///     * Note that this may theoretically change as the process executes,
  ///     * and even be different for different parts of the thread.
  ///     *
  ///     * This handle is owned by the debugging API. The debugger should duplicate
  ///     * it before using it.
  ///     */
  ///
  ///    HRESULT GetHandle([out] HTHREAD *phThreadHandle);
  ///
  ///    /*
  ///     * GetAppDomain returns the app domain which the thread is currently
  ///     * executing in.
  ///     */
  ///
  ///    HRESULT GetAppDomain([out] ICorDebugAppDomain **ppAppDomain);
  ///
  ///    /*
  ///     * SetDebugState sets the current debug state of the thread.
  ///     * (The "current debug state"
  ///     * represents the debug state if the process were to be continued,
  ///     * not the actual current state.)
  ///     *
  ///     * The normal value for this is THREAD_RUNNING.  Only the debugger
  ///     * can affect the debug state of a thread.  Debug states do
  ///     * last across continues, so if you want to keep a thread
  ///     * THREAD_SUSPENDed over mulitple continues, you can set it once
  ///     * and thereafter not have to worry about it.
  ///     *
  ///     * Suspending threads and resuming the process can cause deadlocks, though it's
  ///     * usually unlikely. This is an intrinisc quality of threads and processes and is by-design.
  ///     * A debugger can async break and resume the threads to break the deadlock.
  ///     *
  ///     * If the thread's user state includes USER_UNSAFE_POINT, then the thread may block a GC.
  ///     * This means the suspended thread has a mcuh higher chance of causing a deadlock.
  ///     *
  ///     * This may not affect debug events already queued. Thus a debugger should drain the entire 
  ///     * event queue (via calling HasQueuedCallbacks) before suspending or resuming threads. Else it
  ///     * may get events on a thread that it believes it has already suspended.
  ///     *
  ///     */
  ///
  ///    HRESULT SetDebugState([in] CorDebugThreadState state);
  ///
  ///    /*
  ///     * GetDebugState returns the current debug state of the thread.
  ///     * (If the process is currently stopped, the "current debug state"
  ///     * represents the debug state if the process were to be continued,
  ///     * not the actual current state.)
  ///     */
  ///
  ///    HRESULT GetDebugState([out] CorDebugThreadState *pState);
  ///
  ///    /*
  ///     * GetUserState returns the user state of the thread, that is, the state
  ///     * which it has when the program being debugged examines it.
  ///     * A thread may have multiple state bits set.
  ///     */
  ///
  ///    typedef enum CorDebugUserState
  ///    {
  ///        USER_STOP_REQUESTED     = 0x01,
  ///        USER_SUSPEND_REQUESTED  = 0x02,
  ///        USER_BACKGROUND         = 0x04,
  ///        USER_UNSTARTED          = 0x08,
  ///        USER_STOPPED            = 0x10,
  ///        USER_WAIT_SLEEP_JOIN    = 0x20,
  ///        USER_SUSPENDED          = 0x40,
  ///
  ///        // An "unsafe point" is a place where the thread may block a Garbage Collection (GC).
  ///        // Debug events may be dispatched from unsafe points, but suspending a thread at
  ///        // an unsafe spot will very likely cause a deadlock (until the thread is resumed).
  ///        // This is a function of the thread's IP and the available GC info. The exact details
  ///        // of what is safe and unsafe is unspecified and highly determined by jit/gc implementation details.
  ///        USER_UNSAFE_POINT       = 0x80,
  ///
  ///        // indicates that this thread is a threadpool thread
  ///        USER_THREADPOOL         = 0x100,
  ///    } CorDebugUserState;
  ///
  ///    HRESULT GetUserState([out] CorDebugUserState *pState);
  ///
  ///    /*
  ///     * GetCurrentException returns the exception object which is
  ///     * currently being thrown by the thread.  This will exist from the time the exception
  ///     * is thrown until the end of the catch block. That range will include filters
  ///     * and finallys.
  ///     *
  ///     * FuncEval will clear out the exception object on setup and restore it on completion.
  ///     *
  ///     * Exceptions can be nested (eg, if an exception is thrown in filter or a func-eval),
  ///     * so there may be multiple outstanding exceptions on a single thread.
  ///     * This returns the most current exception.
  ///     *
  ///     * The exception object and type may change throughout the life of the exception. For example, an
  ///     * exception of type X may be thrown, but then the CLR may run out of memory and promote
  ///     * that to an OutOfMemory exception.
  ///     */
  ///
  ///    HRESULT GetCurrentException([out] ICorDebugValue **ppExceptionObject);
  ///
  ///    /*
  ///     * This is not implemented.
  ///     */
  ///
  ///    HRESULT ClearCurrentException();
  ///
  ///    /*
  ///     * CreateStepper creates a stepper object which operates relative
  ///     * to the active frame in the given thread. (Note that this may be
  ///     * unmanaged code.)  The Stepper API must then be used to perform
  ///     * actual stepping.
  ///     *
  ///     */
  ///
  ///    HRESULT CreateStepper([out] ICorDebugStepper **ppStepper);
  ///
  ///    /*
  ///     * EnumerateChains returns an enum which will return all the stack
  ///     * chains in the thread, starting at the active (most recent) one.
  ///     * These chains represent the physical call stack for the thread.
  ///     *
  ///     * Chain boundaries occur for several reasons:
  ///     *   managed &lt;-&gt; unmanaged transitions
  ///     *   context switches
  ///     *   debugger hijacking of user threads
  ///     *
  ///     * Note that in the simple case for a thread running purely
  ///     * managed code in a single context there will be a one to one
  ///     * correspondence between threads &amp; chains.
  ///     *
  ///     * A debugger may want to rearrange the physical call
  ///     * stacks of all threads into logical call stacks. This would involve
  ///     * sorting all the threads' chains by their caller/callee
  ///     * relationships &amp; regrouping them.
  ///     *
  ///     */
  ///
  ///    HRESULT EnumerateChains([out] ICorDebugChainEnum **ppChains);
  ///
  ///    /*
  ///     * GetActiveChain is a convenience routine to return the
  ///     * active (most recent) chain on the thread, if any.
  ///     *
  ///     */
  ///
  ///    HRESULT GetActiveChain([out] ICorDebugChain **ppChain);
  ///
  ///    /*
  ///     * GetActiveFrame is a convenience routine to return the
  ///     * active (most recent) frame on the thread, if any.
  ///     * If there are no frames on the stack, ppFrame will point to NULL
  ///     * and the function still returns S_OK.
  ///     */
  ///
  ///    HRESULT GetActiveFrame([out] ICorDebugFrame **ppFrame);
  ///
  ///    /*
  ///     * GetRegisterSet returns the register set for the active part
  ///     * of the thread.
  ///     *
  ///     */
  ///
  ///    HRESULT GetRegisterSet([out] ICorDebugRegisterSet **ppRegisters);
  ///
  ///    /*
  ///     * CreateEval creates an evaluation object which operates on the
  ///     * given thread.  The Eval will push a new chain on the thread before
  ///     * doing its computation.
  ///     *
  ///     * Note that this interrupts the computation currently
  ///     * being performed on the thread until the eval completes.
  ///     *
  ///     */
  ///
  ///    HRESULT CreateEval([out] ICorDebugEval **ppEval);
  ///
  ///    /*
  ///     * Returns the runtime thread object.
  ///     */
  ///
  ///    HRESULT GetObject([out] ICorDebugValue **ppObject);
  ///
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("938C6D66-7FB6-4F69-B389-425B8987329B")]
    [ComImport]
    public unsafe interface ICorDebugThread
    {
      /// <summary>
      /// GetProcess returns the process of which this thread is a part.
      /// </summary>
      /// <param name="ppProcess"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetProcess ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

      /// <summary>
      /// GetID returns the current OS ID of the active part of the thread.
      /// Note that this may theoretically change as the process executes,
      /// and even be different for different parts of the thread.
      /// </summary>
      /// <param name="pdwThreadId"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetID (UInt32 *pdwThreadId);

      /// <summary>
      /// GetHandle returns the current Handle of the active part of the thread.
      /// Note that this may theoretically change as the process executes,
      /// and even be different for different parts of the thread.
      /// 
      /// This handle is owned by the debugging API. The debugger should duplicate
      /// it before using it.
      /// </summary>
      /// <param name="phThreadHandle"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetHandle (void **phThreadHandle);
      
      /// <summary>
      /// GetAppDomain returns the app domain which the thread is currently executing in.
      /// </summary>
      /// <param name="ppAppDomain"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAppDomain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAppDomain ppAppDomain);

      /// <summary>
      /// SetDebugState sets the current debug state of the thread.
      /// (The "current debug state"
      /// represents the debug state if the process were to be continued,
      /// not the actual current state.)
      /// 
      /// The normal value for this is THREAD_RUNNING.  Only the debugger
      /// can affect the debug state of a thread.  Debug states do
      /// last across continues, so if you want to keep a thread
      /// THREAD_SUSPENDed over mulitple continues, you can set it once
      /// and thereafter not have to worry about it.
      /// 
      /// Suspending threads and resuming the process can cause deadlocks, though it's
      /// usually unlikely. This is an intrinisc quality of threads and processes and is by-design.
      /// A debugger can async break and resume the threads to break the deadlock.
      /// 
      /// If the thread's user state includes USER_UNSAFE_POINT, then the thread may block a GC.
      /// This means the suspended thread has a mcuh higher chance of causing a deadlock.
      /// 
      /// This may not affect debug events already queued. Thus a debugger should drain the entire 
      /// event queue (via calling HasQueuedCallbacks) before suspending or resuming threads. Else it
      /// may get events on a thread that it believes it has already suspended.
      /// </summary>
      /// <param name="state"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetDebugState ([In] CorDebugThreadState state);

      /// <summary>
      /// GetDebugState returns the current debug state of the thread.
      /// (If the process is currently stopped, the "current debug state"
      /// represents the debug state if the process were to be continued,
      /// not the actual current state.)
      /// </summary>
      /// <param name="pState"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDebugState (CorDebugThreadState *pState);

      /// <summary>
      /// GetUserState returns the user state of the thread, that is, the state
      /// which it has when the program being debugged examines it.
      /// A thread may have multiple state bits set.
      /// </summary>
      /// <param name="pState"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetUserState (CorDebugUserState *pState);

      /// <summary>
      /// GetCurrentException returns the exception object which is
      /// currently being thrown by the thread.  This will exist from the time the exception
      /// is thrown until the end of the catch block. That range will include filters
      /// and finallys.
      /// 
      /// FuncEval will clear out the exception object on setup and restore it on completion.
      /// 
      /// Exceptions can be nested (eg, if an exception is thrown in filter or a func-eval),
      /// so there may be multiple outstanding exceptions on a single thread.
      /// This returns the most current exception.
      /// 
      /// The exception object and type may change throughout the life of the exception. For example, an
      /// exception of type X may be thrown, but then the CLR may run out of memory and promote
      /// that to an OutOfMemory exception.
      /// </summary>
      /// <param name="ppExceptionObject"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCurrentException ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppExceptionObject);

      /// <summary>
      /// This is not implemented.
      /// </summary>
      [Obsolete("This is not implemented.")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ClearCurrentException ();

      /// <summary>
      /// CreateStepper creates a stepper object which operates relative
      /// to the active frame in the given thread. (Note that this may be
      /// unmanaged code.)  The Stepper API must then be used to perform
      /// actual stepping.
      /// </summary>
      /// <param name="ppStepper"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateStepper ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStepper ppStepper);

      /// <summary>
      /// EnumerateChains returns an enum which will return all the stack
      /// chains in the thread, starting at the active (most recent) one.
      /// These chains represent the physical call stack for the thread.
      /// 
      /// Chain boundaries occur for several reasons:
      ///   managed &lt;-&gt; unmanaged transitions
      ///   context switches
      ///   debugger hijacking of user threads
      /// 
      /// Note that in the simple case for a thread running purely
      /// managed code in a single context there will be a one to one
      /// correspondence between threads & chains.
      /// 
      /// A debugger may want to rearrange the physical call
      /// stacks of all threads into logical call stacks. This would involve
      /// sorting all the threads' chains by their caller/callee
      /// relationships & regrouping them.
      /// </summary>
      /// <param name="ppChains"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateChains ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChainEnum ppChains);

      /// <summary>
      /// GetActiveChain is a convenience routine to return the
      /// active (most recent) chain on the thread, if any.
      /// </summary>
      /// <param name="ppChain"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetActiveChain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

      /// <summary>
      /// GetActiveFrame is a convenience routine to return the
      /// active (most recent) frame on the thread, if any.
      /// If there are no frames on the stack, ppFrame will point to NULL
      /// and the function still returns S_OK.
      /// </summary>
      /// <param name="ppFrame"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetActiveFrame ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

      /// <summary>
      /// GetRegisterSet returns the register set for the active part
      /// of the thread.
      /// </summary>
      /// <param name="ppRegisters"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRegisterSet ([MarshalAs (UnmanagedType.Interface)] out ICorDebugRegisterSet ppRegisters);

      /// <summary>
      /// CreateEval creates an evaluation object which operates on the
      /// given thread.  The Eval will push a new chain on the thread before
      /// doing its computation.
      /// 
      /// Note that this interrupts the computation currently
      /// being performed on the thread until the eval completes.
      /// </summary>
      /// <param name="ppEval"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateEval ([MarshalAs (UnmanagedType.Interface)] out ICorDebugEval ppEval);

      /// <summary>
      /// Returns the runtime thread object.
      /// </summary>
      /// <param name="ppObject"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetObject ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppObject);
    }
}