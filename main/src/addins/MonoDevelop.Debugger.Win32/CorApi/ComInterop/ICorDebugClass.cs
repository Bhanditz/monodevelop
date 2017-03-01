using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugClass represents a Class (mdTypeDef) in the IL image.
  ///  For generic types, it represents the generic type definition (eg. List&lt;T&gt;) not any of
  ///  the specific instantiations (eg. List&lt;int&gt;).
  ///  Use ICorDebugClass2::GetParameterizedType to build an ICorDebugType from an
  ///  ICorDebugClass and type parameters.
  ///  Classes live in a module and are uniquely identified by a mdTypeDef.
  ///  In other words, you can round-trip a class like so:
  ///     ICorDebugClass * pClass1 = ...; // some initial class
  ///     ICorDebugModule * pModule = NULL;
  ///     pClass1-&gt;GetModule(&amp;pModule);
  ///     mdTypeDef token;
  ///     pClass1-&gt;GetToken(&amp;token);
  ///     ICorDebugClass * pClass2;
  ///     pModule-&gt;GetClassFromToken(token, &amp;pClass2);
  ///     // Now: pClass1 == pClass2
  /// </summary>
  /// <example><code>
  ///  
  ///  ICorDebugClass represents a Class (mdTypeDef) in the IL image.
  ///  For generic types, it represents the generic type definition (eg. List&lt;T&gt;) not any of 
  ///  the specific instantiations (eg. List&lt;int&gt;). 
  ///  
  ///  Use ICorDebugClass2::GetParameterizedType to build an ICorDebugType from an
  ///  ICorDebugClass and type parameters.
  /// 
  ///  Classes live in a module and are uniquely identified by a mdTypeDef. 
  ///  In other words, you can round-trip a class like so:
  ///     ICorDebugClass * pClass1 = ...; // some initial class
  ///     
  ///     ICorDebugModule * pModule = NULL;    
  ///     pClass1-&gt;GetModule(&amp;pModule);
  /// 
  ///     mdTypeDef token;
  ///     pClass1-&gt;GetToken(&amp;token);
  ///     
  ///     ICorDebugClass * pClass2;
  ///     pModule-&gt;GetClassFromToken(token, &amp;pClass2); 
  ///     // Now: pClass1 == pClass2 
  /// 
  /// */
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAF5-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugClass : IUnknown
  /// {
  ///     /*
  ///      * GetModule returns the module for the class.
  ///      */
  /// 
  ///     HRESULT GetModule([out] ICorDebugModule **pModule);
  /// 
  ///     /*
  ///      * GetTypeDefToken returns the metadata typedef token for the class.
  ///      */
  /// 
  ///     HRESULT GetToken([out] mdTypeDef *pTypeDef);
  /// 
  ///     /*
  ///      * GetStaticFieldValue returns a value object (ICorDebugValue) for the given static field
  ///      * variable. If the static field could possibly be relative to either
  ///      * a thread, context, or appdomain, then pFrame will help the debugger
  ///      * determine the proper value.
  ///      *
  ///      * Note that if the class accepts type parameters, then you should
  ///      * use GetStaticField on an appropriate ICorDebugType rather than on the
  ///      * ICorDebugClass.
  ///      * 
  ///      * Returns:
  ///      *  S_OK on success.
  ///      *  CORDBG_E_FIELD_NOT_STATIC if the field is not static.
  ///      *  CORDBG_E_STATIC_VAR_NOT_AVAILABLE if field is not yet available (storage for statics
  ///      *    may be lazily allocated). 
  ///      *  CORDBG_E_VARIABLE_IS_ACTUALLY_LITERAL if the field is actually a metadata literal. In this
  ///      *    case, the debugger should get the value from the metadata.
  ///      *  error on other errors.
  ///      */
  /// 
  ///     HRESULT GetStaticFieldValue([in] mdFieldDef fieldDef,
  ///                                 [in] ICorDebugFrame *pFrame,
  ///                                 [out] ICorDebugValue **ppValue);
  /// };
  ///  </code></example>
  [Guid ("CC7BCAF5-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugClass
  {
    /// <summary>
    /// GetModule returns the module for the class.
    /// </summary>
    /// <param name="pModule"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetModule ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModule pModule);

    /// <summary>
    /// GetTypeDefToken returns the metadata typedef token for the class.
    /// </summary>
    /// <param name="pTypeDef"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetToken ([ComAliasName ("mdTypeDef")] UInt32* pTypeDef);

    /// <summary>
    /// GetStaticFieldValue returns a value object (ICorDebugValue) for the given static field
    /// variable. If the static field could possibly be relative to either
    /// a thread, context, or appdomain, then pFrame will help the debugger
    /// determine the proper value.
    /// Note that if the class accepts type parameters, then you should
    /// use GetStaticField on an appropriate ICorDebugType rather than on the
    /// ICorDebugClass.
    /// Returns:
    ///  S_OK on success.
    ///  CORDBG_E_FIELD_NOT_STATIC if the field is not static.
    ///  CORDBG_E_STATIC_VAR_NOT_AVAILABLE if field is not yet available (storage for statics
    ///    may be lazily allocated).
    ///  CORDBG_E_VARIABLE_IS_ACTUALLY_LITERAL if the field is actually a metadata literal. In this
    ///    case, the debugger should get the value from the metadata.
    ///  error on other errors.
    /// </summary>
    /// <param name="fieldDef"></param>
    /// <param name="pFrame"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStaticFieldValue ([In] UInt32 fieldDef, [MarshalAs (UnmanagedType.Interface)] [In] ICorDebugFrame pFrame, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);
  }
}