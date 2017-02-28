using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(CC7BCAEA-8A68-11d2-983C-0000F808342D),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugModuleBreakpoint : ICorDebugBreakpoint
  ///{
  ///    /*
  ///     * Returns the module on which this breakpoint is set.
  ///     */
  ///    HRESULT GetModule([out] ICorDebugModule **ppModule);
  ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCAEA-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    public unsafe interface ICorDebugModuleBreakpoint : ICorDebugBreakpoint
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
      /// Returns the module on which this breakpoint is set.
      /// </summary>
      /// <param name="ppModule"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetModule ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModule ppModule);
    }
}