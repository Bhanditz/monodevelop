using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  [
  ///     object,
  ///     local,
  ///     uuid(AD1B3588-0EF0-4744-A496-AA09A9F80371),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugProcess2 : IUnknown
  /// {
  /// 
  ///     /*
  ///       * Return a ICorDebugThread2 interface given a TASKID
  ///      * Host can set TASKID using ICLRTask::SetTaskIdentifier
  ///       */
  ///     HRESULT GetThreadForTaskID(
  ///        [in] TASKID taskid,
  ///        [out] ICorDebugThread2 **ppThread);
  /// 
  /// 
  ///     /*
  ///      * Returns the version of the runtime the debugee process is running.
  ///      */
  ///     HRESULT GetVersion([out] COR_VERSION* version);
  /// 
  ///     /*
  ///      * Set an unmanaged breakpoint at the given native address. If the address is within
  ///      * the runtime, the breakpoint will be ignored.
  ///      * This allows the CLR to avoid dispatching out-of-band breakpoints for breakpoints
  ///      * set by the debugger.
  ///      * buffer[] returns the opcode at the address that is replaced by the breakpoint.
  ///      */
  /// 
  ///     HRESULT SetUnmanagedBreakpoint([in] CORDB_ADDRESS address,
  ///                                    [in] ULONG32 bufsize,
  ///                                    [out, size_is(bufsize), length_is(*bufLen)] BYTE buffer[],
  ///                                    [out] ULONG32 * bufLen);
  /// 
  ///     /*
  ///      * Remove a breakpoint set by SetUnmanagedBreakpoint.
  ///      */
  ///     HRESULT ClearUnmanagedBreakpoint([in] CORDB_ADDRESS address);
  /// 
  /// 
  ///     /*
  ///      * SetDesiredNGENCompilerFlags specifies the set of flags that must be set in a pre-JITted
  ///      * image in order for the runtime to load that image into this app domain. If no such image exists,
  ///      * the runtime will load the IL and JIT instead. The flags set by this function are just used to select the
  ///      * correct pre-JITted image; if no suitable image is found the debugger will still need to use
  ///      * ICorDebugModule2::SetJITCompilerFlags to set the flags as desired for JIT.
  ///      *
  ///      * This function must be called during the CreateProcess callback.
  ///      * Attempts to call it after this callback has been delivered will fail.
  ///      */
  ///      HRESULT SetDesiredNGENCompilerFlags( [in] DWORD pdwFlags );
  /// 
  ///     /*
  ///      * GetDesiredNGENCompilerFlags gets the set of flags that must be set in a pre-JITted image in order
  ///      * for the runtime to load that image into this process.
  ///      */
  ///      HRESULT GetDesiredNGENCompilerFlags( [out] DWORD *pdwFlags );
  /// 
  /// 
  ///     /*
  ///     * Gets an ICorDebugReferenceValue object from a raw GC handle value.
  ///     *
  ///     * handle is the IntPtr within a GCHandle. Do not confuse
  ///     * this with a GC reference value. This is a potentially dangerous API and may
  ///     * corrupt both the debugger and debuggee if a bogus handle is passed in.
  ///     * This API does not necessarily validate that the handle is valid.
  ///     *
  ///     * The ICorDebugReferenceValue will behave much like a normal reference. It will
  ///     * be neutered on the next continue; the lifetime of the target object will
  ///     * not be affected by the existence of the ReferenceValue.
  ///     */
  ///     HRESULT GetReferenceValueFromGCHandle( [in] UINT_PTR handle,
  ///                                            [out] ICorDebugReferenceValue **pOutValue);
  /// 
  /// };
  ///  </code></example>
  [Guid ("AD1B3588-0EF0-4744-A496-AA09A9F80371")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugProcess2
  {
    /// <summary>
    /// Return a ICorDebugThread2 interface given a TASKID
    /// Host can set TASKID using ICLRTask::SetTaskIdentifier
    /// </summary>
    /// <param name="taskid"></param>
    /// <param name="ppThread"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetThreadForTaskID ([In] [ComAliasName ("TASKID")] UInt64 taskid, [MarshalAs (UnmanagedType.Interface)] out ICorDebugThread2 ppThread);

    /// <summary>
    /// Returns the version of the runtime the debugee process is running.
    /// </summary>
    /// <param name="version"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetVersion (COR_VERSION* version);

    /// <summary>
    /// Set an unmanaged breakpoint at the given native address. If the address is within
    /// the runtime, the breakpoint will be ignored.
    /// This allows the CLR to avoid dispatching out-of-band breakpoints for breakpoints
    /// set by the debugger.
    /// buffer[] returns the opcode at the address that is replaced by the breakpoint.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="bufsize"></param>
    /// <param name="buffer"></param>
    /// <param name="bufLen"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetUnmanagedBreakpoint ([In] [ComAliasName ("CORDB_ADDRESS")] UInt64 address, [In] UInt32 bufsize, Byte* buffer, UInt32* bufLen);

    /// <summary>
    /// Remove a breakpoint set by SetUnmanagedBreakpoint.
    /// </summary>
    /// <param name="address"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ClearUnmanagedBreakpoint ([In] [ComAliasName ("CORDB_ADDRESS")] UInt64 address);

    /// <summary>
    /// SetDesiredNGENCompilerFlags specifies the set of flags that must be set in a pre-JITted
    /// image in order for the runtime to load that image into this app domain. If no such image exists,
    /// the runtime will load the IL and JIT instead. The flags set by this function are just used to select the
    /// correct pre-JITted image; if no suitable image is found the debugger will still need to use
    /// ICorDebugModule2::SetJITCompilerFlags to set the flags as desired for JIT.
    /// This function must be called during the CreateProcess callback.
    /// Attempts to call it after this callback has been delivered will fail.
    /// </summary>
    /// <param name="pdwFlags"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetDesiredNGENCompilerFlags ([In] UInt32 pdwFlags);

    /// <summary>
    ///  GetDesiredNGENCompilerFlags gets the set of flags that must be set in a pre-JITted image in order
    ///  for the runtime to load that image into this process.
    /// </summary>
    /// <param name="pdwFlags"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetDesiredNGENCompilerFlags (UInt32* pdwFlags);

    /// <summary>
    /// Gets an ICorDebugReferenceValue object from a raw GC handle value.
    /// handle is the IntPtr within a GCHandle. Do not confuse
    /// this with a GC reference value. This is a potentially dangerous API and may
    /// corrupt both the debugger and debuggee if a bogus handle is passed in.
    /// This API does not necessarily validate that the handle is valid.
    /// The ICorDebugReferenceValue will behave much like a normal reference. It will
    /// be neutered on the next continue; the lifetime of the target object will
    /// not be affected by the existence of the ReferenceValue.
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="pOutValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetReferenceValueFromGCHandle ([ComAliasName ("UINT_PTR")] [In] void* handle, [MarshalAs (UnmanagedType.Interface)] out ICorDebugReferenceValue pOutValue);
  }
}