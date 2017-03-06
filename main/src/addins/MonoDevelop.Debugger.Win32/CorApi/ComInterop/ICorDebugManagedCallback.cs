using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugManagedCallback is implemented by the user of the
  /// ICorDebug interfaces in order to respond to events in managed code
  /// in the debuggee process.
  /// This interface handles manage debug events from v1.0/v1.1
  /// 
  /// All callbacks are called with the process in the synchronized state
  /// All callbacks are serialized, and are called in in the same thread.
  /// Each callback implementor must call Continue in a callback to
  ///      resume execution.
  /// If Continue is not called before returning, the process will
  /// remain stopped. Continue must later be called before any more
  /// event callbacks will happen.
  /// </summary>
  /// <example><code>
  /// /*
  /// * ICorDebugManagedCallback is implemented by the user of the
  /// * ICorDebug interfaces in order to respond to events in managed code
  /// * in the debuggee process.
  /// * This interface handles manage debug events from v1.0/v1.1
  /// */
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(3d6f5f60-7538-11d3-8d5b-00104b35e7ef),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugManagedCallback : IUnknown
  ///{
  ///    /*
  ///     * All callbacks are called with the process in the synchronized state
  ///     * All callbacks are serialized, and are called in in the same thread.
  ///     * Each callback implementor must call Continue in a callback to
  ///     *      resume execution.
  ///     * If Continue is not called before returning, the process will
  ///     * remain stopped. Continue must later be called before any more
  ///     * event callbacks will happen.
  ///     *
  ///     */
  ///
  ///    /*
  ///     * Breakpoint is called when a breakpoint is hit.
  ///     */
  ///
  ///    HRESULT Breakpoint([in] ICorDebugAppDomain *pAppDomain,
  ///                       [in] ICorDebugThread *pThread,
  ///                       [in] ICorDebugBreakpoint *pBreakpoint);
  ///
  ///    /*
  ///     * StepComplete is called when a step has completed.  The stepper
  ///     * may be used to continue stepping if desired (except for TERMINATE
  ///     * reasons.)
  ///     *
  ///     * STEP_NORMAL means that stepping completed normally, in the same
  ///     *      function.
  ///     *
  ///     * STEP_RETURN means that stepping continued normally, after the function
  ///     *      returned.
  ///     *
  ///     * STEP_CALL means that stepping continued normally, at the start of
  ///     *      a newly called function.
  ///     *
  ///     * STEP_EXCEPTION_FILTER means that control passed to an exception filter
  ///     *      after an exception was thrown.
  ///     *
  ///     * STEP_EXCEPTION_HANDLER means that control passed to an exception handler
  ///     *      after an exception was thrown.
  ///     *
  ///     * STEP_INTERCEPT means that control passed to an interceptor.
  ///     *
  ///     * STEP_EXIT means that the thread exited before the step completed.
  ///     *      No more stepping can be performed with the stepper.
  ///     */
  ///
  ///    typedef enum CorDebugStepReason
  ///    {
  ///        STEP_NORMAL,
  ///        STEP_RETURN,
  ///        STEP_CALL,
  ///        STEP_EXCEPTION_FILTER,
  ///        STEP_EXCEPTION_HANDLER,
  ///        STEP_INTERCEPT,
  ///        STEP_EXIT
  ///    } CorDebugStepReason;
  ///
  ///    HRESULT StepComplete([in] ICorDebugAppDomain *pAppDomain,
  ///                         [in] ICorDebugThread *pThread,
  ///                         [in] ICorDebugStepper *pStepper,
  ///                         [in] CorDebugStepReason reason);
  ///
  ///    /*
  ///     * Break is called when a break opcode in the code stream is
  ///     * executed.
  ///     */
  ///
  ///    HRESULT Break([in] ICorDebugAppDomain *pAppDomain,
  ///                  [in] ICorDebugThread *thread);
  ///
  ///    /*
  ///     * Exception is called when an exception is thrown from managed
  ///     * code, The specific exception can be retrieved from the thread object.
  ///     *
  ///     * If unhandled is FALSE, this is a "first chance" exception that
  ///     * hasn't had a chance to be processed by the application.  If
  ///     * unhandled is TRUE, this is an unhandled exception which will
  ///     * terminate the process.
  ///     */
  ///
  ///    HRESULT Exception([in] ICorDebugAppDomain *pAppDomain,
  ///                      [in] ICorDebugThread *pThread,
  ///                      [in] BOOL unhandled);
  ///
  ///    /*
  ///     * EvalComplete is called when an evaluation is completed.
  ///     */
  ///
  ///    HRESULT EvalComplete([in] ICorDebugAppDomain *pAppDomain,
  ///                         [in] ICorDebugThread *pThread,
  ///                         [in] ICorDebugEval *pEval);
  ///
  ///    /*
  ///     * EvalException is called when an evaluation terminates with
  ///     * an unhandled exception.
  ///     */
  ///
  ///    HRESULT EvalException([in] ICorDebugAppDomain *pAppDomain,
  ///                          [in] ICorDebugThread *pThread,
  ///                          [in] ICorDebugEval *pEval);
  ///
  ///    /*
  ///     * CreateProcess is called when a process is first attached to or
  ///     * started.
  ///     *
  ///     * This entry point won't be called until the EE is initialized.
  ///     * Most of the ICorDebug API will return CORDBG_E_NOTREADY prior
  ///     * to the CreateProcess callback.
  ///     */
  ///
  ///    HRESULT CreateProcess([in] ICorDebugProcess *pProcess);
  ///
  ///    /*
  ///     * ExitProcess is called when a process exits.
  ///     *
  ///     * Note: you don't Continue from an ExitProcess event, and this
  ///     * event may fire asynchronously to other events, while the
  ///     * process appears to be stopped. This can occur if the process
  ///     * dies while stopped, usually due to some external force.
  ///     *
  ///     * If the CLR is already dispatching a managed callback, this event
  ///     * will be delayed until after that callback has returned.
  ///     *
  ///     * This is the only exit/unload event that is guaranteed to get called
  ///     * on shutdown.
  ///     */
  ///
  ///    HRESULT ExitProcess([in] ICorDebugProcess *pProcess);
  ///
  ///    /*
  ///     * CreateThread is called when a thread first begins executing managed
  ///     * code. The thread will be positioned immediately at the first
  ///     * managed code to be executed.
  ///     */
  ///
  ///    HRESULT CreateThread([in] ICorDebugAppDomain *pAppDomain,
  ///                         [in] ICorDebugThread *thread);
  ///
  ///    /*
  ///     * ExitThread is called when a thread which has run managed code exits. 
  ///     * Once this callback is fired, the thread no longer will appear in thread enumerations.
  ///     */
  ///
  ///    HRESULT ExitThread([in] ICorDebugAppDomain *pAppDomain,
  ///                       [in] ICorDebugThread *thread);
  ///
  ///    /*
  ///     * LoadModule is called when a Common Language Runtime module is successfully
  ///     * loaded. This is an appropriate time to examine metadata for the
  ///     * module, set JIT compiler flags, or enable or disable
  ///     * class loading callbacks for the module.
  ///     */
  ///
  ///    HRESULT LoadModule([in] ICorDebugAppDomain *pAppDomain,
  ///                       [in] ICorDebugModule *pModule);
  ///
  ///    /*
  ///     * UnloadModule is called when a Common Language Runtime module (DLL) is unloaded. The module
  ///     * should not be used after this point.
  ///     */
  ///
  ///    HRESULT UnloadModule([in] ICorDebugAppDomain *pAppDomain,
  ///                         [in] ICorDebugModule *pModule);
  ///
  ///    /*
  ///     * LoadClass is called when a class finishes loading.  This callback only
  ///     * occurs if ClassLoading has been enabled for the class's module.
  ///     *
  ///     * ClassLoading is always enabled for dynamic modules. This is a good time
  ///     * to update symbols (ICorDebugModule3::CreateReaderForInMemorySymbols) and
  ///     * bind breakpoints to newly generated classes in dynamic modules.
  ///     */
  ///
  ///    HRESULT LoadClass([in] ICorDebugAppDomain *pAppDomain,
  ///                      [in] ICorDebugClass *c);
  ///
  ///    /*
  ///     * UnloadClass is called immediately before a class is unloaded. The class
  ///     * should not be referenced after this point. This callback only occurs if
  ///     * ClassLoading has been enabled for the class's module.
  ///     */
  ///
  ///    HRESULT UnloadClass([in] ICorDebugAppDomain *pAppDomain,
  ///                        [in] ICorDebugClass *c);
  ///
  ///    /*
  ///     * DebuggerError is called when an error occurs while attempting to
  ///     * handle an event from the Common Language Runtime. It is very strongly
  ///     * advised that debuggers log this message to the end user because
  ///     * this callback indicates the debugging services have been disabled due to
  ///     * an error.
  ///     *
  ///     * ICorDebugProcess::GetID() will be safe to call, but all other APIs should
  ///     * not be called and will fail if they are.
  ///     * This includes ICorDebugProcess::Terminate and ICorDebug  Process::Detach. The
  ///     * debugger should use OS facilities for terminating processes to shut down the process.
  ///     */
  ///    HRESULT DebuggerError([in] ICorDebugProcess *pProcess,
  ///                          [in] HRESULT errorHR,
  ///                          [in] DWORD errorCode);
  ///
  ///
  ///    /*
  ///     * Enum defining log message LoggingLevels
  ///     */
  ///    typedef enum LoggingLevelEnum
  ///    {
  ///        LTraceLevel0 = 0,
  ///        LTraceLevel1,
  ///        LTraceLevel2,
  ///        LTraceLevel3,
  ///        LTraceLevel4,
  ///        LStatusLevel0 = 20,
  ///        LStatusLevel1,
  ///        LStatusLevel2,
  ///        LStatusLevel3,
  ///        LStatusLevel4,
  ///        LWarningLevel = 40,
  ///        LErrorLevel = 50,
  ///        LPanicLevel = 100
  ///    } LoggingLevelEnum;
  ///
  ///
  ///    typedef enum LogSwitchCallReason
  ///    {
  ///        SWITCH_CREATE,
  ///        SWITCH_MODIFY,
  ///        SWITCH_DELETE
  ///    } LogSwitchCallReason;
  ///
  ///
  ///    /*
  ///     * LogMessage is called when a Common Language Runtime managed thread calls the Log
  ///     * class in the System.Diagnostics package to log an event.
  ///     */
  ///    HRESULT LogMessage([in] ICorDebugAppDomain *pAppDomain,
  ///                       [in] ICorDebugThread *pThread,
  ///                       [in] LONG lLevel,
  ///                       [in] WCHAR *pLogSwitchName,
  ///                       [in] WCHAR *pMessage);
  ///
  ///    /*
  ///     * LogSwitch is called when a Common Language Runtime managed thread calls the LogSwitch
  ///     * class in the System.Diagnostics package to create/modify a LogSwitch.
  ///     */
  ///    HRESULT LogSwitch([in] ICorDebugAppDomain *pAppDomain,
  ///                      [in] ICorDebugThread *pThread,
  ///                      [in] LONG lLevel,
  ///                      [in] ULONG ulReason,
  ///                      [in] WCHAR *pLogSwitchName,
  ///                      [in] WCHAR *pParentName);
  ///
  ///    /*
  ///     * CreateAppDomain is called when an app domain is created.
  ///     */
  ///    HRESULT CreateAppDomain([in] ICorDebugProcess *pProcess,
  ///                            [in] ICorDebugAppDomain *pAppDomain);
  ///
  ///    /*
  ///     * ExitAppDomain is called when an app domain exits.
  ///     */
  ///    HRESULT ExitAppDomain([in] ICorDebugProcess *pProcess,
  ///                          [in] ICorDebugAppDomain *pAppDomain);
  ///
  ///
  ///    /*
  ///     * LoadAssembly is called when a Common Language Runtime assembly is successfully
  ///     * loaded.
  ///     */
  ///    HRESULT LoadAssembly([in] ICorDebugAppDomain *pAppDomain,
  ///                         [in] ICorDebugAssembly *pAssembly);
  ///
  ///    /*
  ///     * UnloadAssembly is called when a Common Language Runtime assembly is unloaded. The assembly
  ///     * should not be used after this point.
  ///     */
  ///    HRESULT UnloadAssembly([in] ICorDebugAppDomain *pAppDomain,
  ///                           [in] ICorDebugAssembly *pAssembly);
  ///
  ///    /*
  ///     * ControlCTrap is called if a CTRL-C is trapped in the process being
  ///     * debugged. All appdomains within the process are stopped for
  ///     * this callback.
  ///     * Return values:
  ///     *      S_OK    : Debugger will handle the ControlC Trap
  ///     *      S_FALSE : Debugger won't handle the ControlC Trap
  ///     */
  ///    HRESULT ControlCTrap([in] ICorDebugProcess *pProcess);
  ///
  ///    /*
  ///     * NameChange() is called if either an AppDomain's or
  ///     * Thread's name changes.
  ///     */
  ///    HRESULT NameChange([in] ICorDebugAppDomain *pAppDomain,
  ///                       [in] ICorDebugThread *pThread);
  ///
  ///    /*
  ///     * UpdateModuleSymbols is called when PDB debug symbols are available for an 
  ///     * in-memory module. This is a debugger's chance to load the symbols 
  ///     * (using ISymUnmanagedBinder::GetReaderForStream), and bind source-level
  ///     * breakpoints for the module.
  ///     * 
  ///     * This callback is no longer dispatched for dynamic modules.  Instead,
  ///     * debuggers should call ICorDebugModule3::CreateReaderForInMemorySymbols
  ///     * to obtain a symbol reader for a dynamic module.  
  ///     */
  ///    HRESULT UpdateModuleSymbols([in] ICorDebugAppDomain *pAppDomain,
  ///                                [in] ICorDebugModule *pModule,
  ///                                [in] IStream *pSymbolStream);
  ///
  ///
  ///    /*
  ///     * DEPRECATED
  ///     */
  ///    HRESULT EditAndContinueRemap([in] ICorDebugAppDomain *pAppDomain,
  ///                                 [in] ICorDebugThread *pThread,
  ///                                 [in] ICorDebugFunction *pFunction,
  ///                                 [in] BOOL fAccurate);
  ///
  ///    /*
  ///     * BreakpointSetError is called if the CLR was unable to accuratley bind a breakpoint that
  ///     * was set before a function was JIT compiled. The given breakpoint will never be hit. The
  ///     * debugger should deactivate it and rebind it appropiatley.
  ///     */
  ///    HRESULT BreakpointSetError([in] ICorDebugAppDomain *pAppDomain,
  ///                               [in] ICorDebugThread *pThread,
  ///                               [in] ICorDebugBreakpoint *pBreakpoint,
  ///                               [in] DWORD dwError);
  ///};
  ///
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("3D6F5F60-7538-11D3-8D5B-00104B35E7EF")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugManagedCallback
    {
      /// <summary>
      /// Breakpoint is called when a breakpoint is hit.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="pBreakpoint"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Breakpoint ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugBreakpoint pBreakpoint);

      /// <summary>
      /// StepComplete is called when a step has completed.  The stepper
      /// may be used to continue stepping if desired (except for TERMINATE
      /// reasons.)
      /// 
      /// STEP_NORMAL means that stepping completed normally, in the same
      ///      function.
      /// 
      /// STEP_RETURN means that stepping continued normally, after the function
      ///      returned.
      /// 
      /// STEP_CALL means that stepping continued normally, at the start of
      ///      a newly called function.
      /// 
      /// STEP_EXCEPTION_FILTER means that control passed to an exception filter
      ///      after an exception was thrown.
      /// 
      /// STEP_EXCEPTION_HANDLER means that control passed to an exception handler
      ///      after an exception was thrown.
      /// 
      /// STEP_INTERCEPT means that control passed to an interceptor.
      /// 
      /// STEP_EXIT means that the thread exited before the step completed.
      ///      No more stepping can be performed with the stepper.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="pStepper"></param>
      /// <param name="reason"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 StepComplete ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugStepper pStepper, [In] CorDebugStepReason reason);

      /// <summary>
      ///  Break is called when a break opcode in the code stream is
      ///  executed.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="thread"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Break ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread thread);

      /// <summary>
      /// Exception is called when an exception is thrown from managed
      /// code, The specific exception can be retrieved from the thread object.
      /// 
      /// If unhandled is FALSE, this is a "first chance" exception that
      /// hasn't had a chance to be processed by the application.  If
      /// unhandled is TRUE, this is an unhandled exception which will
      /// terminate the process.
      /// 
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="unhandled"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Exception ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread, [In] Int32 unhandled);

      /// <summary>
      /// EvalComplete is called when an evaluation is completed.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="pEval"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EvalComplete ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugEval pEval);

      /// <summary>
      /// EvalException is called when an evaluation terminates with
      /// an unhandled exception.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="pEval"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EvalException ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugEval pEval);

      /// <summary>
      /// CreateProcess is called when a process is first attached to or
      /// started.
      /// 
      /// This entry point won't be called until the EE is initialized.
      /// Most of the ICorDebug API will return CORDBG_E_NOTREADY prior
      /// to the CreateProcess callback.
      /// </summary>
      /// <param name="pProcess"></param>
        [MethodImpl (MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)][PreserveSig]
        Int32 CreateProcess ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugProcess pProcess);

      /// <summary>
      /// ExitProcess is called when a process exits.
      /// 
      /// Note: you don't Continue from an ExitProcess event, and this
      /// event may fire asynchronously to other events, while the
      /// process appears to be stopped. This can occur if the process
      /// dies while stopped, usually due to some external force.
      /// 
      /// If the CLR is already dispatching a managed callback, this event
      /// will be delayed until after that callback has returned.
      /// 
      /// This is the only exit/unload event that is guaranteed to get called
      /// on shutdown.
      /// </summary>
      /// <param name="pProcess"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ExitProcess ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugProcess pProcess);

      /// <summary>
      /// CreateThread is called when a thread first begins executing managed
      /// code. The thread will be positioned immediately at the first
      /// managed code to be executed.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="thread"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateThread ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread thread);

      /// <summary>
      /// ExitThread is called when a thread which has run managed code exits. 
      /// Once this callback is fired, the thread no longer will appear in thread enumerations.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="thread"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ExitThread ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread thread);

      /// <summary>
      /// LoadModule is called when a Common Language Runtime module is successfully
      /// loaded. This is an appropriate time to examine metadata for the
      /// module, set JIT compiler flags, or enable or disable
      /// class loading callbacks for the module.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pModule"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LoadModule ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugModule pModule);

      /// <summary>
      /// UnloadModule is called when a Common Language Runtime module (DLL) is unloaded. The module
      /// should not be used after this point.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pModule"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 UnloadModule ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugModule pModule);

      /// <summary>
      /// LoadClass is called when a class finishes loading.  This callback only
      /// occurs if ClassLoading has been enabled for the class's module.
      /// 
      /// ClassLoading is always enabled for dynamic modules. This is a good time
      /// to update symbols (ICorDebugModule3::CreateReaderForInMemorySymbols) and
      /// bind breakpoints to newly generated classes in dynamic modules.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="c"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LoadClass ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugClass c);

      /// <summary>
      /// UnloadClass is called immediately before a class is unloaded. The class
      /// should not be referenced after this point. This callback only occurs if
      /// ClassLoading has been enabled for the class's module.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="c"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 UnloadClass ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugClass c);

      /// <summary>
      /// DebuggerError is called when an error occurs while attempting to
      /// handle an event from the Common Language Runtime. It is very strongly
      /// advised that debuggers log this message to the end user because
      /// this callback indicates the debugging services have been disabled due to
      /// an error.
      /// 
      /// ICorDebugProcess::GetID() will be safe to call, but all other APIs should
      /// not be called and will fail if they are.
      /// This includes ICorDebugProcess::Terminate and ICorDebug  Process::Detach. The
      /// debugger should use OS facilities for terminating processes to shut down the process.
      /// </summary>
      /// <param name="pProcess"></param>
      /// <param name="errorHR"></param>
      /// <param name="errorCode"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 DebuggerError ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugProcess pProcess,
            [MarshalAs (UnmanagedType.Error)][In] Int32 errorHR, [In] UInt32 errorCode);

      /// <summary>
      /// LogMessage is called when a Common Language Runtime managed thread calls the Log
      /// class in the System.Diagnostics package to log an event.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="lLevel"><see cref="LoggingLevelEnum"/></param>
      /// <param name="pLogSwitchName"></param>
      /// <param name="pMessage"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LogMessage ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread, [In] Int32 lLevel,
            [In] UInt16* pLogSwitchName, [In] UInt16* pMessage);

      /// <summary>
      /// LogSwitch is called when a Common Language Runtime managed thread calls the LogSwitch
      /// class in the System.Diagnostics package to create/modify a LogSwitch.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="lLevel"><see cref="LoggingLevelEnum"/></param>
      /// <param name="ulReason"><see cref="LogSwitchCallReason"/></param>
      /// <param name="pLogSwitchName"></param>
      /// <param name="pParentName"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LogSwitch ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread, [In] Int32 lLevel, [In] UInt32 ulReason,
            [In] UInt16* pLogSwitchName, [In] UInt16* pParentName);

      /// <summary>
      /// CreateAppDomain is called when an app domain is created.
      /// </summary>
      /// <param name="pProcess"></param>
      /// <param name="pAppDomain"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateAppDomain ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugProcess pProcess,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain);

      /// <summary>
      /// ExitAppDomain is called when an app domain exits.
      /// </summary>
      /// <param name="pProcess"></param>
      /// <param name="pAppDomain"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ExitAppDomain ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugProcess pProcess,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain);

      /// <summary>
      /// LoadAssembly is called when a Common Language Runtime assembly is successfully
      /// loaded.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pAssembly"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 LoadAssembly ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugAssembly pAssembly);

      /// <summary>
      /// UnloadAssembly is called when a Common Language Runtime assembly is unloaded. The assembly
      /// should not be used after this point.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pAssembly"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 UnloadAssembly ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugAssembly pAssembly);

      /// <summary>
      /// ControlCTrap is called if a CTRL-C is trapped in the process being
      /// debugged. All appdomains within the process are stopped for
      /// this callback.
      /// Return values:
      ///      S_OK    : Debugger will handle the ControlC Trap
      ///      S_FALSE : Debugger won't handle the ControlC Trap
      /// </summary>
      /// <param name="pProcess"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ControlCTrap ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugProcess pProcess);

      /// <summary>
      /// NameChange() is called if either an AppDomain's or
      /// Thread's name changes.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 NameChange ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread);

      /// <summary>
      /// UpdateModuleSymbols is called when PDB debug symbols are available for an 
      /// in-memory module. This is a debugger's chance to load the symbols 
      /// (using ISymUnmanagedBinder::GetReaderForStream), and bind source-level
      /// breakpoints for the module.
      /// 
      /// This callback is no longer dispatched for dynamic modules.  Instead,
      /// debuggers should call ICorDebugModule3::CreateReaderForInMemorySymbols
      /// to obtain a symbol reader for a dynamic module.  
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pModule"></param>
      /// <param name="pSymbolStream"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 UpdateModuleSymbols ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugModule pModule,
            [MarshalAs (UnmanagedType.Interface)][In] IStream pSymbolStream);

      /// <summary>
      /// DEPRECATED
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="pFunction"></param>
      /// <param name="fAccurate"></param>
      [Obsolete("DEPRECATED")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EditAndContinueRemap ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugFunction pFunction, [In] Int32 fAccurate);

      /// <summary>
      /// BreakpointSetError is called if the CLR was unable to accuratley bind a breakpoint that
      /// was set before a function was JIT compiled. The given breakpoint will never be hit. The
      /// debugger should deactivate it and rebind it appropiatley.
      /// </summary>
      /// <param name="pAppDomain"></param>
      /// <param name="pThread"></param>
      /// <param name="pBreakpoint"></param>
      /// <param name="dwError"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 BreakpointSetError ([MarshalAs (UnmanagedType.Interface)][In] ICorDebugAppDomain pAppDomain,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugThread pThread,
            [MarshalAs (UnmanagedType.Interface)][In] ICorDebugBreakpoint pBreakpoint, [In] UInt32 dwError);
    }
}