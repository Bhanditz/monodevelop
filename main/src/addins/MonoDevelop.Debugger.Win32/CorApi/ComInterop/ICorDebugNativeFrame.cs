using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugNativeFrame is a specialized interface of ICorDebugFrame for jitted frames, i.e. native frames for managed methods.
  ///  (Note that jitted frames implement both ICorDebugILFrame and ICorDebugNativeFrame.)
  /// </summary>
  /// <example><code>
  ///  
  ///  /*
  ///  * ICorDebugNativeFrame is a specialized interface of ICorDebugFrame for jitted frames, i.e. 
  ///  * native frames for managed methods.
  ///  * (Note that jitted frames implement both ICorDebugILFrame and ICorDebugNativeFrame.)
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(03E26314-4F76-11d3-88C6-006097945418),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugNativeFrame : ICorDebugFrame
  /// {
  ///     /*
  ///      * GetIP returns the stack frame's offset into the function's
  ///      * native code.  If this stack frame is active, this address is
  ///      * the next instruction to execute.  If this stack frame is not
  ///      * active, this is the next instruction to execute when the stack
  ///      * frame is reactivated.
  ///      */
  /// 
  ///     HRESULT GetIP([out] ULONG32 *pnOffset);
  /// 
  ///     /*
  ///      * SetIP sets the instruction pointer to the given native
  ///      * offset. CorDebug will attempt to keep the stack frame in a
  ///      * coherent state.  (Note that even if the frame is in a valid
  ///      * state as far as the runtime is concerned, there still may be
  ///      * problems - e.g. uninitialized local variables, etc.  The caller
  ///      * (or perhaps the user) is responsible for insuring coherency of
  ///      * the running program.)
  ///      *
  ///      * Calling SetIP immediately invalidates all frames and chains for the
  ///      * current thread; the debugger must perform a new stack trace if it
  ///      * requires frame information after calling SetIP.
  ///      */
  /// 
  ///     HRESULT SetIP([in] ULONG32 nOffset);
  /// 
  ///     /*
  ///      * GetRegisterSet returns the register set for the given frame.
  ///      *
  ///      */
  /// 
  ///     HRESULT GetRegisterSet([out] ICorDebugRegisterSet **ppRegisters);
  /// 
  ///     /*
  ///      * GetLocalRegisterValue gets the value for a local variable or
  ///      * argument stored in a register of a native frame. This can be
  ///      * used either in a native frame or a jitted frame.
  ///      */
  /// 
  ///     HRESULT GetLocalRegisterValue([in] CorDebugRegister reg,
  ///                                   [in] ULONG cbSigBlob,
  ///                                   [in] PCCOR_SIGNATURE pvSigBlob,
  ///                                   [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * GetLocalDoubleRegisterValue gets the value for a local variable
  ///      * or argument stored in 2 registers of a native frame. This can
  ///      * be used either in a native frame or a jitted frame.
  ///      */
  /// 
  ///     HRESULT GetLocalDoubleRegisterValue([in] CorDebugRegister highWordReg,
  ///                                         [in] CorDebugRegister lowWordReg,
  ///                                         [in] ULONG cbSigBlob,
  ///                                         [in] PCCOR_SIGNATURE pvSigBlob,
  ///                                         [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * GetLocalMemoryValue gets the value for a local variable stored
  ///      * at the given address.
  ///      */
  /// 
  ///     HRESULT GetLocalMemoryValue([in] CORDB_ADDRESS address,
  ///                                 [in] ULONG cbSigBlob,
  ///                                 [in] PCCOR_SIGNATURE pvSigBlob,
  ///                                 [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * GetLocalRegisterMemoryValue gets the value for a local which
  ///      * is stored half in a register and half in memory.
  ///      */
  /// 
  ///     HRESULT GetLocalRegisterMemoryValue([in] CorDebugRegister highWordReg,
  ///                                         [in] CORDB_ADDRESS lowWordAddress,
  ///                                         [in] ULONG cbSigBlob,
  ///                                         [in] PCCOR_SIGNATURE pvSigBlob,
  ///                                         [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * GetLocalMemoryRegisterValue gets the value for a local which
  ///      * is stored half in a register and half in memory.
  ///      */
  /// 
  ///     HRESULT GetLocalMemoryRegisterValue([in] CORDB_ADDRESS highWordAddress,
  ///                                         [in] CorDebugRegister lowWordRegister,
  ///                                         [in] ULONG cbSigBlob,
  ///                                         [in] PCCOR_SIGNATURE pvSigBlob,
  ///                                         [out] ICorDebugValue **ppValue);
  ///     /*
  ///      * CanSetIP attempts to determine if it's safe to set the instruction pointer
  ///      * to the given native offset. If this returns S_OK, then executing
  ///      * SetIP (see above) will result in a safe, correct, continued execution.
  ///      * If CanSetIP returns anything else, SetIP can still be invoked, but
  ///      * continued, correct execution of the debuggee cannot be guaranteed.
  ///      *
  ///      */
  /// 
  ///     HRESULT CanSetIP([in] ULONG32 nOffset);
  /// };
  ///  </code></example>
  [Guid ("03E26314-4F76-11D3-88C6-006097945418")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public unsafe interface ICorDebugNativeFrame : ICorDebugFrame
  {
    /// <summary>
    /// GetChain returns the chain of which this stack frame is a part.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetChain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetCode returns the code which this stack frame is running if any.
    /// </summary>
    /// <param name="ppCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

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
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// GetFunctionToken is a convenience routine to return the token for the
    /// function for the code which this stack frame is running.
    /// The scope to resolve the token can be gotten from the ICorDebugFunction
    /// associated with this frame.</summary>
    /// <param name="pToken"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetFunctionToken ([ComAliasName ("mdMethodDef")] uint* pToken);

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
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetStackRange ([ComAliasName ("CORDB_ADDRESS")] ulong* pStart, [ComAliasName ("CORDB_ADDRESS")] ulong* pEnd);

    /// <summary>
    /// GetCaller returns a pointer to the frame in the current chain
    /// which called this frame, or NULL if this is the rootmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetCaller ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// GetCallee returns a pointer to the frame in the current chain
    /// which this frame called, or NULL if this is the leafmost frame
    /// in the chain.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetCallee ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// CreateStepper creates a stepper object which operates relative to the
    /// frame. The Stepper API must then be used to perform actual stepping.
    /// Note that if this frame is not active, the frame will typically have to
    /// be returned to before the step is completed.
    /// </summary>
    /// <param name="ppStepper"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CreateStepper ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStepper ppStepper);

    /// <summary>
    /// GetIP returns the stack frame's offset into the function's
    /// native code.  If this stack frame is active, this address is
    /// the next instruction to execute.  If this stack frame is not
    /// active, this is the next instruction to execute when the stack
    /// frame is reactivated.
    /// </summary>
    /// <param name="pnOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetIP (uint* pnOffset);

    /// <summary>
    /// SetIP sets the instruction pointer to the given native
    /// offset. CorDebug will attempt to keep the stack frame in a
    /// coherent state.  (Note that even if the frame is in a valid
    /// state as far as the runtime is concerned, there still may be
    /// problems - e.g. uninitialized local variables, etc.  The caller
    /// (or perhaps the user) is responsible for insuring coherency of
    /// the running program.)
    /// Calling SetIP immediately invalidates all frames and chains for the
    /// current thread; the debugger must perform a new stack trace if it
    /// requires frame information after calling SetIP.
    /// </summary>
    /// <param name="nOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetIP ([In] uint nOffset);

    /// <summary>
    /// GetRegisterSet returns the register set for the given frame.
    /// </summary>
    /// <param name="ppRegisters"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetRegisterSet ([MarshalAs (UnmanagedType.Interface)] out ICorDebugRegisterSet ppRegisters);

    /// <summary>
    /// GetLocalRegisterValue gets the value for a local variable or
    /// argument stored in a register of a native frame. This can be
    /// used either in a native frame or a jitted frame.
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetLocalRegisterValue ([In] CorDebugRegister reg, [In] uint cbSigBlob, [ComAliasName ("PCCOR_SIGNATURE")] [In] void* pvSigBlob, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// GetLocalDoubleRegisterValue gets the value for a local variable
    /// or argument stored in 2 registers of a native frame. This can
    /// be used either in a native frame or a jitted frame.
    /// </summary>
    /// <param name="highWordReg"></param>
    /// <param name="lowWordReg"></param>
    /// <param name="cbSigBlob"></param>
    /// <param name="pvSigBlob"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetLocalDoubleRegisterValue ([In] CorDebugRegister highWordReg, [In] CorDebugRegister lowWordReg, [In] uint cbSigBlob, [ComAliasName ("PCCOR_SIGNATURE")] void* pvSigBlob, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// GetLocalMemoryValue gets the value for a local variable stored
    /// at the given address.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="cbSigBlob"></param>
    /// <param name="pvSigBlob"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetLocalMemoryValue ([In] [ComAliasName ("CORDB_ADDRESS")] ulong address, [In] uint cbSigBlob, [ComAliasName ("PCCOR_SIGNATURE")] [In] void* pvSigBlob, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// GetLocalRegisterMemoryValue gets the value for a local which
    /// is stored half in a register and half in memory.
    /// </summary>
    /// <param name="highWordReg"></param>
    /// <param name="lowWordAddress"></param>
    /// <param name="cbSigBlob"></param>
    /// <param name="pvSigBlob"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetLocalRegisterMemoryValue ([In] CorDebugRegister highWordReg, [In] [ComAliasName ("CORDB_ADDRESS")] ulong lowWordAddress, [In] uint cbSigBlob, [ComAliasName ("PCCOR_SIGNATURE")] [In] void* pvSigBlob, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// GetLocalMemoryRegisterValue gets the value for a local which
    /// is stored half in a register and half in memory.
    /// </summary>
    /// <param name="highWordAddress"></param>
    /// <param name="lowWordRegister"></param>
    /// <param name="cbSigBlob"></param>
    /// <param name="pvSigBlob"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetLocalMemoryRegisterValue ([In] [ComAliasName ("CORDB_ADDRESS")] ulong highWordAddress, [In] CorDebugRegister lowWordRegister, [In] uint cbSigBlob, [ComAliasName ("PCCOR_SIGNATURE")] [In] void* pvSigBlob, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// CanSetIP attempts to determine if it's safe to set the instruction pointer
    /// to the given native offset. If this returns S_OK, then executing
    /// SetIP (see above) will result in a safe, correct, continued execution.
    /// If CanSetIP returns anything else, SetIP can still be invoked, but
    /// continued, correct execution of the debuggee cannot be guaranteed.
    /// </summary>
    /// <param name="nOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CanSetIP ([In] uint nOffset);
  }
}