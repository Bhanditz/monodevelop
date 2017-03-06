using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugModule2 is a logical extension to ICorDebugModule.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugModule2 is a logical extension to ICorDebugModule.
  ///  */
  /// [
  ///     object,
  ///     local,
  ///     uuid(7FCC5FB5-49C0-41de-9938-3B88B5B9ADD7),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugModule2 : IUnknown
  /// {
  ///     /*
  ///      * SetUserCode sets the user-code status of all the functions on all the classes in
  ///      * the module to bIsJustMyCode, except for the functions or classes in the tokens array,
  ///      * which it sets to !bIsJustMyCode.
  ///      * These settings erase all previous JMC settings in this module.
  ///      * JMC status can be refined by calls to SetJMCStatus on the Class and Function.
  ///      * Returns S_OK if all functions were set successfully,
  ///      * CORDBG_E_FUNCTION_NOT_DEBUGGABLE if some function to be marked TRUE was not
  ///      * debuggable.
  ///      */
  ///     HRESULT SetJMCStatus([in] BOOL bIsJustMyCode,
  ///                         [in] ULONG32 cTokens,
  ///                         [in, size_is(cTokens)] mdToken pTokens[]);
  /// 
  ///    /*
  ///      * ApplyChanges is called to apply an Edit and Continue delta to the running process.
  ///      * An EnC delta consists of a delta metadata blob (created by IMetadataEmit2::SaveDelta)
  ///      * and a delta IL blob (a method body stream just like the one in an on disk assembly).
  ///      * If this operation fails, the debug session is considered to be in an invalid state
  ///      * and must be restarted.
  ///      */
  ///     HRESULT ApplyChanges([in] ULONG cbMetadata,
  ///                          [in, size_is(cbMetadata)] BYTE pbMetadata[],
  ///                          [in] ULONG cbIL,
  ///                          [in, size_is(cbIL)] BYTE pbIL[]);
  /// 
  ///   /*
  ///    * SetJITCompilerFlags sets the flags that control the JIT compiler. If the set of flags is invalid,
  ///    * the function will fail. This function can only be called from within the true LoadModule callback
  ///    * for the given module. Attempts to call it after this callback has been delivered or in a "faked"
  ///    * LoadModule callback for debugger attach will fail.
  ///    */
  /// 
  ///     HRESULT SetJITCompilerFlags( [in] DWORD dwFlags );
  /// 
  ///     /*
  ///     * GetJITCompilerFlags gets the set of flags that control the JIT compiler for this module.
  ///     */
  /// 
  ///     HRESULT GetJITCompilerFlags( [out] DWORD *pdwFlags );
  /// 
  ///     /*
  ///      * Resolve an assembly given an AssemblyRef token. Note that
  ///      * this will not trigger the loading of assembly. If assembly is not yet loaded,
  ///      * this will return an CORDBG_E_CANNOT_RESOLVE_ASSEMBLY error
  ///      *
  ///     */
  ///     HRESULT ResolveAssembly([in] mdToken tkAssemblyRef,
  /// 							[out] ICorDebugAssembly **ppAssembly);
  /// 
  /// };
  /// 
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("7FCC5FB5-49C0-41DE-9938-3B88B5B9ADD7")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugModule2
  {
    /// <summary>
    /// SetUserCode sets the user-code status of all the functions on all the classes in
    /// the module to bIsJustMyCode, except for the functions or classes in the tokens array,
    /// which it sets to !bIsJustMyCode.
    /// These settings erase all previous JMC settings in this module.
    /// JMC status can be refined by calls to SetJMCStatus on the Class and Function.
    /// Returns S_OK if all functions were set successfully,
    /// CORDBG_E_FUNCTION_NOT_DEBUGGABLE if some function to be marked TRUE was not
    /// debuggable.
    /// </summary>
    /// <param name="bIsJustMyCode"></param>
    /// <param name="cTokens"></param>
    /// <param name="pTokens"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetJMCStatus ([In] Int32 bIsJustMyCode, [In] UInt32 cTokens, [In] [ComAliasName ("mdToken[]")] UInt32* pTokens);

    /// <summary>
    /// ApplyChanges is called to apply an Edit and Continue delta to the running process.
    /// An EnC delta consists of a delta metadata blob (created by IMetadataEmit2::SaveDelta)
    /// and a delta IL blob (a method body stream just like the one in an on disk assembly).
    /// If this operation fails, the debug session is considered to be in an invalid state
    /// and must be restarted.
    /// </summary>
    /// <param name="cbMetadata"></param>
    /// <param name="pbMetadata"></param>
    /// <param name="cbIL"></param>
    /// <param name="pbIL"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ApplyChanges ([In] UInt32 cbMetadata, [In] Byte* pbMetadata, [In] UInt32 cbIL, [In] Byte* pbIL);

    /// <summary>
    /// SetJITCompilerFlags sets the flags that control the JIT compiler. If the set of flags is invalid,
    /// the function will fail. This function can only be called from within the true LoadModule callback
    /// for the given module. Attempts to call it after this callback has been delivered or in a "faked"
    /// LoadModule callback for debugger attach will fail.
    /// </summary>
    /// <param name="dwFlags"><see cref="CorDebugJITCompilerFlags"/></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetJITCompilerFlags ([In] UInt32 dwFlags);

    /// <summary>
    /// GetJITCompilerFlags gets the set of flags that control the JIT compiler for this module.
    /// </summary>
    /// <param name="pdwFlags"><see cref="CorDebugJITCompilerFlags"/></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetJITCompilerFlags (UInt32* pdwFlags);

    /// <summary>
    /// Resolve an assembly given an AssemblyRef token. Note that
    /// this will not trigger the loading of assembly. If assembly is not yet loaded,
    /// this will return an CORDBG_E_CANNOT_RESOLVE_ASSEMBLY error
    /// </summary>
    /// <param name="tkAssemblyRef"></param>
    /// <param name="ppAssembly"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 ResolveAssembly ([In] UInt32 tkAssemblyRef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugAssembly ppAssembly);
  }
}