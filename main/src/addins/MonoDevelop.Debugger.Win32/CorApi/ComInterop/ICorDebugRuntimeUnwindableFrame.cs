using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugRuntimeUnwindableFrame is a specialized interface of ICorDebugFrame for unmanaged methods
  ///  which requires special knowledge to unwind.  They are not jitted code.  When the debugger sees this type
  ///  of frames, it should use ICorDebugStackWalk::Next() to unwind, but it should do inspection itself.
  ///  The debugger can call ICorDebugStackWalk::GetContext() to retrieve the CONTEXT of the frame when it gets
  ///  an ICorDebugRuntimeUnwindableFrame.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugRuntimeUnwindableFrame is a specialized interface of ICorDebugFrame for unmanaged methods
  ///  * which requires special knowledge to unwind.  They are not jitted code.  When the debugger sees this type
  ///  * of frames, it should use ICorDebugStackWalk::Next() to unwind, but it should do inspection itself.
  ///  * The debugger can call ICorDebugStackWalk::GetContext() to retrieve the CONTEXT of the frame when it gets 
  ///  * an ICorDebugRuntimeUnwindableFrame.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(879CAC0A-4A53-4668-B8E3-CB8473CB187F),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugRuntimeUnwindableFrame : ICorDebugFrame
  /// {
  /// }
  ///  </code></example>
  [Guid ("879CAC0A-4A53-4668-B8E3-CB8473CB187F")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public unsafe interface ICorDebugRuntimeUnwindableFrame : ICorDebugFrame
  {
    /// <summary>
    /// GetChain returns the chain of which this stack frame is a part.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetChain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetCode returns the code which this stack frame is running if any.
    /// </summary>
    /// <param name="ppCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

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
    new void GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// GetFunctionToken is a convenience routine to return the token for the
    /// function for the code which this stack frame is running.
    /// The scope to resolve the token can be gotten from the ICorDebugFunction
    /// associated with this frame.</summary>
    /// <param name="pToken"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetFunctionToken ([ComAliasName ("mdMethodDef")] uint* pToken);

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
    new void GetStackRange ([ComAliasName ("CORDB_ADDRESS")] ulong* pStart, [ComAliasName ("CORDB_ADDRESS")] ulong* pEnd);

    /// <summary>
    /// GetCaller returns a pointer to the frame in the current chain
    /// which called this frame, or NULL if this is the rootmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetCaller ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// GetCallee returns a pointer to the frame in the current chain
    /// which this frame called, or NULL if this is the leafmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetCallee ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// CreateStepper creates a stepper object which operates relative to the
    /// frame. The Stepper API must then be used to perform actual stepping.
    /// Note that if this frame is not active, the frame will typically have to
    /// be returned to before the step is completed.
    /// </summary>
    /// <param name="ppStepper"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void CreateStepper ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStepper ppStepper);
  }
}