﻿using System.Runtime.CompilerServices;
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
  ///    uuid(CC7BCAEB-8A68-11d2-983C-0000F808342D),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugValueBreakpoint : ICorDebugBreakpoint
  ///{
  ///    /*
  ///     * Gets the value on which this breakpoint is set.
  ///     */
  ///    HRESULT GetValue([out] ICorDebugValue **ppValue);
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCAEB-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    public unsafe interface ICorDebugValueBreakpoint : ICorDebugBreakpoint
    {
      /// <summary>
      /// Sets the active state of the breakpoint.
      /// </summary>
      /// <param name="bActive"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      new void Activate ([In] int bActive);

      /// <summary>
      /// Returns whether the breakpoint is active.
      /// </summary>
      /// <param name="pbActive"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      new void IsActive ([Out] int* pbActive);

      /// <summary>
      /// Gets the value on which this breakpoint is set.
      /// </summary>
      /// <param name="ppValue"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetValue ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);
    }
}