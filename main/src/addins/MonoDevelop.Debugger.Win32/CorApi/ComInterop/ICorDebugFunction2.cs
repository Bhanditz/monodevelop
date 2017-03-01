using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugFunction2 is a logical extension to ICorDebugFunction.
  /// </summary>
  /// <example><code>
  ///  /*
  ///     ICorDebugFunction2 is a logical extension to ICorDebugFunction.
  /// */
  /// [
  ///     object,
  ///     local,
  ///     uuid(EF0C490B-94C3-4e4d-B629-DDC134C532D8),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugFunction2 : IUnknown
  /// {
  ///     /*
  ///      * Sets the User-code status (for JMC stepping) for this function.
  ///      * A JMC stepper will skip non-user code.
  ///      * User code must be a subset of debuggable code.
  ///      * Returns S_OK if successful, CORDBG_E_FUNCTION_NOT_DEBUGGABLE
  ///      * if bIsJustMyCode is TRUE and the function is not debuggable.
  ///      */
  ///     HRESULT SetJMCStatus([in] BOOL bIsJustMyCode);
  /// 
  ///     /*
  ///      * IsUserCode outputs whether the function is marked as user code.
  ///      * Always outputs FALSE for non-debuggable functions.
  ///      * Returns S_OK if successful.
  ///      */
  ///     HRESULT GetJMCStatus([out] BOOL * pbIsJustMyCode);
  /// 
  ///     /*
  ///      *    Not yet implemented.
  ///      */
  /// 
  ///     HRESULT EnumerateNativeCode([out] ICorDebugCodeEnum **ppCodeEnum);
  /// 
  ///     /*
  ///      * Obtains the EnC version number of the function represented by this ICorDebugFunction2.
  ///      * When a function is edited with EnC, the new function has a larger version number than
  ///      * that of any previous version (not necessarily exactly 1 greater).
  ///      * This function's version number will be less than or equal to the value returned by
  ///      * ICorDebugFunction::GetCurrentVersionNumber.
  ///      */
  /// 
  ///      HRESULT GetVersionNumber([out] ULONG32 *pnVersion);
  /// };
  ///  </code></example>
  [Guid ("EF0C490B-94C3-4E4D-B629-DDC134C532D8")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugFunction2
  {
    /// <summary>
    /// Sets the User-code status (for JMC stepping) for this function.
    /// A JMC stepper will skip non-user code.
    /// User code must be a subset of debuggable code.
    /// Returns S_OK if successful, CORDBG_E_FUNCTION_NOT_DEBUGGABLE
    /// if bIsJustMyCode is TRUE and the function is not debuggable.
    /// </summary>
    /// <param name="bIsJustMyCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetJMCStatus ([In] Int32 bIsJustMyCode);

    /// <summary>
    /// IsUserCode outputs whether the function is marked as user code.
    /// Always outputs FALSE for non-debuggable functions.
    /// Returns S_OK if successful.
    /// </summary>
    /// <param name="pbIsJustMyCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetJMCStatus (Int32* pbIsJustMyCode);

    /// <summary>
    /// Not yet implemented.
    /// </summary>
    /// <param name="ppCodeEnum"></param>
    [Obsolete ("Not yet implemented.")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumerateNativeCode ([MarshalAs (UnmanagedType.Interface)] out ICorDebugCodeEnum ppCodeEnum);

    /// <summary>
    /// Obtains the EnC version number of the function represented by this ICorDebugFunction2.
    /// When a function is edited with EnC, the new function has a larger version number than
    /// that of any previous version (not necessarily exactly 1 greater).
    /// This function's version number will be less than or equal to the value returned by
    /// ICorDebugFunction::GetCurrentVersionNumber.
    /// </summary>
    /// <param name="pnVersion"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetVersionNumber (UInt32* pnVersion);
  }
}