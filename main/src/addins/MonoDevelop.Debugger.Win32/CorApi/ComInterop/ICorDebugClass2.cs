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
  /// [
  ///     object,
  ///     local,
  ///     uuid(B008EA8D-7AB1-43f7-BB20-FBB5A04038AE),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugClass2 : IUnknown
  /// {
  ///     /*
  ///      * GetParameterizedType returns a type that corresponds to this class.
  ///      *
  ///      * If the class is non-generic, i.e. has no type parameters, then
  ///      * this simply gets the type object corresponding to the class.
  ///      * elementType should be set to the correct element type for the
  ///      * class, i.e. ELEMENT_TYPE_VALUETYPE if the class is a value type
  ///      * otherwise ELEMENT_TYPE_CLASS.
  ///      *
  ///      * If the class accepts type parameters, e.g. ArrayList&lt;T&gt;, then
  ///      * this function can be used to construct a type object for an
  ///      * instantiated type such as ArrayList&lt;int&gt;.
  ///      */
  ///     HRESULT GetParameterizedType([in] CorElementType elementType,
  ///                                  [in] ULONG32 nTypeArgs,
  ///                                  [in, size_is(nTypeArgs)] ICorDebugType *ppTypeArgs[],
  ///                                  [out] ICorDebugType **ppType);
  /// 
  ///     /*
  ///      * Sets the User-code status (for JMC stepping) for all methods
  ///      * in this class. This is functionally equivalent to setting the
  ///      * JMCStatus onall methods in this class.
  ///      * A JMC stepper will skip non-user code.
  ///      * User code must be a subset of debuggable code.
  ///      *
  ///      * Returns S_OK if all methods are set succesfully.
  ///      * Return failure if any are not set.
  ///      * On failure, some may still be set.
  ///      */
  ///     HRESULT SetJMCStatus([in] BOOL bIsJustMyCode);
  /// 
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("B008EA8D-7AB1-43F7-BB20-FBB5A04038AE")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public interface ICorDebugClass2
  {
    /// <summary>
    /// GetParameterizedType returns a type that corresponds to this class.
    /// If the class is non-generic, i.e. has no type parameters, then
    /// this simply gets the type object corresponding to the class.
    /// elementType should be set to the correct element type for the
    /// class, i.e. ELEMENT_TYPE_VALUETYPE if the class is a value type
    /// otherwise ELEMENT_TYPE_CLASS.
    /// If the class accepts type parameters, e.g. ArrayList&lt;T&gt;, then
    /// this function can be used to construct a type object for an
    /// instantiated type such as ArrayList&lt;int&gt;.
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="nTypeArgs"></param>
    /// <param name="ppTypeArgs"></param>
    /// <param name="ppType"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetParameterizedType ([In] CorElementType elementType, [In] UInt32 nTypeArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugType[] ppTypeArgs, [MarshalAs (UnmanagedType.Interface)] ICorDebugType[] ppType);

    /// <summary>
    /// Sets the User-code status (for JMC stepping) for all methods
    /// in this class. This is functionally equivalent to setting the
    /// JMCStatus onall methods in this class.
    /// A JMC stepper will skip non-user code.
    /// User code must be a subset of debuggable code.
    /// Returns S_OK if all methods are set succesfully.
    /// Return failure if any are not set.
    /// On failure, some may still be set.
    /// </summary>
    /// <param name="bIsJustMyCode"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetJMCStatus ([In] Int32 bIsJustMyCode);
  }
}