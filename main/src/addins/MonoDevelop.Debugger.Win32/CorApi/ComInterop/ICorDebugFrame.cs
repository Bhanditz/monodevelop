using System;
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
  ///     uuid(CC7BCAEF-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugFrame : IUnknown
  /// {
  ///     /*
  ///      * GetChain returns the chain of which this stack frame is a part.
  ///      */
  /// 
  ///     HRESULT GetChain([out] ICorDebugChain **ppChain);
  /// 
  ///     /*
  ///      * GetCode returns the code which this stack frame is running if any.
  ///      */
  /// 
  ///     HRESULT GetCode([out] ICorDebugCode **ppCode);
  /// 
  ///     /*
  ///      * GetFunction returns the function for the code which this stack 
  ///      * frame is running.  
  ///      * For ICorDebugInternalFrames, this may point to a method the 
  ///      * frame is associated with (which may be in a different AppDomain
  ///      * from the frame itself), or may fail if the frame doesn't relate to any
  ///      * particular function.
  ///      */
  /// 
  ///     HRESULT GetFunction([out] ICorDebugFunction **ppFunction);
  /// 
  ///     /*
  ///      * GetFunctionToken is a convenience routine to return the token for the
  ///      * function for the code which this stack frame is running.
  ///      * The scope to resolve the token can be gotten from the ICorDebugFunction 
  ///      * associated with this frame.
  ///      */
  /// 
  ///     HRESULT GetFunctionToken([out] mdMethodDef *pToken);
  /// 
  ///     /*
  ///      * GetStackRange returns the absolute address range of the stack
  ///      * frame.  (This is useful for piecing together interleaved stack
  ///      * traces gathered from multiple debugging engines.)  Note that you
  ///      * cannot make any assumptions about what is actually stored on
  ///      * the stack - the numeric range is to compare stack frame
  ///      * locations only.
  ///      * The start of a stack range is the leafmost boundary of the frame, and 
  ///      * the end of a stack range is the rootmost boundary of the frame.
  ///      */
  /// 
  ///     HRESULT GetStackRange([out] CORDB_ADDRESS *pStart, [out] CORDB_ADDRESS *pEnd);
  /// 
  ///     /*
  ///      * GetCaller returns a pointer to the frame in the current chain
  ///      * which called this frame, or NULL if this is the rootmost frame
  ///      * in the chain.
  ///      */
  /// 
  ///     HRESULT GetCaller([out] ICorDebugFrame **ppFrame);
  /// 
  ///     /*
  ///      * GetCallee returns a pointer to the frame in the current chain
  ///      * which this frame called, or NULL if this is the leafmost frame
  ///      * in the chain.
  ///      */
  /// 
  ///     HRESULT GetCallee([out] ICorDebugFrame **ppFrame);
  /// 
  ///     /*
  ///      * CreateStepper creates a stepper object which operates relative to the
  ///      * frame. The Stepper API must then be used to perform actual stepping.
  ///      *
  ///      * Note that if this frame is not active, the frame will typically have to
  ///      * be returned to before the step is completed.
  ///      *
  ///      */
  /// 
  ///     HRESULT CreateStepper([out] ICorDebugStepper **ppStepper);
  /// };
  /// 
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("CC7BCAEF-8A68-11D2-983C-0000F808342D")]
  [ComImport]
  public unsafe interface ICorDebugFrame
  {
    /// <summary>
    /// GetChain returns the chain of which this stack frame is a part.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetChain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetCode returns the code which this stack frame is running if any.
    /// </summary>
    /// <param name="ppCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

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
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// GetFunctionToken is a convenience routine to return the token for the
    /// function for the code which this stack frame is running.
    /// The scope to resolve the token can be gotten from the ICorDebugFunction
    /// associated with this frame.</summary>
    /// <param name="pToken"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunctionToken ([ComAliasName ("mdMethodDef")] uint* pToken);

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
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetStackRange ([ComAliasName ("CORDB_ADDRESS")] ulong* pStart, [ComAliasName ("CORDB_ADDRESS")] ulong* pEnd);

    /// <summary>
    /// GetCaller returns a pointer to the frame in the current chain
    /// which called this frame, or NULL if this is the rootmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCaller ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// GetCallee returns a pointer to the frame in the current chain
    /// which this frame called, or NULL if this is the leafmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCallee ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// CreateStepper creates a stepper object which operates relative to the
    /// frame. The Stepper API must then be used to perform actual stepping.
    /// Note that if this frame is not active, the frame will typically have to
    /// be returned to before the step is completed.
    /// </summary>
    /// <param name="ppStepper"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateStepper ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStepper ppStepper);
  }
}