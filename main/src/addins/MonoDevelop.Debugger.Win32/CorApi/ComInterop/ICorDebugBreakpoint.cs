using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugBreakpoint represents a breakpoint; either a breakpoint
  /// set in a function, or a watchpoint set on a value.
  /// 
  /// Note that breakpoints have no direct support for condition
  /// expressions.  The debugger must implement this functionality on top of
  /// this interface if desired.
  /// </summary>
  /// <example><code>
  ////* ------------------------------------------------------------------------- *
  /// * Breakpoint interface
  /// * ------------------------------------------------------------------------- */
  ///
  ////*
  /// * ICorDebugBreakpoint represents a breakpoint; either a breakpoint
  /// * set in a function, or a watchpoint set on a value.
  /// *
  /// * Note that breakpoints have no direct support for condition
  /// * expressions.  The debugger must implement this functionality on top of
  /// * this interface if desired.
  /// *
  /// */
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(CC7BCAE8-8A68-11d2-983C-0000F808342D),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugBreakpoint : IUnknown
  ///{
  ///    /*
  ///     * Sets the active state of the breakpoint
  ///     */
  ///    HRESULT Activate([in] BOOL bActive);
  ///
  ///    /*
  ///     * Returns whether the breakpoint is active.
  ///     */
  ///    HRESULT IsActive([out] BOOL *pbActive);
  ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCAE8-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugBreakpoint
    {
      /// <summary>
      /// Sets the active state of the breakpoint.
      /// </summary>
      /// <param name="bActive"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Activate ([In] Int32 bActive);

      /// <summary>
      /// Returns whether the breakpoint is active.
      /// </summary>
      /// <param name="pbActive"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsActive ([Out] Int32 *pbActive);
    }
}