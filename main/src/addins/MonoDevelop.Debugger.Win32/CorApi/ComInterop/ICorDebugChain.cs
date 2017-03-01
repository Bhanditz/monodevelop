using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugChain represents a segment of a physical or logical call
  ///  stack.  All frames in a chain occupy contiguous stack space, and
  ///  they share the same thread &amp; context.  A chain may represent either
  ///  managed or unmanaged code. Chains may be empty. Unmanaged chains are
  ///  always empty.
  /// </summary>
  /// <example><code>
  ///  
  ///  * ICorDebugChain represents a segment of a physical or logical call
  ///  * stack.  All frames in a chain occupy contiguous stack space, and
  ///  * they share the same thread &amp; context.  A chain may represent either
  ///  * managed or unmanaged code. Chains may be empty. Unmanaged chains are
  ///  * always empty.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAEE-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugChain : IUnknown
  /// {
  ///     /*
  ///      * GetThread returns the physical thread which this call chain is
  ///      * part of.
  ///      */
  /// 
  ///     HRESULT GetThread([out] ICorDebugThread **ppThread);
  /// 
  ///     /*
  ///      * GetStackRange returns the address range of the stack segment for the
  ///      * call chain.  Note that you cannot make any assumptions about
  ///      * what is actually stored on the stack - the numeric range is to compare
  ///      * stack frame locations only.
  ///      * The start of a stack range is the leafmost boundary of the chain, and 
  ///      * the end of a stack range is the rootmost boundary of the chain.
  ///      */
  /// 
  ///     HRESULT GetStackRange([out] CORDB_ADDRESS *pStart, [out] CORDB_ADDRESS *pEnd);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetContext([out] ICorDebugContext **ppContext);
  /// 
  ///     /*
  ///      * GetCaller returns a pointer to the chain which called this
  ///      * chain.  Note that this may be a chain on another thread in the
  ///      * case of cross-thread-marshalled calls. The caller will be NULL
  ///      * for spontaneously called chains (e.g. the ThreadProc, a
  ///      * debugger initiated call, etc.)
  ///      */
  /// 
  ///     HRESULT GetCaller([out] ICorDebugChain **ppChain);
  /// 
  ///     /*
  ///      * GetCallee returns a pointer to the chain which this chain is
  ///      * waiting on before it resumes. Note that this may be a chain on
  ///      * another thread in the case of cross-thread-marshalled
  ///      * calls. The callee will be NULL if the chain is currently
  ///      * actively running.
  ///      */
  /// 
  ///     HRESULT GetCallee([out] ICorDebugChain **ppChain);
  /// 
  ///     /*
  ///      * GetPrevious returns a pointer to the chain which was on this
  ///      * thread before the current one was pushed, if there is one.
  ///      */
  /// 
  ///     HRESULT GetPrevious([out] ICorDebugChain **ppChain);
  /// 
  ///     /*
  ///      * GetNext returns a pointer to the chain which was pushed on this
  ///      * thread after the current one, if there is one.
  ///      */
  /// 
  ///     HRESULT GetNext([out] ICorDebugChain **ppChain);
  /// 
  ///     /*
  ///      * IsManaged returns whether or not the chain is running managed
  ///      * code.
  ///      */
  /// 
  ///     HRESULT IsManaged([out] BOOL *pManaged);
  /// 
  ///     /*
  ///      * These chains represent the physical call stack for the thread.
  ///      * EnumerateFrames returns an iterator which will list all the stack
  ///      * frames in the chain, starting at the active (most recent) one. This
  ///      * should be called only for managed chains.
  ///      *
  ///      * NOTE: The debugging API does not provide methods for obtaining
  ///      * frames contained in unmanaged chains. The debugger needs to use
  ///      * other means to obtain this information.
  ///      */
  /// 
  ///     HRESULT EnumerateFrames([out] ICorDebugFrameEnum **ppFrames);
  /// 
  ///     /*
  ///      * GetActiveFrame is a convenience routine to return the
  ///      * active (most recent) frame on the chain, if any.
  ///      *
  ///      * If the active frame is not available, the call will succeed
  ///      * and *ppFrame will be NULL. Active frames will not be available
  ///      * for all CHAIN_ENTER_UNMANAGED chains, and for some
  ///      * CHAIN_CLASS_INIT chains.
  ///      */
  /// 
  ///     HRESULT GetActiveFrame([out] ICorDebugFrame **ppFrame);
  /// 
  ///     /*
  ///      * GetRegisterSet returns the register set for the beginnning (the leafmost end)
  ///      * of the chain.
  ///      */
  /// 
  ///     HRESULT GetRegisterSet([out] ICorDebugRegisterSet **ppRegisters);
  /// 
  ///     /*
  ///      * GetReason returns the reason for the genesis of this calling chain.
  ///      */
  /// 
  ///     typedef enum CorDebugChainReason
  ///     {
  ///         // Note that the first five line up with CorDebugIntercept
  ///         CHAIN_NONE              = 0x000,
  ///         CHAIN_CLASS_INIT        = 0x001,
  ///         CHAIN_EXCEPTION_FILTER  = 0x002,
  ///         CHAIN_SECURITY          = 0x004,
  ///         CHAIN_CONTEXT_POLICY    = 0x008,
  ///         CHAIN_INTERCEPTION      = 0x010,
  ///         CHAIN_PROCESS_START     = 0x020,
  ///         CHAIN_THREAD_START      = 0x040,
  ///         CHAIN_ENTER_MANAGED     = 0x080,
  ///         CHAIN_ENTER_UNMANAGED   = 0x100,
  ///         CHAIN_DEBUGGER_EVAL     = 0x200,
  ///         CHAIN_CONTEXT_SWITCH    = 0x400,
  ///         CHAIN_FUNC_EVAL         = 0x800,
  ///     } CorDebugChainReason;
  /// 
  ///     HRESULT GetReason([out] CorDebugChainReason *pReason);
  /// };
  ///  </code></example>
  [Guid ("CC7BCAEE-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugChain
  {
    /// <summary>
    /// GetThread returns the physical thread which this call chain is
    /// part of.
    /// </summary>
    /// <param name="ppThread"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetThread ([MarshalAs (UnmanagedType.Interface)] out ICorDebugThread ppThread);

    /// <summary>
    /// GetStackRange returns the address range of the stack segment for the
    /// call chain.  Note that you cannot make any assumptions about
    /// what is actually stored on the stack - the numeric range is to compare
    /// stack frame locations only.
    /// The start of a stack range is the leafmost boundary of the chain, and
    /// the end of a stack range is the rootmost boundary of the chain.
    /// </summary>
    /// <param name="pStart"></param>
    /// <param name="pEnd"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStackRange ([ComAliasName ("CORDB_ADDRESS")] UInt64* pStart, [ComAliasName ("CORDB_ADDRESS")] UInt64* pEnd);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="ppContext"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetContext ([MarshalAs (UnmanagedType.Interface)] out ICorDebugContext ppContext);

    /// <summary>
    /// GetCaller returns a pointer to the chain which called this
    /// chain.  Note that this may be a chain on another thread in the
    /// case of cross-thread-marshalled calls. The caller will be NULL
    /// for spontaneously called chains (e.g. the ThreadProc, a
    /// debugger initiated call, etc.)
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCaller ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetCallee returns a pointer to the chain which this chain is
    /// waiting on before it resumes. Note that this may be a chain on
    /// another thread in the case of cross-thread-marshalled
    /// calls. The callee will be NULL if the chain is currently
    /// actively running.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCallee ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetPrevious returns a pointer to the chain which was on this
    /// thread before the current one was pushed, if there is one.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetPrevious ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// GetNext returns a pointer to the chain which was pushed on this
    /// thread after the current one, if there is one.
    /// </summary>
    /// <param name="ppChain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetNext ([MarshalAs (UnmanagedType.Interface)] out ICorDebugChain ppChain);

    /// <summary>
    /// IsManaged returns whether or not the chain is running managed
    /// code.
    /// </summary>
    /// <param name="pManaged"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsManaged (Int32* pManaged);

    /// <summary>
    /// These chains represent the physical call stack for the thread.
    /// EnumerateFrames returns an iterator which will list all the stack
    /// frames in the chain, starting at the active (most recent) one. This
    /// should be called only for managed chains.
    /// NOTE: The debugging API does not provide methods for obtaining
    /// frames contained in unmanaged chains. The debugger needs to use
    /// other means to obtain this information.
    /// </summary>
    /// <param name="ppFrames"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumerateFrames ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrameEnum ppFrames);

    /// <summary>
    /// GetActiveFrame is a convenience routine to return the
    /// active (most recent) frame on the chain, if any.
    /// If the active frame is not available, the call will succeed
    /// and *ppFrame will be NULL. Active frames will not be available
    /// for all CHAIN_ENTER_UNMANAGED chains, and for some
    /// CHAIN_CLASS_INIT chains.
    /// </summary>
    /// <param name="ppFrame"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetActiveFrame ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFrame ppFrame);

    /// <summary>
    /// GetRegisterSet returns the register set for the beginnning (the leafmost end)
    /// of the chain.
    /// </summary>
    /// <param name="ppRegisters"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetRegisterSet ([MarshalAs (UnmanagedType.Interface)] out ICorDebugRegisterSet ppRegisters);

    /// <summary>
    /// GetReason returns the reason for the genesis of this calling chain.
    /// </summary>
    /// <param name="pReason"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetReason (CorDebugChainReason* pReason);
  }
}