using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugType represents an instantiated type in the debugggee.
  /// Unlike ICorDebugClass, it can store type-parameter information and thus can
  /// represent instantiated generic types (Eg, List&lt;int&gt;)
  /// Use the metadata interfaces to get static (Compile-time) information about the type.
  /// 
  /// A type (and all of its type parameters) lives in an single AppDomain and becomes 
  /// invalid once the containing ICorDebugAppDomain is unloaded.
  /// 
  /// Types may be lazily loaded, so if the debugger queries for a type that hasn't been
  /// loaded yet, it may be unavailable. 
  /// </summary>
  /// <example><code>
  /// /*
  /// * ICorDebugType represents an instantiated type in the debugggee.
  /// * Unlike ICorDebugClass, it can store type-parameter information and thus can
  /// * represent instantiated generic types (Eg, List&lt;int&gt;)
  /// * Use the metadata interfaces to get static (Compile-time) information about the type.
  /// *
  /// * A type (and all of its type parameters) lives in an single AppDomain and becomes 
  /// * invalid once the containing ICorDebugAppDomain is unloaded.
  /// *
  /// * Types may be lazily loaded, so if the debugger queries for a type that hasn't been
  /// * loaded yet, it may be unavailable.
  /// */
  ///[
  ///    object,
  ///    local,
  ///    uuid(D613F0BB-ACE1-4c19-BD72-E4C08D5DA7F5),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugType : IUnknown
  ///{
  ///    /*
  ///    * GetType gets the basic type of the generic parameter.  This can be used to
  ///    * determine if it is necessary to call GetClass to find the full information for the
  ///    * generic type parameter.
  ///    */
  ///    HRESULT GetType([out] CorElementType *ty);
  ///
  ///    /*
  ///    * GetClass is used if the CorElementType returned by GetType is ELEMENT_TYPE_CLASS,
  ///    * ELEMENT_TYPE_VALUETYPE.  If the type is a constructed type, e.g. List&lt;String&gt;,
  ///    * then this will return the ICorDebugClass for the type constructor, i.e. "List&lt;T&gt;".
  ///    *
  ///    * GetClass should not be used if the element type is anything other than these two element
  ///    * types.  In particular, it may not be used if the element type is ELEMENT_TYPE_STRING.
  ///    */
  ///    HRESULT GetClass([out] ICorDebugClass **ppClass);
  ///
  ///    /*
  ///    * EnumerateTypeParameters may be used if the CorElementType
  ///        * returned by GetType is one of ELEMENT_TYPE_CLASS,
  ///        * ELEMENT_TYPE_VALUETYPE, ELEMENT_TYPE_ARRAY, ELEMENT_TYPE_SZARRAY,
  ///    * ELEMENT_TYPE_BYREF, ELEMENT_TYPE_PTR or ELEMENT_TYPE_FNPTR.
  ///        * It returns the parameters specifying further information about
  ///        * the type.  For example, if the type is "class Dict&lt;String,int32&gt;"
  ///    * then EnumerateTypeParameters will return "String" and "int32"
  ///        * in sequence.
  ///    *
  ///     */
  ///    HRESULT EnumerateTypeParameters([out] ICorDebugTypeEnum **ppTyParEnum);
  ///
  ///    /*
  ///    * GetFirstTypeParameter can be used in those cases where the further
  ///        * information about the type involves at most one type
  ///    * parameter.  You can determine this from the element type returned by
  ///        * GetType.  In particular it may be used with
  ///        * ELEMENT_TYPE_ARRAY, ELEMENT_TYPE_SZARRAY, ELEMENT_TYPE_BYREF
  ///        * or ELEMENT_TYPE_PTR.
  ///     * This can only be called if the type does indeed have a type-parameter.
  ///    */
  ///    HRESULT GetFirstTypeParameter([out] ICorDebugType **value);
  ///
  ///    /*
  ///     * GetBase returns the ICorDebugType object for the base type of this type, if it
  ///     * has one, i.e. if the type is a class type.
  ///     * For example, if
  ///     *        class MyStringDict&lt;T&gt; : Dict&lt;String,T&gt;
  ///     * then the base type of "MyStringDict&lt;int32&gt;" will be "Dict&lt;String,int32&gt;".
  ///     *
  ///     * This is a helper function - you could compute this from EnumerateTypeParemeters,
  ///     * GetClass and the relevant metadata, but it is relatively painful: you would
  ///     * have to lookup the class, then the metadata of that class
  ///     * to find the "generic" base type, then instantiate this generic base type by
  ///     * looking up the type paramaters to the initial type,
  ///     * and then perform the appropriate instantiation in the case where the class
  ///     * happens to be either a generic class or a normal class with a constructed type
  ///     * as its parent.  Looking up the base types is useful to implement common
  ///     * debugger functionality, e.g. printing out all the fields of an object, including its
  ///     * superclasses.
  ///     *
  ///     */
  ///
  ///    HRESULT GetBase([out] ICorDebugType **pBase);
  ///
  ///    /*
  ///     * GetStaticFieldValue returns a value object (ICorDebugValue)
  ///         * for the given static field variable. For non-parameterized
  ///         * types, this is identical to calling GetStaticFieldValue on the
  ///         * ICorDebugClass object returned by ICorDebugType::GetClass.
  ///         * For parameterized types a static field value will be relative to a
  ///         * particular instantiation.  If in addition the static field could
  ///         * possibly be relative to either a thread, context, or appdomain, then pFrame
  ///         * will help the debugger determine the proper value.
  ///         *
  ///         * This may only be used when ICorDebugType::GetType returns
  ///         * ELEMENT_TYPE_CLASS or ELEMENT_TYPE_VALUETYPE.
  ///     */
  ///    HRESULT GetStaticFieldValue([in] mdFieldDef fieldDef,
  ///                                    [in] ICorDebugFrame *pFrame,
  ///                            [out] ICorDebugValue **ppValue);
  ///
  ///
  ///    /*
  ///     * GetRank returns the number of dimensions in an array type
  ///     */
  ///
  ///    HRESULT GetRank([out] ULONG32 *pnRank);
  ///
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("D613F0BB-ACE1-4C19-BD72-E4C08D5DA7F5")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugType
    {
      /// <summary>
      /// GetType gets the basic type of the generic parameter.  This can be used to
      /// determine if it is necessary to call GetClass to find the full information for the
      /// generic type parameter.
      /// </summary>
      /// <param name="ty"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetType ([Out] CorElementType *ty);

      /// <summary>
      /// GetClass is used if the CorElementType returned by GetType is ELEMENT_TYPE_CLASS,
      /// ELEMENT_TYPE_VALUETYPE.  If the type is a constructed type, e.g. List&lt;String&gt;,
      /// then this will return the ICorDebugClass for the type constructor, i.e. "List&lt;T&gt;".
      /// 
      /// GetClass should not be used if the element type is anything other than these two element
      /// types.  In particular, it may not be used if the element type is ELEMENT_TYPE_STRING.
      /// </summary>
      /// <param name="ppClass"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetClass ([MarshalAs (UnmanagedType.Interface)] out ICorDebugClass ppClass);

      /// <summary>
      ///     EnumerateTypeParameters may be used if the CorElementType
      ///         returned by GetType is one of ELEMENT_TYPE_CLASS,
      ///         ELEMENT_TYPE_VALUETYPE, ELEMENT_TYPE_ARRAY, ELEMENT_TYPE_SZARRAY,
      ///     ELEMENT_TYPE_BYREF, ELEMENT_TYPE_PTR or ELEMENT_TYPE_FNPTR.
      ///         It returns the parameters specifying further information about
      ///         the type.  For example, if the type is "class Dict&lt;String,int32&gt;"
      ///     then EnumerateTypeParameters will return "String" and "int32"
      ///         in sequence.
      /// </summary>
      /// <param name="ppTyParEnum"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumerateTypeParameters ([MarshalAs (UnmanagedType.Interface)] out ICorDebugTypeEnum ppTyParEnum);

      /// <summary>
      ///  GetFirstTypeParameter can be used in those cases where the further
      ///     information about the type involves at most one type
      ///  parameter.  You can determine this from the element type returned by
      ///     GetType.  In particular it may be used with
      ///     ELEMENT_TYPE_ARRAY, ELEMENT_TYPE_SZARRAY, ELEMENT_TYPE_BYREF
      ///     or ELEMENT_TYPE_PTR.
      ///  This can only be called if the type does indeed have a type-parameter.
      /// </summary>
      /// <param name="value"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFirstTypeParameter ([MarshalAs (UnmanagedType.Interface)] out ICorDebugType value);

      /// <summary>
      /// GetBase returns the ICorDebugType object for the base type of this type, if it
      /// has one, i.e. if the type is a class type.
      /// For example, if
      ///        class MyStringDict&lt;T&gt; : Dict&lt;String,T&gt;
      /// then the base type of "MyStringDict&lt;int32&gt;" will be "Dict&lt;String,int32&gt;".
      /// 
      /// This is a helper function - you could compute this from EnumerateTypeParemeters,
      /// GetClass and the relevant metadata, but it is relatively painful: you would
      /// have to lookup the class, then the metadata of that class
      /// to find the "generic" base type, then instantiate this generic base type by
      /// looking up the type paramaters to the initial type,
      /// and then perform the appropriate instantiation in the case where the class
      /// happens to be either a generic class or a normal class with a constructed type
      /// as its parent.  Looking up the base types is useful to implement common
      /// debugger functionality, e.g. printing out all the fields of an object, including its
      /// superclasses.
      /// </summary>
      /// <param name="pBase"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetBase ([MarshalAs (UnmanagedType.Interface)] out ICorDebugType pBase);

      /// <summary>
      /// GetStaticFieldValue returns a value object (ICorDebugValue)
      /// for the given static field variable. For non-parameterized
      /// types, this is identical to calling GetStaticFieldValue on the
      /// ICorDebugClass object returned by ICorDebugType::GetClass.
      /// For parameterized types a static field value will be relative to a
      /// particular instantiation.  If in addition the static field could
      /// possibly be relative to either a thread, context, or appdomain, then pFrame
      /// will help the debugger determine the proper value.
      /// 
      /// This may only be used when ICorDebugType::GetType returns
      /// ELEMENT_TYPE_CLASS or ELEMENT_TYPE_VALUETYPE. 
      /// </summary>
      /// <param name="fieldDef"></param>
      /// <param name="pFrame"></param>
      /// <param name="ppValue"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetStaticFieldValue ([In][ComAliasName("mdFieldDef")] UInt32 fieldDef, [MarshalAs (UnmanagedType.Interface), In] ICorDebugFrame pFrame,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

      /// <summary>
      /// GetRank returns the number of dimensions in an array type.
      /// </summary>
      /// <param name="pnRank"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetRank ([Out] UInt32 *pnRank);
    }
}