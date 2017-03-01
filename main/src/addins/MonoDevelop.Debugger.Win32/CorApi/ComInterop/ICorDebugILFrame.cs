using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugILFrame is a specialized interface of ICorDebugFrame for IL frames or jitted frames.
  ///  (Note that jitted frames implement both ICorDebugILFrame and ICorDebugNativeFrame.)
  /// </summary>
  /// <example><code>
  ///  
  ///  /*
  ///  * ICorDebugILFrame is a specialized interface of ICorDebugFrame for IL frames or jitted frames.
  ///  * (Note that jitted frames implement both ICorDebugILFrame and ICorDebugNativeFrame.)
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(03E26311-4F76-11d3-88C6-006097945418),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugILFrame : ICorDebugFrame
  /// {
  ///     /*
  ///      * GetIP returns the stack frame's offset into the function's IL code.
  ///      * If this stack frame is active, this address is the next
  ///      * instruction to execute.  If this stack frame is not active, this is the
  ///      * next instruction to execute when the stack frame is reactivated.
  ///      *
  ///      * Note that if this a jitted frame, the IP will be determined by
  ///      * mapping backwards from the actual native IP, so the value may
  ///      * be only approximately correct.
  ///      *
  ///      * If pMappingResult is not NULL, A mapping result is returned which
  ///      * indicates the details of how the IP was obtained.  The following values
  ///      * can be returned:
  ///      *
  ///      *  MAPPING_EXACT - the IP is correct; either the frame is
  ///      *  interpreted or there is an exact IL map for the function.
  ///      *
  ///      *  MAPPING_APPROXIMATE - the IP was successfully mapped, but may
  ///      *  be only approximately correct
  ///      *
  ///      *  MAPPING_UNMAPPED_ADDRESS - although there is mapping info for
  ///      *  the function, the current address is not mappable to IL.  An
  ///      *  IP of 0 is returned.
  ///      *
  ///      *  MAPPING_PROLOG - the native code is in the prolog, so an IP of
  ///      *  0 is returned
  ///      *
  ///      *  MAPPING_EPILOG - the native code is in an epilog, so the last
  ///      *  IP of the method is returned
  ///      *
  ///      *  MAPPING_NO_INFO - no mapping info is available for the method,
  ///      *  so an IP of 0 is returned
  ///      *
  ///      */
  /// 
  ///     typedef enum CorDebugMappingResult
  ///     {
  ///         MAPPING_PROLOG              = 0x1,
  ///         MAPPING_EPILOG              = 0x2,
  ///         MAPPING_NO_INFO             = 0x4,
  ///         MAPPING_UNMAPPED_ADDRESS    = 0x8,
  ///         MAPPING_EXACT               = 0x10,
  ///         MAPPING_APPROXIMATE         = 0x20,
  ///     } CorDebugMappingResult;
  /// 
  ///     HRESULT GetIP([out] ULONG32 *pnOffset, [out] CorDebugMappingResult *pMappingResult);
  /// 
  ///     /*
  ///      * SetIP sets the instruction pointer to the IL at the given offset.
  ///      * The debugger will do its best to fix up the state of the executing code
  ///      * so that it is consistent with the new IP as far as the EE is concerned,
  ///      * while preserving as much of the state of the user program as possible.
  ///      *
  ///      * Calling SetIP immediately invalidates all frames and chains for the
  ///      * current thread; the debugger must perform a new stack trace if it
  ///      * requires frame information after calling SetIP.
  ///      *
  ///      */
  /// 
  ///     HRESULT SetIP([in] ULONG32 nOffset);
  /// 
  ///     /*
  ///      * EnumerateLocalVariables returns a list of the local variables
  ///      * available in the frame.  Note that this may not include all of
  ///      * the locals in the running function, as some of them may not be
  ///      * active.
  ///      */
  /// 
  ///     HRESULT EnumerateLocalVariables([out] ICorDebugValueEnum **ppValueEnum);
  /// 
  ///     /*
  ///      * GetLocalVariable gets the value for a local variable
  ///      * in an IL frame.  This can be used either in an IL
  ///      * frame or a jitted frame.
  ///      */
  /// 
  ///     HRESULT GetLocalVariable([in] DWORD dwIndex,
  ///                              [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * EnumerateArguments returns a list of the arguments available in the
  ///      * frame.  Note that this will include varargs arguments as well as
  ///      * arguments declared by the function signature (inlucding the implicit 
  ///      * "this" argument if any).
  ///      */
  /// 
  ///     HRESULT EnumerateArguments([out] ICorDebugValueEnum **ppValueEnum);
  /// 
  ///     /*
  ///      * GetArgument gets the value for an argument
  ///      * in an IL frame.  This can be used either in an IL
  ///      * frame or a jitted frame.
  ///      * For instance (non-static) methods, argument index 0 is the "this" object,
  ///      * and the normal explicit arguments start with index 1.
  ///      */
  /// 
  ///     HRESULT GetArgument([in] DWORD dwIndex,
  ///                         [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetStackDepth([out] ULONG32 *pDepth);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetStackValue([in] DWORD dwIndex,
  ///                           [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * CanSetIP attempts to determine if it's safe to set the instruction pointer
  ///      * to the IL at the given offset. If this returns S_OK, then executing
  ///      * SetIP (see above) will result in a safe, correct, continued execution.
  ///      * If CanSetIP returns anything else, SetIP can still be invoked, but
  ///      * continued, correct execution of the debuggee cannot be guaranteed.
  ///      *
  ///      */
  /// 
  ///     HRESULT CanSetIP([in] ULONG32 nOffset);
  /// };
  /// 
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("03E26311-4F76-11D3-88C6-006097945418")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugILFrame : ICorDebugFrame
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

    /// <summary>
    /// GetIP returns the stack frame's offset into the function's IL code.
    /// If this stack frame is active, this address is the next
    /// instruction to execute.  If this stack frame is not active, this is the
    /// next instruction to execute when the stack frame is reactivated.
    /// Note that if this a jitted frame, the IP will be determined by
    /// mapping backwards from the actual native IP, so the value may
    /// be only approximately correct.
    /// If pMappingResult is not NULL, A mapping result is returned which
    /// indicates the details of how the IP was obtained.  The following values
    /// can be returned:
    ///  MAPPING_EXACT - the IP is correct; either the frame is
    ///  interpreted or there is an exact IL map for the function.
    ///  MAPPING_APPROXIMATE - the IP was successfully mapped, but may
    ///  be only approximately correct
    ///  MAPPING_UNMAPPED_ADDRESS - although there is mapping info for
    ///  the function, the current address is not mappable to IL.  An
    ///  IP of 0 is returned.
    ///  MAPPING_PROLOG - the native code is in the prolog, so an IP of
    ///  0 is returned
    ///  MAPPING_EPILOG - the native code is in an epilog, so the last
    ///  IP of the method is returned
    ///  MAPPING_NO_INFO - no mapping info is available for the method,
    ///  so an IP of 0 is returned
    /// </summary>
    /// <param name="pnOffset"></param>
    /// <param name="pMappingResult"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetIP (UInt32* pnOffset, CorDebugMappingResult* pMappingResult);

    /// <summary>
    /// SetIP sets the instruction pointer to the IL at the given offset.
    /// The debugger will do its best to fix up the state of the executing code
    /// so that it is consistent with the new IP as far as the EE is concerned,
    /// while preserving as much of the state of the user program as possible.
    /// Calling SetIP immediately invalidates all frames and chains for the
    /// current thread; the debugger must perform a new stack trace if it
    /// requires frame information after calling SetIP.
    /// </summary>
    /// <param name="nOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetIP ([In] UInt32 nOffset);

    /// <summary>
    /// EnumerateLocalVariables returns a list of the local variables
    /// available in the frame.  Note that this may not include all of
    /// the locals in the running function, as some of them may not be
    /// active.
    /// </summary>
    /// <param name="ppValueEnum"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumerateLocalVariables ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueEnum ppValueEnum);

    /// <summary>
    /// GetLocalVariable gets the value for a local variable
    /// in an IL frame.  This can be used either in an IL
    /// frame or a jitted frame.
    /// </summary>
    /// <param name="dwIndex"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetLocalVariable ([In] UInt32 dwIndex, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// EnumerateArguments returns a list of the arguments available in the
    /// frame.  Note that this will include varargs arguments as well as
    /// arguments declared by the function signature (inlucding the implicit
    /// "this" argument if any).
    /// </summary>
    /// <param name="ppValueEnum"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumerateArguments ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueEnum ppValueEnum);

    /// <summary>
    /// GetArgument gets the value for an argument
    /// in an IL frame.  This can be used either in an IL
    /// frame or a jitted frame.
    /// For instance (non-static) methods, argument index 0 is the "this" object,
    /// and the normal explicit arguments start with index 1.
    /// </summary>
    /// <param name="dwIndex"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetArgument ([In] UInt32 dwIndex, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="pDepth"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStackDepth (UInt32* pDepth);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStackValue ([In] UInt32 dwIndex, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// CanSetIP attempts to determine if it's safe to set the instruction pointer
    /// to the IL at the given offset. If this returns S_OK, then executing
    /// SetIP (see above) will result in a safe, correct, continued execution.
    /// If CanSetIP returns anything else, SetIP can still be invoked, but
    /// continued, correct execution of the debuggee cannot be guaranteed.
    /// </summary>
    /// <param name="nOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CanSetIP ([In] UInt32 nOffset);
  }
}