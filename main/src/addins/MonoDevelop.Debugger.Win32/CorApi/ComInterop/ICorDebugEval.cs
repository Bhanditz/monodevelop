using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugEval collects functionality which requires running code
  ///  inside the debuggee. Note that the operations do not complete until
  ///  ICorDebugProcess::Continue is called, and the EvalComplete callback
  ///  is called.
  ///  An ICorDebugEval object is created in the context of a specific
  ///  thread, which will be used to perform the evaluations.
  ///  If you need to use this functionality without allowing other threads
  ///  to run, set the DebugState of the program's threads to STOP
  ///  before calling Continue.
  ///  Note that since user code is running when the evaluation is in
  ///  progress, any debug events can occur, including class loads,
  ///  breakpoints, etc. Callbacks will be called normally in such a
  ///  case. The state of the Eval will be seen as part of the normal
  ///  program state inspection, the stack chain will be a CHAIN_FUNC_EVAL chain;
  ///  the full debugger API continues to operate as normal. Evals can even be nested.
  ///  Also, the user code may never complete due to deadlock or infinite
  ///  looping. In this case you will need to Abort the Eval before
  ///  resuming the program.
  ///  All objects and types used in a given func-eval must all reside within the
  ///  same app domain. That app-domain need not be the same as the current
  ///  app domain of the thread.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugEval collects functionality which requires running code
  ///  * inside the debuggee. Note that the operations do not complete until
  ///  * ICorDebugProcess::Continue is called, and the EvalComplete callback
  ///  * is called.
  ///  *
  ///  * An ICorDebugEval object is created in the context of a specific
  ///  * thread, which will be used to perform the evaluations.
  ///  *
  ///  * If you need to use this functionality without allowing other threads
  ///  * to run, set the DebugState of the program's threads to STOP
  ///  * before calling Continue.
  ///  *
  ///  * Note that since user code is running when the evaluation is in
  ///  * progress, any debug events can occur, including class loads,
  ///  * breakpoints, etc. Callbacks will be called normally in such a
  ///  * case. The state of the Eval will be seen as part of the normal
  ///  * program state inspection, the stack chain will be a CHAIN_FUNC_EVAL chain;
  ///  * the full debugger API continues to operate as normal. Evals can even be nested.
  ///  *
  ///  * Also, the user code may never complete due to deadlock or infinite
  ///  * looping. In this case you will need to Abort the Eval before
  ///  * resuming the program.
  ///  *
  ///  * All objects and types used in a given func-eval must all reside within the
  ///  * same app domain. That app-domain need not be the same as the current
  ///  * app domain of the thread.
  ///  *
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAF6-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugEval : IUnknown
  /// {
  ///     /*
  ///      * CallFunction sets up a function call.  Note that if the function
  ///      * is virtual, this will perform virtual dispatch.  If the function is
  ///      * not static, then the first argument must be the "this" object.
  ///      * If the function is in an a different AppDomain, a transition will 
  ///      * occur (but all arguments must also be in the target AppDomain)
  ///      */
  /// 
  ///     HRESULT CallFunction([in] ICorDebugFunction *pFunction,
  ///                          [in] ULONG32 nArgs,
  ///                          [in, size_is(nArgs)] ICorDebugValue *ppArgs[]);
  /// 
  ///     /*
  ///      * NewObject allocates and calls the constructor for an object.
  ///      */
  /// 
  ///     HRESULT NewObject([in] ICorDebugFunction *pConstructor,
  ///                       [in] ULONG32 nArgs,
  ///                       [in, size_is(nArgs)] ICorDebugValue *ppArgs[]);
  /// 
  ///     /*
  ///      * NewObjectNoConstructor allocates a new object without
  ///      * attempting to call any constructor on the object.
  ///      */
  /// 
  ///     HRESULT NewObjectNoConstructor([in] ICorDebugClass *pClass);
  /// 
  ///     /*
  ///      * NewString allocates a string object with the given contents.
  ///      * The string is always created in the AppDomain the thread is currently in.
  ///      */
  /// 
  ///     HRESULT NewString([in] LPCWSTR string);
  /// 
  ///     /*
  ///      * NewArray allocates a new array with the given element type and
  ///      * dimensions. If the elementType is a primitive, pElementClass
  ///      * may be NULL. Otherwise, pElementClass should be the class of
  ///      * the elements of the array. Note: lowBounds is optional. If
  ///      * omitted, a zero lower bound for each dimension is assumed.
  ///      * The array is always created in the AppDomain the thread is currently in.
  ///      *
  ///      * NOTE: In the current release, rank must be 1.
  ///      */
  /// 
  ///     HRESULT NewArray([in] CorElementType elementType,
  ///                      [in] ICorDebugClass *pElementClass,
  ///                      [in] ULONG32 rank,
  ///                      [in, size_is(rank)] ULONG32 dims[],
  ///                      [in, size_is(rank)] ULONG32 lowBounds[]);
  /// 
  ///     /*
  ///      * IsActive returns whether the func-eval is currently executing.
  ///      */
  /// 
  ///     HRESULT IsActive([out] BOOL *pbActive);
  /// 
  ///     /*
  ///      * Abort aborts the current computation.  Note that in the case of nested
  ///      * Evals, this may fail unless it is the most recent Eval.
  ///      */
  /// 
  ///     HRESULT Abort();
  /// 
  ///     /*
  ///      * GetResult returns the result of the evaluation.  This is only
  ///      * valid after the evaluation is completed.
  ///      *
  ///      * If the evaluation completes normally, the result will be the
  ///      * return value.  If it terminates with an exception, the result
  ///      * is the exception thrown. If the evaluation was for a new object,
  ///      * the return value is the reference to the object.
  ///      */
  /// 
  ///     HRESULT GetResult([out] ICorDebugValue **ppResult);
  /// 
  ///     /*
  ///      * GetThread returns the thread on which this eval will run or is running.
  ///      */
  /// 
  ///     HRESULT GetThread([out] ICorDebugThread **ppThread);
  /// 
  ///     /*
  ///      * CreateValue creates an ICorDebugValue of the given type for the
  ///      * sole purpose of using it in a function evaluation. These can be
  ///      * used to pass user constants as parameters. The value has a zero
  ///      * or NULL initial value. Use ICorDebugValue::SetValue to
  ///      * set the value.
  ///      *
  ///      * pElementClass is only required for value classes. Pass NULL
  ///      * otherwise.
  ///      *
  ///      * If elementType == ELEMENT_TYPE_CLASS, then you get an
  ///      * ICorDebugReferenceValue representing the NULL object reference.
  ///      * You can use this to pass NULL to evals that have object reference
  ///      * parameters. You cannot set the ICorDebugReferenceValue to
  ///      * anything... it always remains NULL.
  ///      */
  /// 
  ///     HRESULT CreateValue([in] CorElementType elementType,
  ///                         [in] ICorDebugClass *pElementClass,
  ///                         [out] ICorDebugValue **ppValue);
  /// }; 
  ///  </code></example>
  [Guid ("CC7BCAF6-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugEval
  {
    /// <summary>
    /// CallFunction sets up a function call.  Note that if the function
    /// is virtual, this will perform virtual dispatch.  If the function is
    /// not static, then the first argument must be the "this" object.
    /// If the function is in an a different AppDomain, a transition will
    /// occur (but all arguments must also be in the target AppDomain)
    /// </summary>
    /// <param name="pFunction"></param>
    /// <param name="nArgs"></param>
    /// <param name="ppArgs"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CallFunction ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pFunction, [In] UInt32 nArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugValue[] ppArgs);

    /// <summary>
    /// NewObject allocates and calls the constructor for an object.
    /// </summary>
    /// <param name="pConstructor"></param>
    /// <param name="nArgs"></param>
    /// <param name="ppArgs"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 NewObject ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFunction pConstructor, [In] UInt32 nArgs, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugValue[] ppArgs);

    /// <summary>
    /// NewObjectNoConstructor allocates a new object without
    /// attempting to call any constructor on the object.
    /// </summary>
    /// <param name="pClass"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 NewObjectNoConstructor ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugClass pClass);

    /// <summary>
    /// NewString allocates a string object with the given contents.
    /// The string is always created in the AppDomain the thread is currently in.
    /// </summary>
    /// <param name="string"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 NewString (UInt16* @string);

    /// <summary>
    /// NewArray allocates a new array with the given element type and
    /// dimensions. If the elementType is a primitive, pElementClass
    /// may be NULL. Otherwise, pElementClass should be the class of
    /// the elements of the array. Note: lowBounds is optional. If
    /// omitted, a zero lower bound for each dimension is assumed.
    /// The array is always created in the AppDomain the thread is currently in.
    /// NOTE: In the current release, rank must be 1.
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="pElementClass"></param>
    /// <param name="rank"></param>
    /// <param name="dims"></param>
    /// <param name="lowBounds"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 NewArray ([In] CorElementType elementType, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugClass pElementClass, [In] UInt32 rank, [In] UInt32* dims, [In] UInt32* lowBounds);

    /// <summary>
    /// IsActive returns whether the func-eval is currently executing.
    /// </summary>
    /// <param name="pbActive"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsActive (Int32* pbActive);

    /// <summary>
    /// Abort aborts the current computation.  Note that in the case of nested
    /// Evals, this may fail unless it is the most recent Eval.
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Abort ();

    /// <summary>
    /// GetResult returns the result of the evaluation.  This is only
    /// valid after the evaluation is completed.
    /// If the evaluation completes normally, the result will be the
    /// return value.  If it terminates with an exception, the result
    /// is the exception thrown. If the evaluation was for a new object,
    /// the return value is the reference to the object.
    /// </summary>
    /// <param name="ppResult"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetResult ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppResult);

    /// <summary>
    /// GetThread returns the thread on which this eval will run or is running.
    /// </summary>
    /// <param name="ppThread"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetThread ([MarshalAs (UnmanagedType.Interface)] out ICorDebugThread ppThread);

    /// <summary>
    /// CreateValue creates an ICorDebugValue of the given type for the
    /// sole purpose of using it in a function evaluation. These can be
    /// used to pass user constants as parameters. The value has a zero
    /// or NULL initial value. Use ICorDebugValue::SetValue to
    /// set the value.
    /// pElementClass is only required for value classes. Pass NULL
    /// otherwise.
    /// If elementType == ELEMENT_TYPE_CLASS, then you get an
    /// ICorDebugReferenceValue representing the NULL object reference.
    /// You can use this to pass NULL to evals that have object reference
    /// parameters. You cannot set the ICorDebugReferenceValue to
    /// anything... it always remains NULL.
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="pElementClass"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateValue ([In] CorElementType elementType, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugClass pElementClass, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);
  }
}