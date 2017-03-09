using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugFunction represents a managed function.
  ///  In the non-EnC case, it is 1:1 with a methoddef metadata token.
  ///  For EnC, each version of a function has its own ICorDebugFunction instance.
  ///  EnCed functions keep the same metadata tokens, but will get new ICorDebugCode instances.
  ///  ICorDebugFunction does not represent generic typeparameters. That means that there's
  ///  an ICDFunction for Func&lt;T&gt;, but not for Func&lt;string&gt; or Func&lt;Bar&gt;. Get the generic
  ///  parameters from ICorDebugIlFrame::EnumerateTypeParameters.
  /// </summary>
  /// <example><code>
  ///  /*
  ///     ICorDebugFunction represents a managed function.
  ///     In the non-EnC case, it is 1:1 with a methoddef metadata token.
  ///     For EnC, each version of a function has its own ICorDebugFunction instance.
  ///     EnCed functions keep the same metadata tokens, but will get new ICorDebugCode instances.
  /// 
  ///     ICorDebugFunction does not represent generic typeparameters. That means that there's
  ///     an ICDFunction for Func&lt;T&gt;, but not for Func&lt;string&gt; or Func&lt;Bar&gt;. Get the generic
  ///     parameters from ICorDebugIlFrame::EnumerateTypeParameters.
  /// */
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAF3-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugFunction : IUnknown
  /// {
  ///     /*
  ///      * GetModule returns the module for the function.
  ///      */
  /// 
  ///     HRESULT GetModule([out] ICorDebugModule **ppModule);
  /// 
  ///     /*
  ///      * GetClass returns the class for the function. Returns null if
  ///      * the function is not a member.
  ///      */
  /// 
  ///     HRESULT GetClass([out] ICorDebugClass **ppClass);
  /// 
  ///     /*
  ///      * GetToken returns the metadata methodDef token for the function.
  ///      */
  /// 
  ///     HRESULT GetToken([out] mdMethodDef *pMethodDef);
  /// 
  ///     /*
  ///      * GetILCode returns the IL code for the function.  Returns null
  ///      * if there is no IL code for the function.  Note that this will
  ///      * get the IL code corresponding to this function's EnC version of
  ///      * the code in the runtime, if this function has been EnC'd.
  ///      */
  /// 
  ///     HRESULT GetILCode([out] ICorDebugCode **ppCode);
  /// 
  ///     /*
  ///      * GetNativeCode returns the native code for the function.
  ///      * Returns null if there is no native code for the function
  ///      * (i.e. it is an IL function which has not been jitted)
  ///      * If this function has been jitted multiple times (Eg, generics) this
  ///      * will return a random Native Code object.
  ///      */
  /// 
  ///     HRESULT GetNativeCode([out] ICorDebugCode **ppCode);
  /// 
  ///     /*
  ///      * CreateBreakpoint creates a breakpoint at the start of the function.
  ///      *
  ///      */
  /// 
  ///     HRESULT CreateBreakpoint([out] ICorDebugFunctionBreakpoint **ppBreakpoint);
  /// 
  ///     /*
  ///      * Returns the token for the local variable signature for this function.
  ///      * If there is no signature (ie, the function doesn't have any local
  ///      * variables), then mdSignatureNil will be returned.
  ///      */
  /// 
  ///     HRESULT GetLocalVarSigToken([out] mdSignature *pmdSig);
  /// 
  /// 
  ///     /*
  ///      * Obtains the latest (largest) EnC version number for this function.
  ///      * If this function has never been edited with EnC, this will return
  ///      * the same value as ICorDebugFunction2::GetVersionNumber.
  ///      */
  ///      HRESULT GetCurrentVersionNumber([out] ULONG32 *pnCurrentVersion);
  /// };
  ///  </code></example>
  [Guid ("CC7BCAF3-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugFunction
  {
    /// <summary>
    /// GetModule returns the module for the function.
    /// </summary>
    /// <param name="ppModule"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetModule ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModule ppModule);

    /// <summary>
    /// GetClass returns the class for the function. Returns null if
    /// the function is not a member.
    /// </summary>
    /// <param name="ppClass"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetClass ([MarshalAs (UnmanagedType.Interface)] out ICorDebugClass ppClass);

    /// <summary>
    /// GetToken returns the metadata methodDef token for the function.
    /// </summary>
    /// <param name="pMethodDef"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetToken ([ComAliasName ("mdMethodDef")] UInt32 *pMethodDef);

    /// <summary>
    /// GetILCode returns the IL code for the function.  Returns null
    /// if there is no IL code for the function.  Note that this will
    /// get the IL code corresponding to this function's EnC version of
    /// the code in the runtime, if this function has been EnC'd.
    /// </summary>
    /// <param name="ppCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetILCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

    /// <summary>
    /// GetNativeCode returns the native code for the function.
    /// Returns null if there is no native code for the function
    /// (i.e. it is an IL function which has not been jitted)
    /// If this function has been jitted multiple times (Eg, generics) this
    /// will return a random Native Code object.
    /// </summary>
    /// <param name="ppCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetNativeCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCode ppCode);

    /// <summary>
    /// CreateBreakpoint creates a breakpoint at the start of the function.
    /// </summary>
    /// <param name="ppBreakpoint"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunctionBreakpoint ppBreakpoint);

    /// <summary>
    /// Returns the token for the local variable signature for this function.
    /// If there is no signature (ie, the function doesn't have any local
    /// variables), then mdSignatureNil will be returned.
    /// </summary>
    /// <param name="pmdSig"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetLocalVarSigToken ([ComAliasName ("mdSignature")] UInt32* pmdSig);

    /// <summary>
    /// Obtains the latest (largest) EnC version number for this function.
    /// If this function has never been edited with EnC, this will return
    /// the same value as ICorDebugFunction2::GetVersionNumber.
    /// </summary>
    /// <param name="pnCurrentVersion"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCurrentVersionNumber (UInt32* pnCurrentVersion);
  }
}