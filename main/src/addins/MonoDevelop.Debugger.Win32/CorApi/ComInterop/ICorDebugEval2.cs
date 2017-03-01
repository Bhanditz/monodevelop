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
  ///     uuid(FB0D9CE7-BE66-4683-9D32-A42A04E2FD91),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugEval2 : IUnknown
  /// {
  ///     /*
  ///      * CallParameterizedFunction is like CallFunction except the function
  ///      * may be inside a class with type parameters, or may itself take type
  ///      * parameters, or both.  The type arguments should be given for the
  ///      * class first, then the function.
  ///      *
  ///      * If the function is in an a different AppDomain, a transition will occur.
  ///      * However, all type and value arguments must be in the target AppDomain.
  ///      *
  ///      * Func-eval can only be performed in limited scenarios. If Call*Function
  ///      * fails, then the HR makes a best effort at describing the most general
  ///      * possible reason for failure.
  ///      */
  /// 
  ///     HRESULT CallParameterizedFunction([in] ICorDebugFunction *pFunction,
  ///                       [in] ULONG32 nTypeArgs,
  ///                       [in, size_is(nTypeArgs)] ICorDebugType *ppTypeArgs[],
  ///                       [in] ULONG32 nArgs,
  ///                       [in, size_is(nArgs)] ICorDebugValue *ppArgs[]);
  /// 
  ///     /*
  ///      * CreateValueForType generalizes CreateValue by allowing you to specify an
  ///      * arbitrary object type including constructed types such as List&lt;int&gt;.
  ///      * Once again the sole purpose is to generate a value to pass for a function evaluation.
  ///      *
  ///      * The element type of the type must be ELEMENT_TYPE_CLASS or
  ///      * ELEMENT_TYPE_VALUE, or one of the simple types.  You cannot use this
  ///      * to create array values or string values.
  ///      */
  /// 
  ///     HRESULT CreateValueForType([in] ICorDebugType *pType,
  ///                                [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///     * NewParameterizedObject allocates and calls the constructor for an object.
  ///     * The object may be in a class that includes type parameters.
  ///     */
  /// 
  ///     HRESULT NewParameterizedObject([in] ICorDebugFunction *pConstructor,
  ///                                [in] ULONG32 nTypeArgs,
  ///                    [in, size_is(nTypeArgs)] ICorDebugType *ppTypeArgs[],
  ///                            [in] ULONG32 nArgs,
  ///                    [in, size_is(nArgs)] ICorDebugValue *ppArgs[]);
  /// 
  ///     /*
  ///      * NewParameterizedObjectNoConstructor allocates a new object without
  ///      * attempting to call any constructor on the object.
  ///      * The object may be in a class that includes type parameters.
  ///      */
  /// 
  ///     HRESULT NewParameterizedObjectNoConstructor([in] ICorDebugClass *pClass,
  ///                         [in] ULONG32 nTypeArgs,
  ///                         [in, size_is(nTypeArgs)] ICorDebugType *ppTypeArgs[]);
  /// 
  ///     /*
  ///      * NewParamaterizedArray allocates a new array whose elements may be instances
  ///      * of a generic type.  The array is always created in the AppDomain the thread is
  ///      * currently in.
  ///      */
  ///     HRESULT NewParameterizedArray([in] ICorDebugType *pElementType,
  ///                   [in] ULONG32 rank,
  ///                   [in, size_is(rank)] ULONG32 dims[],
  ///                   [in, size_is(rank)] ULONG32 lowBounds[]);
  /// 
  ///    /*
  ///     * NewStringWithLength allocates a string object with the given contents.
  ///     * The length is specified in uiLength. This is used for user to pass in null
  ///     * embedded string. If the string's tailing null is expected to be in
  ///     * the managed string, client has to ensure the length including the tailing null.
  ///     *
  ///     * The string is always created in the AppDomain the thread is currently in.
  ///     */
  /// 
  ///     HRESULT NewStringWithLength([in] LPCWSTR string,
  ///                                [in] UINT  uiLength);
  /// 
  ///     /*
  ///      * RudeAbort aborts the current computation.  Any locks the aborted
  ///      * eval was holding are not released, and thus the debugging session
  ///      * is in an unsafe state.
  ///      */
  /// 
  ///      HRESULT RudeAbort(void);
  /// 
  /// };
  /// 
  ///  </code></example>
  [Guid ("FB0D9CE7-BE66-4683-9D32-A42A04E2FD91")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugEval2
  {
    /// <summary>
    /// CallParameterizedFunction is like CallFunction except the function
    /// may be inside a class with type parameters, or may itself take type
    /// parameters, or both.  The type arguments should be given for the
    /// class first, then the function.
    /// If the function is in an a different AppDomain, a transition will occur.
    /// However, all type and value arguments must be in the target AppDomain.
    /// Func-eval can only be performed in limited scenarios. If Call*Function
    /// fails, then the HR makes a best effort at describing the most general
    /// possible reason for failure.
    /// </summary>
    /// <param name="pFunction"></param>
    /// <param name="nTypeArgs"></param>
    /// <param name="ppTypeArgs"></param>
    /// <param name="nArgs"></param>
    /// <param name="ppArgs"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CallParameterizedFunction ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pFunction, [In] UInt32 nTypeArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugType[] ppTypeArgs, [In] UInt32 nArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugValue[] ppArgs);

    /// <summary>
    /// CreateValueForType generalizes CreateValue by allowing you to specify an
    /// arbitrary object type including constructed types such as List&lt;int&gt;.
    /// Once again the sole purpose is to generate a value to pass for a function evaluation.
    /// The element type of the type must be ELEMENT_TYPE_CLASS or
    /// ELEMENT_TYPE_VALUE, or one of the simple types.  You cannot use this
    /// to create array values or string values.
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateValueForType ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugType pType, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// NewParameterizedObject allocates and calls the constructor for an object.
    /// The object may be in a class that includes type parameters.
    /// </summary>
    /// <param name="pConstructor"></param>
    /// <param name="nTypeArgs"></param>
    /// <param name="ppTypeArgs"></param>
    /// <param name="nArgs"></param>
    /// <param name="ppArgs"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void NewParameterizedObject ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pConstructor, [In] UInt32 nTypeArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugType[] ppTypeArgs, [In] UInt32 nArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugValue[] ppArgs);

    /// <summary>
    /// NewParameterizedObjectNoConstructor allocates a new object without
    /// attempting to call any constructor on the object.
    /// The object may be in a class that includes type parameters.
    /// </summary>
    /// <param name="pClass"></param>
    /// <param name="nTypeArgs"></param>
    /// <param name="ppTypeArgs"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void NewParameterizedObjectNoConstructor ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugClass pClass, [In] UInt32 nTypeArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugType[] ppTypeArgs);

    /// <summary>
    /// NewParamaterizedArray allocates a new array whose elements may be instances
    /// of a generic type.  The array is always created in the AppDomain the thread is
    /// currently in.
    /// </summary>
    /// <param name="pElementType"></param>
    /// <param name="rank"></param>
    /// <param name="dims"></param>
    /// <param name="lowBounds"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void NewParameterizedArray ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugType pElementType, [In] UInt32 rank, [In] UInt32* dims, [In] UInt32* lowBounds);

    /// <summary>
    /// NewStringWithLength allocates a string object with the given contents.
    /// The length is specified in uiLength. This is used for user to pass in null
    /// embedded string. If the string's tailing null is expected to be in
    /// the managed string, client has to ensure the length including the tailing null.
    /// The string is always created in the AppDomain the thread is currently in.
    /// </summary>
    /// <param name="string"></param>
    /// <param name="uiLength"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void NewStringWithLength (UInt16 *@string, [In] UInt32 uiLength);

    /// <summary>
    /// RudeAbort aborts the current computation.  Any locks the aborted
    /// eval was holding are not released, and thus the debugging session
    /// is in an unsafe state.
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RudeAbort ();
  }
}