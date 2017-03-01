using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  The new V3.0 stackwalking API.
  /// </summary>
  /// <example><code>
  ///  
  ///  * The new V3.0 stackwalking API.
  ///  */
  /// [
  ///     object,
  ///     local,
  ///     uuid(A0647DE9-55DE-4816-929C-385271C64CF7),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugStackWalk : IUnknown
  /// {
  ///     typedef enum CorDebugSetContextFlag
  ///     {
  ///         SET_CONTEXT_FLAG_ACTIVE_FRAME = 0x1,
  ///         SET_CONTEXT_FLAG_UNWIND_FRAME = 0x2,
  ///     } CorDebugSetContextFlag;
  /// 
  ///     /* 
  ///      * Get the current context of this stack frame.
  ///      * 
  ///      * The CONTEXT is retrieved from the ICorDebugStackWalk.  As unwinding may only restore a subset of the 
  ///      * registers, such as only non-volatile registers, the context may not exactly match the register state at 
  ///      * the time of the actual call.
  ///      */
  ///     HRESULT GetContext([in] ULONG32 contextFlags,
  ///                        [in] ULONG32 contextBufSize,
  ///                        [out] ULONG32* contextSize,
  ///                        [out, size_is(contextBufSize)] BYTE contextBuf[]);
  /// 
  ///     /* 
  ///      * Change the current context of this stack walk, allowing the
  ///      * debugger to move it to an arbitrary context. Does not actually
  ///      * alter the current context of the thread whose stack is being walked.
  ///      *
  ///      * The CONTEXT has to be a valid CONTEXT of a stack frame on the thread.
  ///      * If the CONTEXT is outside of the current thread's stack range, we'll
  ///      * return a failure HRESULT.  Otherwise, in the case of an invalid CONTEXT, 
  ///      * the result is undefined.
  ///      */
  ///     HRESULT SetContext([in] CorDebugSetContextFlag flag,
  ///                        [in] ULONG32 contextSize,
  ///                        [in, size_is(contextSize)] BYTE context[]);
  /// 
  ///     /*
  ///      * Attempt to advance the stackwalk to the next frame.  
  ///      * If the current frame type is a native stack frame, Next() will not advance to the caller frame.
  ///      * Instead, Next() will advance to the next managed stack frame or the next internal frame marker.
  ///      *
  ///      * If a debugger wants to unwind unmanaged stack frames, it needs to start from the 
  ///      * native stack frame itself.  It can seed the unwind by calling GetContext().
  ///      *
  ///      * This function will return CORDBG_S_AT_END_OF_STACK when there are no more frames.
  ///      */
  ///     HRESULT Next();
  /// 
  ///     /*
  ///      * Return the current frame.  If the stackwalker is stopped at a native stack frame, we will return S_FALSE
  ///      * and set pFrame to NULL.
  ///      */
  ///     HRESULT GetFrame([out] ICorDebugFrame ** pFrame);
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("A0647DE9-55DE-4816-929C-385271C64CF7")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugStackWalk
  {
    /// <summary>
    /// Get the current context of this stack frame.
    /// The CONTEXT is retrieved from the ICorDebugStackWalk.  As unwinding may only restore a subset of the
    /// registers, such as only non-volatile registers, the context may not exactly match the register state at
    /// the time of the actual call.
    /// </summary>
    /// <param name="contextFlags"><see cref="CorDebugSetContextFlag" /></param>
    /// <param name="contextBufSize"></param>
    /// <param name="contextSize"></param>
    /// <param name="contextBuf"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetContext ([In] UInt32 contextFlags, [In] UInt32 contextBufSize, UInt32* contextSize, Byte* contextBuf);

    /// <summary>
    /// Change the current context of this stack walk, allowing the
    /// debugger to move it to an arbitrary context. Does not actually
    /// alter the current context of the thread whose stack is being walked.
    /// The CONTEXT has to be a valid CONTEXT of a stack frame on the thread.
    /// If the CONTEXT is outside of the current thread's stack range, we'll
    /// return a failure HRESULT.  Otherwise, in the case of an invalid CONTEXT,
    /// the result is undefined.
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="contextSize"></param>
    /// <param name="context"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetContext ([In] CorDebugSetContextFlag flag, [In] UInt32 contextSize, [In] Byte* context);

    /// <summary>
    /// Attempt to advance the stackwalk to the next frame.
    /// If the current frame type is a native stack frame, Next() will not advance to the caller frame.
    /// Instead, Next() will advance to the next managed stack frame or the next internal frame marker.
    /// If a debugger wants to unwind unmanaged stack frames, it needs to start from the
    /// native stack frame itself.  It can seed the unwind by calling GetContext().
    /// This function will return CORDBG_S_AT_END_OF_STACK when there are no more frames.
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Next ();

    /// <summary>
    /// Return the current frame.  If the stackwalker is stopped at a native stack frame, we will return S_FALSE and set pFrame to NULL.
    /// </summary>
    /// <param name="pFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFrame ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame pFrame);
  }
}