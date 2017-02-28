using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// [
  ///    object,
  ///    local,
  ///    uuid(5263E909-8CB5-11d3-BD2F-0000F80849BD),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugUnmanagedCallback : IUnknown
  ///{
  ///    /*
  ///     * DebugEvent is called when a DEBUG_EVENT is received which is
  ///     * not directly related to the Common Language Runtime.
  ///     *
  ///     * DO NOT USE any parts of the debugging API from the Win32 Event
  ///     * Thread. Only ICorDebugController::Continue() can be called on
  ///     * the Win32 Event Thread, and only when continuing from an out-of-band
  ///     * event.
  ///     *
  ///     * This callback is an exception to the rules about callbacks.
  ///     * When this callback is called, the process will be in the "raw"
  ///     * OS debug stopped state. The process will not be synchronized.
  ///     * The process will automatically enter the synchronized state when
  ///     * necessary to satisfy certain requests for information about
  ///     * managed code. (Note that this may result in other nested
  ///     * DebugEvent callbacks.)
  ///     *
  ///     * Call ClearCurrentException on the process to ignore an
  ///     * exception event before continuing the process. (Causes
  ///     * DBG_CONTINUE to be sent on continue rather than
  ///     * DBG_EXCEPTION_NOT_HANDLED)
  ///     * Out-of-band Breakpoint and single-step exceptions are automatically cleared.
  ///     *
  ///     * fOutOfBand will be FALSE if the debugging services support
  ///     * interaction with the process's managed state while the process
  ///     * is stopped due to this event. fOutOfBand will be TRUE if
  ///     * interaction with the process's managed state is impossible until
  ///     * the unmanaged event is continued from.
  ///     *
  ///     * Out-Of-Band events can come at anytime; even when there debuggee appears stopped
  ///     * and even when there's already an outstanding inband event.
  ///     *
  ///     * In v2.0, it is strongly recommended that the debugger just immediately
  ///     * continues OOB breakpoint events. The debugger should be using the ICorDebugProcess2
  ///     * SetUnmanagedBreakpoint and ClearUnmanagedBreakpoint APIs to add/remove breakpoints.
  ///     * Those APIs will already skip over any OOB breakpoints automatically. Thus the only
  ///     * oob breakpoints that get dispatched should be raw breakpoints already in the
  ///     * instruction stream (eg, like a call to kernel32!DebugBreak). In these cases,
  ///     * just continuing past the breakpoint is the correct thing to do. Do not try to use
  ///     * any other portion of the API like ClearCurrentException or Get/SetThreadContext.
  ///     *
  ///     */
  ///
  ///    HRESULT DebugEvent([in] LPDEBUG_EVENT pDebugEvent,
  ///                       [in] BOOL fOutOfBand);
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("5263E909-8CB5-11D3-BD2F-0000F80849BD")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugUnmanagedCallback
    {
      /// <summary>
      /// DebugEvent is called when a DEBUG_EVENT is received which is
      /// not directly related to the Common Language Runtime.
      /// 
      /// DO NOT USE any parts of the debugging API from the Win32 Event
      /// Thread. Only ICorDebugController::Continue() can be called on
      /// the Win32 Event Thread, and only when continuing from an out-of-band
      /// event.
      /// 
      /// This callback is an exception to the rules about callbacks.
      /// When this callback is called, the process will be in the "raw"
      /// OS debug stopped state. The process will not be synchronized.
      /// The process will automatically enter the synchronized state when
      /// necessary to satisfy certain requests for information about
      /// managed code. (Note that this may result in other nested
      /// DebugEvent callbacks.)
      /// 
      /// Call ClearCurrentException on the process to ignore an
      /// exception event before continuing the process. (Causes
      /// DBG_CONTINUE to be sent on continue rather than
      /// DBG_EXCEPTION_NOT_HANDLED)
      /// Out-of-band Breakpoint and single-step exceptions are automatically cleared.
      /// 
      /// fOutOfBand will be FALSE if the debugging services support
      /// interaction with the process's managed state while the process
      /// is stopped due to this event. fOutOfBand will be TRUE if
      /// interaction with the process's managed state is impossible until
      /// the unmanaged event is continued from.
      /// 
      /// Out-Of-Band events can come at anytime; even when there debuggee appears stopped
      /// and even when there's already an outstanding inband event.
      /// 
      /// In v2.0, it is strongly recommended that the debugger just immediately
      /// continues OOB breakpoint events. The debugger should be using the ICorDebugProcess2
      /// SetUnmanagedBreakpoint and ClearUnmanagedBreakpoint APIs to add/remove breakpoints.
      /// Those APIs will already skip over any OOB breakpoints automatically. Thus the only
      /// oob breakpoints that get dispatched should be raw breakpoints already in the
      /// instruction stream (eg, like a call to kernel32!DebugBreak). In these cases,
      /// just continuing past the breakpoint is the correct thing to do. Do not try to use
      /// any other portion of the API like ClearCurrentException or Get/SetThreadContext.
      /// </summary>
      /// <param name="pDebugEvent"></param>
      /// <param name="fOutOfBand"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void DebugEvent ([ComAliasName ("LPDEBUG_EVENT"), In] void* pDebugEvent, [In] Int32 fOutOfBand);
    }
}