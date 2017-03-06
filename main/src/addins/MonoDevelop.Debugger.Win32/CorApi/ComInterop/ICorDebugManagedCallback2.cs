using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugManagedCallback2 is a logical extension to ICorDebugManagedCallback.
  ///  This handles new debug events introduced in V2.0. A debugger's callback object
  ///  to ICorDebug::SetManagedHandler must implement this interface if it is debugging v2.0 apps.
  /// </summary>
  /// <example><code>
  ///  
  ///   * ICorDebugManagedCallback2 is a logical extension to ICorDebugManagedCallback.
  ///   * This handles new debug events introduced in V2.0. A debugger's callback object
  ///   * to ICorDebug::SetManagedHandler must implement this interface if it is debugging v2.0 apps.
  ///   */
  /// [
  ///     object,
  ///     local,
  ///     uuid(250E5EEA-DB5C-4C76-B6F3-8C46F12E3203),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugManagedCallback2 : IUnknown
  /// {
  /// 
  ///    /*
  ///      * FunctionRemapOpportunity is fired whenever execution reaches a sequence point in an older version
  ///      * of an edited function. This event gives the debugger an opportunity to remap the IP to its proper
  ///      * place in the new version by calling ICorDebugILFrame2::RemapFunction. If the debugger does not call
  ///      * RemapFunction before calling Continue, the runtime will continue executing the old code and will
  ///      * fire another FunctionRemapOpportunity callback at the next sequence point
  ///      */
  ///     HRESULT FunctionRemapOpportunity([in] ICorDebugAppDomain *pAppDomain,
  ///                                      [in] ICorDebugThread *pThread,
  ///                                      [in] ICorDebugFunction *pOldFunction,
  ///                                      [in] ICorDebugFunction *pNewFunction,
  ///                                      [in] ULONG32 oldILOffset);
  /// 
  ///     /*
  ///      * CreateConnection is called when a new connection is created.
  ///      */
  ///     HRESULT CreateConnection([in] ICorDebugProcess *pProcess,
  ///                              [in] CONNID dwConnectionId,
  ///                              [in] WCHAR *pConnName);
  /// 
  ///     /*
  ///      * ChangeConnection is called when a connection's set of tasks changes.
  ///      */
  ///     HRESULT ChangeConnection([in] ICorDebugProcess *pProcess,
  ///                              [in] CONNID dwConnectionId );
  /// 
  ///     /*
  ///      * DestroyConnection is called when a connection is ended.
  ///      */
  ///     HRESULT DestroyConnection([in] ICorDebugProcess *pProcess,
  ///                               [in] CONNID dwConnectionId );
  /// 
  /// 
  /// 
  /// 
  /// 
  /// 
  ///     typedef enum CorDebugExceptionCallbackType
  ///     {
  ///         DEBUG_EXCEPTION_FIRST_CHANCE = 1,        /* Fired when exception thrown */
  ///         DEBUG_EXCEPTION_USER_FIRST_CHANCE = 2,   /* Fired when search reaches first user code */
  ///         DEBUG_EXCEPTION_CATCH_HANDLER_FOUND = 3, /* Fired if &amp; when search finds a handler */
  ///         DEBUG_EXCEPTION_UNHANDLED = 4            /* Fired if search doesnt find a handler */
  ///     } CorDebugExceptionCallbackType;
  /// 
  /// 
  ///     typedef enum CorDebugExceptionFlags
  ///     {
  ///         DEBUG_EXCEPTION_NONE = 0,
  ///         DEBUG_EXCEPTION_CAN_BE_INTERCEPTED = 0x0001 /* Indicates interceptable exception */
  ///     } CorDebugExceptionFlags;
  /// 
  /// 
  ///     /*
  ///      * Exception is called at various points during the search phase of the
  ///      * exception-handling process.  The exception being processed can be
  ///      * retrieved from the ICorDebugThread.
  ///      */
  ///     HRESULT Exception( [in] ICorDebugAppDomain *pAppDomain,
  ///                        [in] ICorDebugThread *pThread,
  ///                        [in] ICorDebugFrame *pFrame,
  ///                        [in] ULONG32 nOffset,
  ///                        [in] CorDebugExceptionCallbackType dwEventType,
  ///                        [in] DWORD dwFlags );
  /// 
  /// 
  ///     typedef enum CorDebugExceptionUnwindCallbackType
  ///     {
  ///         DEBUG_EXCEPTION_UNWIND_BEGIN = 1, /* Fired at the beginning of the unwind */
  ///         DEBUG_EXCEPTION_INTERCEPTED = 2   /* Fired after an exception has been intercepted */
  ///     } CorDebugExceptionUnwindCallbackType;
  /// 
  /// 
  /// 
  ///     /*
  ///      * For non-intercepted exceptions, ExceptionUnwind is called at the beginning of the second pass 
  ///      * when we start to unwind the stack.  For intercepted exceptions, ExceptionUnwind is called when
  ///      * the interception is complete, conceptually at the end of the second pass.
  ///      *
  ///      * dwFlags is not currently used.
  ///      */
  ///     HRESULT ExceptionUnwind( [in] ICorDebugAppDomain *pAppDomain,
  ///                              [in] ICorDebugThread *pThread,
  ///                              [in] CorDebugExceptionUnwindCallbackType dwEventType,
  ///                              [in] DWORD dwFlags );
  /// 
  ///     /*
  ///      * FunctionRemapComplete is fired whenever execution has completed switching over to a
  ///      * new version of an edited function (as requested by a call to ICorDebugILFrame2::RemapFunction).
  ///      * At this point (and no sooner) steppers can be added to that new version of the function.
  ///      */
  ///     HRESULT FunctionRemapComplete([in] ICorDebugAppDomain *pAppDomain,
  ///                                                                     [in] ICorDebugThread *pThread,
  ///                                                                     [in] ICorDebugFunction *pFunction);
  /// 
  ///     // Notification that an Managed Debug Assistant (MDA) was hit in the debuggee process.
  ///     // - MDAs are heuristic warnings and do not require any explicit debugger action (other than continue, of course) for proper functionality.
  ///     // - The CLR can change what MDAs are fired (and what data is in any given MDA) at any point.
  ///     // - Therefore, debuggers should not build any specific functionality requiring specific MDAs patterns.
  ///     // - MDAs may be queued and fired "after the fact". This could happen if the runtime needs to slip from when an
  ///     //   MDA occurs to get to a safe point for firing it. It also means the runtime reserves the right to fire a bunch of MDAs
  ///     //   in a single set of callback queue (similar for what we do w/ attach events).
  ///     //
  ///     // See the MDA documentation for how to enable / disable notifications.
  ///     //
  ///     // Parameters:
  ///     // - pController is the controller object (process or appdomain) that the MDA occurred in.
  ///     //     Clients should not make any assumption about whether the controller is a process or appdomain (though they can
  ///     //     always QI to find out).
  ///     //     Call continue on this to resume the debuggee.
  ///     // - pThread - managed thread on which the debug event occurred. If the MDA occurred on an unmanaged thread then
  ///     //     this will be null. Get the OS thread ID from the MDA object itself.
  ///     // - pMDA is an object containing MDA information.
  ///     //    Suggested usage is that the client does not keep a reference to the MDA object after returning from this callback
  ///     //    because that lets the CLR quickly recycle the MDA's memory. This could be a performance win if there are
  ///     //    lots of MDAs firing.
  ///     HRESULT MDANotification(
  ///         [in] ICorDebugController * pController,
  ///         [in] ICorDebugThread *pThread,
  ///         [in] ICorDebugMDA * pMDA
  ///     );
  /// 
  /// };
  ///  </code></example>
  [Guid ("250E5EEA-DB5C-4C76-B6F3-8C46F12E3203")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugManagedCallback2
  {
    /// <summary>
    /// FunctionRemapOpportunity is fired whenever execution reaches a sequence point in an older version
    /// of an edited function. This event gives the debugger an opportunity to remap the IP to its proper
    /// place in the new version by calling ICorDebugILFrame2::RemapFunction. If the debugger does not call
    /// RemapFunction before calling Continue, the runtime will continue executing the old code and will
    /// fire another FunctionRemapOpportunity callback at the next sequence point
    /// </summary>
    /// <param name="pAppDomain"></param>
    /// <param name="pThread"></param>
    /// <param name="pOldFunction"></param>
    /// <param name="pNewFunction"></param>
    /// <param name="oldILOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 FunctionRemapOpportunity ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugAppDomain pAppDomain, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugThread pThread, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pOldFunction, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pNewFunction, [In] UInt32 oldILOffset);

    /// <summary>
    /// CreateConnection is called when a new connection is created.
    /// </summary>
    /// <param name="pProcess"></param>
    /// <param name="dwConnectionId"></param>
    /// <param name="pConnName"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateConnection ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugProcess pProcess, [In] [ComAliasName ("CONNID")] UInt32 dwConnectionId, [In] UInt16* pConnName);

    /// <summary>
    /// ChangeConnection is called when a connection's set of tasks changes.
    /// </summary>
    /// <param name="pProcess"></param>
    /// <param name="dwConnectionId"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ChangeConnection ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugProcess pProcess, [In] [ComAliasName ("CONNID")] UInt32 dwConnectionId);

    /// <summary>
    /// DestroyConnection is called when a connection is ended.
    /// </summary>
    /// <param name="pProcess"></param>
    /// <param name="dwConnectionId"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 DestroyConnection ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugProcess pProcess, [In] [ComAliasName ("CONNID")] UInt32 dwConnectionId);

    /// <summary>
    /// Exception is called at various points during the search phase of the
    /// exception-handling process.  The exception being processed can be
    /// retrieved from the ICorDebugThread.
    /// </summary>
    /// <param name="pAppDomain"></param>
    /// <param name="pThread"></param>
    /// <param name="pFrame"></param>
    /// <param name="nOffset"></param>
    /// <param name="dwEventType"></param>
    /// <param name="dwFlags"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Exception ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugAppDomain pAppDomain, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugThread pThread, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFrame pFrame, [In] UInt32 nOffset, [In] CorDebugExceptionCallbackType dwEventType, [In] UInt32 dwFlags);

    /// <summary>
    /// For non-intercepted exceptions, ExceptionUnwind is called at the beginning of the second pass
    /// when we start to unwind the stack.  For intercepted exceptions, ExceptionUnwind is called when
    /// the interception is complete, conceptually at the end of the second pass.
    /// dwFlags is not currently used.
    /// </summary>
    /// <param name="pAppDomain"></param>
    /// <param name="pThread"></param>
    /// <param name="dwEventType"></param>
    /// <param name="dwFlags"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ExceptionUnwind ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugAppDomain pAppDomain, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugThread pThread, [In] CorDebugExceptionUnwindCallbackType dwEventType, [In] UInt32 dwFlags);

    /// <summary>
    /// FunctionRemapComplete is fired whenever execution has completed switching over to a
    /// new version of an edited function (as requested by a call to ICorDebugILFrame2::RemapFunction).
    /// At this point (and no sooner) steppers can be added to that new version of the function.
    /// </summary>
    /// <param name="pAppDomain"></param>
    /// <param name="pThread"></param>
    /// <param name="pFunction"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 FunctionRemapComplete ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugAppDomain pAppDomain, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugThread pThread, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pFunction);

    /// <summary>
    ///  Notification that an Managed Debug Assistant (MDA) was hit in the debuggee process.
    ///  - MDAs are heuristic warnings and do not require any explicit debugger action (other than continue, of course) for proper functionality.
    ///  - The CLR can change what MDAs are fired (and what data is in any given MDA) at any point.
    ///  - Therefore, debuggers should not build any specific functionality requiring specific MDAs patterns.
    ///  - MDAs may be queued and fired "after the fact". This could happen if the runtime needs to slip from when an
    ///    MDA occurs to get to a safe point for firing it. It also means the runtime reserves the right to fire a bunch of MDAs
    ///    in a single set of callback queue (similar for what we do w/ attach events).
    ///  See the MDA documentation for how to enable / disable notifications.
    ///  Parameters:
    ///  - pController is the controller object (process or appdomain) that the MDA occurred in.
    ///      Clients should not make any assumption about whether the controller is a process or appdomain (though they can
    ///      always QI to find out).
    ///      Call continue on this to resume the debuggee.
    ///  - pThread - managed thread on which the debug event occurred. If the MDA occurred on an unmanaged thread then
    ///      this will be null. Get the OS thread ID from the MDA object itself.
    ///  - pMDA is an object containing MDA information.
    ///     Suggested usage is that the client does not keep a reference to the MDA object after returning from this callback
    ///     because that lets the CLR quickly recycle the MDA's memory. This could be a performance win if there are
    ///     lots of MDAs firing.
    /// </summary>
    /// <param name="pController"></param>
    /// <param name="pThread"></param>
    /// <param name="pMDA"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 MDANotification ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugController pController, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugThread pThread, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugMDA pMDA);
  }
}