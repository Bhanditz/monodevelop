using System;
using System.Diagnostics.CodeAnalysis;
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
  ///     uuid(35389FF1-3684-4c55-A2EE-210F26C60E5E),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugNativeFrame2 : IUnknown
  /// {
  ///     /*
  ///      * Returns true if the current frame is a child frame.
  ///      */
  ///     HRESULT IsChild([out] BOOL *pIsChild);
  /// 
  ///     /*
  ///      * Return true if the specified frame is the parent frame of the current frame.
  ///      */
  ///     HRESULT IsMatchingParentFrame([in] ICorDebugNativeFrame2 *pPotentialParentFrame,
  ///                                   [out] BOOL *pIsParent);
  /// 
  ///     /*
  ///      * Return the stack parameter size on x86.  On other platforms, we return S_FALSE and set pSize to 0.
  ///      * This is because other platforms don't need this information for unwinding.
  ///      */
  ///     HRESULT GetStackParameterSize([out] ULONG32 * pSize);
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("35389FF1-3684-4C55-A2EE-210F26C60E5E")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugNativeFrame2
  {
    /// <summary>
    /// Returns true if the current frame is a child frame.
    /// </summary>
    /// <param name="pIsChild"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsChild (Int32* pIsChild);

    /// <summary>
    /// Return true if the specified frame is the parent frame of the current frame.
    /// </summary>
    /// <param name="pPotentialParentFrame"></param>
    /// <param name="pIsParent"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsMatchingParentFrame ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugNativeFrame2 pPotentialParentFrame, Int32* pIsParent);

    /// <summary>
    /// Return the stack parameter size on x86.  On other platforms, we return S_FALSE and set pSize to 0.
    /// This is because other platforms don't need this information for unwinding.
    /// </summary>
    /// <param name="pSize"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStackParameterSize (UInt32* pSize);
  }
}