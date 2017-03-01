using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  
  ///  * Program state object interfaces
  ///  * ------------------------------------------------------------------------- */
  /// 
  ///  * ICorDebugRegisterSet
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCB0B-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugRegisterSet : IUnknown
  /// {
  ///     typedef enum CorDebugRegister
  ///     {
  ///         // registers (potentially) available on all architectures
  ///         // Note that these overlap with the architecture-specific
  ///         // registers
  ///         //
  ///         // NOTE: On IA64, REGISTER_FRAME_POINTER represents the BSP register.
  /// 
  ///         REGISTER_INSTRUCTION_POINTER = 0,
  ///         REGISTER_STACK_POINTER,
  ///         REGISTER_FRAME_POINTER,
  /// 
  /// 
  ///         // X86 registers
  /// 
  ///         REGISTER_X86_EIP = 0,
  ///         REGISTER_X86_ESP,
  ///         REGISTER_X86_EBP,
  /// 
  ///         REGISTER_X86_EAX,
  ///         REGISTER_X86_ECX,
  ///         REGISTER_X86_EDX,
  ///         REGISTER_X86_EBX,
  /// 
  ///         REGISTER_X86_ESI,
  ///         REGISTER_X86_EDI,
  /// 
  ///         REGISTER_X86_FPSTACK_0,
  ///         REGISTER_X86_FPSTACK_1,
  ///         REGISTER_X86_FPSTACK_2,
  ///         REGISTER_X86_FPSTACK_3,
  ///         REGISTER_X86_FPSTACK_4,
  ///         REGISTER_X86_FPSTACK_5,
  ///         REGISTER_X86_FPSTACK_6,
  ///         REGISTER_X86_FPSTACK_7,
  /// 
  /// 
  ///         // AMD64 registers
  /// 
  ///         REGISTER_AMD64_RIP = 0,
  ///         REGISTER_AMD64_RSP,
  ///         REGISTER_AMD64_RBP,
  /// 
  ///         REGISTER_AMD64_RAX,
  ///         REGISTER_AMD64_RCX,
  ///         REGISTER_AMD64_RDX,
  ///         REGISTER_AMD64_RBX,
  /// 
  ///         REGISTER_AMD64_RSI,
  ///         REGISTER_AMD64_RDI,
  /// 
  ///         REGISTER_AMD64_R8,
  ///         REGISTER_AMD64_R9,
  ///         REGISTER_AMD64_R10,
  ///         REGISTER_AMD64_R11,
  ///         REGISTER_AMD64_R12,
  ///         REGISTER_AMD64_R13,
  ///         REGISTER_AMD64_R14,
  ///         REGISTER_AMD64_R15,
  /// 
  ///         // Xmm FP
  /// 
  ///         REGISTER_AMD64_XMM0,
  ///         REGISTER_AMD64_XMM1,
  ///         REGISTER_AMD64_XMM2,
  ///         REGISTER_AMD64_XMM3,
  ///         REGISTER_AMD64_XMM4,
  ///         REGISTER_AMD64_XMM5,
  ///         REGISTER_AMD64_XMM6,
  ///         REGISTER_AMD64_XMM7,
  ///         REGISTER_AMD64_XMM8,
  ///         REGISTER_AMD64_XMM9,
  ///         REGISTER_AMD64_XMM10,
  ///         REGISTER_AMD64_XMM11,
  ///         REGISTER_AMD64_XMM12,
  ///         REGISTER_AMD64_XMM13,
  ///         REGISTER_AMD64_XMM14,
  ///         REGISTER_AMD64_XMM15,
  /// 
  /// 
  ///         // IA64 registers
  /// 
  ///         REGISTER_IA64_BSP = REGISTER_FRAME_POINTER,
  /// 
  ///         // To get a particular general register, add the register number
  ///         // to REGISTER_IA64_R0.  The same also goes for floating point
  ///         // registers.
  ///         //
  ///         // For example, if you need REGISTER_IA64_R83,
  ///         // use REGISTER_IA64_R0 + 83.
  ///         REGISTER_IA64_R0  = REGISTER_IA64_BSP + 1,
  ///         REGISTER_IA64_F0  = REGISTER_IA64_R0  + 128,
  /// 
  /// 
  ///         // ARM registers (@ARMTODO: FP?)
  /// 
  ///         REGISTER_ARM_PC = 0,
  ///         REGISTER_ARM_SP,
  ///         REGISTER_ARM_R0,
  ///         REGISTER_ARM_R1,
  ///         REGISTER_ARM_R2,
  ///         REGISTER_ARM_R3,
  ///         REGISTER_ARM_R4,
  ///         REGISTER_ARM_R5,
  ///         REGISTER_ARM_R6,
  ///         REGISTER_ARM_R7,
  ///         REGISTER_ARM_R8,
  ///         REGISTER_ARM_R9,
  ///         REGISTER_ARM_R10,
  ///         REGISTER_ARM_R11,
  ///         REGISTER_ARM_R12,
  ///         REGISTER_ARM_LR,
  /// 
  ///         // ARM64 registers
  ///             
  ///         REGISTER_ARM64_PC = 0,
  ///         REGISTER_ARM64_SP,
  ///         REGISTER_ARM64_FP,
  ///         REGISTER_ARM64_X0,
  ///         REGISTER_ARM64_X1,
  ///         REGISTER_ARM64_X2,
  ///         REGISTER_ARM64_X3,
  ///         REGISTER_ARM64_X4,
  ///         REGISTER_ARM64_X5,
  ///         REGISTER_ARM64_X6,
  ///         REGISTER_ARM64_X7,
  ///         REGISTER_ARM64_X8,
  ///         REGISTER_ARM64_X9,
  ///         REGISTER_ARM64_X10,
  ///         REGISTER_ARM64_X11,
  ///         REGISTER_ARM64_X12,
  ///         REGISTER_ARM64_X13,
  ///         REGISTER_ARM64_X14,
  ///         REGISTER_ARM64_X15,
  ///         REGISTER_ARM64_X16,
  ///         REGISTER_ARM64_X17,
  ///         REGISTER_ARM64_X18,
  ///         REGISTER_ARM64_X19,
  ///         REGISTER_ARM64_X20,
  ///         REGISTER_ARM64_X21,
  ///         REGISTER_ARM64_X22,
  ///         REGISTER_ARM64_X23,
  ///         REGISTER_ARM64_X24,
  ///         REGISTER_ARM64_X25,
  ///         REGISTER_ARM64_X26,
  ///         REGISTER_ARM64_X27,
  ///         REGISTER_ARM64_X28,
  ///         REGISTER_ARM64_LR,
  /// 
  ///         // other architectures here
  /// 
  ///     } CorDebugRegister;
  /// 
  ///     /*
  ///      * GetRegistersAvailable returns a mask indicating which registers
  ///      * are available in the given register set.  Registers may be unavailable
  ///      * if their value is undeterminable for the given situation.  The returned
  ///      * word contains a bit for each register (1 &lt;&lt; register index), which will
  ///      * be 1 if the register is available or 0 if it is not.
  ///      */
  /// 
  ///     HRESULT GetRegistersAvailable([out] ULONG64 *pAvailable);
  /// 
  ///     /*
  ///      * GetRegisters returns an array of register values corresponding
  ///      * to the given mask.  The registers which have their bit set in
  ///      * the mask will be packed into the resulting array.  (No room is
  ///      * assigned in the array for registers whose mask bit is not set.)
  ///      * Thus, the size of the array should be equal to the number of
  ///      * 1's in the mask.
  ///      *
  ///      * If an unavailable register is indicated by the mask, an indeterminate
  ///      * value will be returned for the corresponding register.
  ///      *
  ///      * registerBufferCount should indicate number of elements in the
  ///      * buffer to receive the register values.  If it is too small for
  ///      * the number of registers indicated by the mask, the higher
  ///      * numbered registers will be truncated from the set.  Or, if it
  ///      * is too large, the unused registerBuffer elements will be
  ///      * unmodified.  */
  /// 
  ///     HRESULT GetRegisters([in] ULONG64 mask, [in] ULONG32 regCount,
  ///                          [out, size_is(regCount), length_is(regCount)]
  ///                          CORDB_REGISTER regBuffer[]);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT SetRegisters([in] ULONG64 mask,
  ///                          [in] ULONG32 regCount,
  ///                          [in, size_is(regCount)] CORDB_REGISTER regBuffer[]);
  /// 
  ///     /*
  ///      * GetThreadContext returns the context for the given thread.  The
  ///      * debugger should call this function rather than the Win32
  ///      * GetThreadContext, because the thread may actually be in a "hijacked"
  ///      * state where its context has been temporarily changed.
  ///      *
  ///      * The data returned is a CONTEXT structure for the current platform.
  ///      *
  ///      * For non-leaf frames, clients should check which registers are valid by
  ///      * using GetRegistersAvailable.
  ///      *
  ///      */
  /// 
  ///     HRESULT GetThreadContext([in] ULONG32 contextSize,
  ///                              [in, out, length_is(contextSize),
  ///                              size_is(contextSize)] BYTE context[]);
  /// 
  ///     /*
  ///      * Not implemented in v2.0. It is too dangerous to manipulate the context of
  ///      * threads in Managed code. Use other high level operations (like SetIp,
  ///      * ICorDebugValue::SetValue) instead.
  ///      *
  ///      */
  /// 
  ///     HRESULT SetThreadContext([in] ULONG32 contextSize,
  ///                              [in, length_is(contextSize),
  ///                              size_is(contextSize)] BYTE context[]);
  /// }
  ///  </code></example>
  [Guid ("CC7BCB0B-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugRegisterSet
  {
    /// <summary>
    /// GetRegistersAvailable returns a mask indicating which registers
    /// are available in the given register set.  Registers may be unavailable
    /// if their value is undeterminable for the given situation.  The returned
    /// word contains a bit for each register (1 &lt;&lt; register index), which will
    /// be 1 if the register is available or 0 if it is not.
    /// </summary>
    /// <param name="pAvailable"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetRegistersAvailable (UInt64* pAvailable);

    /// <summary>
    /// GetRegisters returns an array of register values corresponding
    /// to the given mask.  The registers which have their bit set in
    /// the mask will be packed into the resulting array.  (No room is
    /// assigned in the array for registers whose mask bit is not set.)
    /// Thus, the size of the array should be equal to the number of
    /// 1's in the mask.
    /// If an unavailable register is indicated by the mask, an indeterminate
    /// value will be returned for the corresponding register.
    /// registerBufferCount should indicate number of elements in the
    /// buffer to receive the register values.  If it is too small for
    /// the number of registers indicated by the mask, the higher
    /// numbered registers will be truncated from the set.  Or, if it
    /// is too large, the unused registerBuffer elements will be
    /// unmodified.
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="regCount"></param>
    /// <param name="regBuffer"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetRegisters ([In] UInt64 mask, [In] UInt32 regCount, [ComAliasName ("CORDB_REGISTER")] UInt64* regBuffer);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="regCount"></param>
    /// <param name="regBuffer"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetRegisters ([In] UInt64 mask, [In] UInt32 regCount, [ComAliasName ("CORDB_REGISTER")] UInt64* regBuffer);

    /// <summary>
    /// GetThreadContext returns the context for the given thread.  The
    /// debugger should call this function rather than the Win32
    /// GetThreadContext, because the thread may actually be in a "hijacked"
    /// state where its context has been temporarily changed.
    /// The data returned is a CONTEXT structure for the current platform.
    /// For non-leaf frames, clients should check which registers are valid by
    /// using GetRegistersAvailable.
    /// </summary>
    /// <param name="contextSize"></param>
    /// <param name="context"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetThreadContext ([In] UInt32 contextSize, Byte* context);

    /// <summary>
    /// Not implemented in v2.0. It is too dangerous to manipulate the context of
    /// threads in Managed code. Use other high level operations (like SetIp,
    /// ICorDebugValue::SetValue) instead.
    /// </summary>
    /// <param name="contextSize"></param>
    /// <param name="context"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetThreadContext ([In] UInt32 contextSize, Byte* context);
  }
}