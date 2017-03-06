using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  ///[
  ///    object,
  ///    local,
  ///    uuid(CC7BCAE9-8A68-11d2-983C-0000F808342D),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugFunctionBreakpoint : ICorDebugBreakpoint
  ///{
  ///    /*
  ///     * Returns the function on which this breakpoint is set
  ///     */
  ///    HRESULT GetFunction([out] ICorDebugFunction **ppFunction);
  ///
  ///    /*
  ///     * Returns the offset of this breakpoint within the function
  ///     */
  ///    HRESULT GetOffset([out] ULONG32 *pnOffset);
  ///}; </code></example>
    [Guid ("CC7BCAE9-8A68-11D2-983C-0000F808342D")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public unsafe interface ICorDebugFunctionBreakpoint : ICorDebugBreakpoint
    {
      /// <summary>
      /// Sets the active state of the breakpoint.
      /// </summary>
      /// <param name="bActive"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Activate ([In] Int32 bActive);

      /// <summary>
      /// Returns whether the breakpoint is active.
      /// </summary>
      /// <param name="pbActive"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsActive ([Out] Int32 *pbActive);

      /// <summary>
      /// Returns the function on which this breakpoint is set.
      /// </summary>
      /// <param name="ppFunction"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

      /// <summary>
      /// Returns the offset of this breakpoint within the function.
      /// </summary>
      /// <param name="pnOffset"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetOffset ([Out] UInt32* pnOffset);
    }
}