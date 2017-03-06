using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  
  /// [
  ///     object,
  ///     local,
  ///     uuid(B92CC7F7-9D2D-45c4-BC2B-621FCC9DFBF4),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugInternalFrame : ICorDebugFrame
  /// {
  /// 
  ///     typedef enum CorDebugInternalFrameType
  ///     {
  ///         // This is a 'null' value for GetFrameType and is included for completeness sake.
  ///         // ICorDebugInternalFrame::GetFrameType() should never actually return this.
  ///         STUBFRAME_NONE = 0x00000000,
  /// 
  ///         // This frame is a M2U stub-frame. This could include both PInvoke
  ///         // and COM-interop calls.
  ///         STUBFRAME_M2U = 0x0000001,
  /// 
  ///         // This is a U2M stub frame.
  ///         STUBFRAME_U2M = 0x0000002,
  /// 
  ///         // AppDomain transition.
  ///         STUBFRAME_APPDOMAIN_TRANSITION = 0x00000003,
  /// 
  ///         // LightWeight method calls.
  ///         STUBFRAME_LIGHTWEIGHT_FUNCTION = 0x00000004,
  /// 
  ///         // Start of Func-eval. This is included for CHF callbacks.
  ///         // Funcevals also have a chain CHAIN_FUNC_EVAL (legacy from v1.0)
  ///         STUBFRAME_FUNC_EVAL = 0x00000005,
  /// 
  ///         // Start of an internal call into the CLR.
  ///         STUBFRAME_INTERNALCALL = 0x00000006,
  /// 
  ///         // start of a class initialization; corresponds to CHAIN_CLASS_INIT
  ///         STUBFRAME_CLASS_INIT = 0x00000007,
  /// 
  ///         // an exception is thrown; corresponds to CHAIN_EXCEPTION_FILTER
  ///         STUBFRAME_EXCEPTION = 0x00000008,
  /// 
  ///         // a frame used for code-access security purposes; corresponds to CHAIN_SECURITY
  ///         STUBFRAME_SECURITY = 0x00000009,
  /// 
  ///         // a frame used to mark that the runtime is jitting a managed method
  ///         STUBFRAME_JIT_COMPILATION = 0x0000000a,
  ///     } CorDebugInternalFrameType;
  /// 
  ///     // Get the type of internal frame. This will never be STUBFRAME_NONE.
  ///     // Debuggers should gracefully ignore unrecognized internal frame types.
  ///     HRESULT GetFrameType([out] CorDebugInternalFrameType * pType);
  /// 
  /// };
  ///  </code></example>
  [Guid ("B92CC7F7-9D2D-45C4-BC2B-621FCC9DFBF4")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public unsafe interface ICorDebugInternalFrame : ICorDebugFrame
  {
    /// <summary>
    /// GetChain returns the chain of which this stack frame is a part.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetChain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetCode returns the code which this stack frame is running if any.
    /// </summary>
    /// <param name="ppCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

    /// <summary>
    /// GetFunction returns the function for the code which this stack
    /// frame is running.
    /// For ICorDebugInternalFrames, this may point to a method the
    /// frame is associated with (which may be in a different AppDomain
    /// from the frame itself), or may fail if the frame doesn't relate to any
    /// particular function.
    /// </summary>
    /// <param name="ppFunction"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// GetFunctionToken is a convenience routine to return the token for the
    /// function for the code which this stack frame is running.
    /// The scope to resolve the token can be gotten from the ICorDebugFunction
    /// associated with this frame.</summary>
    /// <param name="pToken"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunctionToken ([ComAliasName ("mdMethodDef")] uint* pToken);

    /// <summary>
    /// GetStackRange returns the absolute address range of the stack
    /// frame.  (This is useful for piecing together interleaved stack
    /// traces gathered from multiple debugging engines.)  Note that you
    /// cannot make any assumptions about what is actually stored on
    /// the stack - the numeric range is to compare stack frame
    /// locations only.
    /// The start of a stack range is the leafmost boundary of the frame, and
    /// the end of a stack range is the rootmost boundary of the frame.
    /// </summary>
    /// <param name="pStart"></param>
    /// <param name="pEnd"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetStackRange ([ComAliasName ("CORDB_ADDRESS")] ulong* pStart, [ComAliasName ("CORDB_ADDRESS")] ulong* pEnd);

    /// <summary>
    /// GetCaller returns a pointer to the frame in the current chain
    /// which called this frame, or NULL if this is the rootmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCaller ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// GetCallee returns a pointer to the frame in the current chain
    /// which this frame called, or NULL if this is the leafmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCallee ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// CreateStepper creates a stepper object which operates relative to the
    /// frame. The Stepper API must then be used to perform actual stepping.
    /// Note that if this frame is not active, the frame will typically have to
    /// be returned to before the step is completed.
    /// </summary>
    /// <param name="ppStepper"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateStepper ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStepper ppStepper);

    /// <summary>
    /// Get the type of internal frame. This will never be STUBFRAME_NONE.
    /// Debuggers should gracefully ignore unrecognized internal frame types.
    /// </summary>
    /// <param name="pType"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFrameType (CorDebugInternalFrameType *pType);
  }
}