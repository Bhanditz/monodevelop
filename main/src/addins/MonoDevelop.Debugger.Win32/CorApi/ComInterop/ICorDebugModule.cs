using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugModule represents a Common Language Runtime module that is loaded into a
  ///  specific AppDomain.  Normally this is an executable or a DLL, but it may also be
  ///  some other file of a multi-module assembly.  There is an ICorDebugModule instance
  ///  for each AppDomain a module is loaded into, even in the case of shared modules
  ///  like
  ///  mscorlib.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugModule represents a Common Language Runtime module that is loaded into a
  ///  * specific AppDomain.  Normally this is an executable or a DLL, but it may also be
  ///  * some other file of a multi-module assembly.  There is an ICorDebugModule instance
  ///  * for each AppDomain a module is loaded into, even in the case of shared modules like
  ///  * mscorlib.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(dba2d8c1-e5c5-4069-8c13-10a7c6abf43d),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugModule : IUnknown
  /// {
  ///     /*
  ///      * GetProcess returns the process of which this module is a part.
  ///      */
  /// 
  ///     HRESULT GetProcess([out] ICorDebugProcess **ppProcess);
  /// 
  ///     /*
  ///      * GetBaseAddress returns the base address of the module.
  ///      *
  ///      * For modules loaded from NGEN images, the base address will be 0.
  ///      */
  /// 
  ///     HRESULT GetBaseAddress([out] CORDB_ADDRESS *pAddress);
  /// 
  ///     /*
  ///      * GetAssembly returns the assembly of which this module is a part.
  ///      */
  /// 
  ///     HRESULT GetAssembly([out] ICorDebugAssembly **ppAssembly);
  /// 
  ///     /*
  ///      * GetName returns a name identifying the module.
  ///      *
  ///      * For on-disk modules this is a full path.  For dynamic modules this 
  ///      * is just the filename if one was provided.  Otherwise, and for other 
  ///      * in-memory modules, this is just the simple name stored in the module's
  ///      * metadata.
  ///      */
  /// 
  ///     HRESULT GetName([in] ULONG32 cchName,
  ///                     [out] ULONG32 *pcchName,
  ///                     [out, size_is(cchName),
  ///                     length_is(*pcchName)] WCHAR szName[]);
  /// 
  ///     /*
  ///      * EnableJITDebugging controls whether the jitter preserves
  ///      * debugging information for methods within this module.
  ///      * If bTrackJITInfo is true, then the jitter preserves
  ///      * mapping information between the IL version of a function and
  ///      * the jitted version for functions in the module.  If bAllowJitOpts
  ///      * is true, then the jitter will generate code with certain (JIT-specific)
  ///      * optimizations.
  ///      *
  ///      * JITDebug is enabled by default for all modules loaded when the
  ///      * debugger is active.  Programmatically enabling/disabling these
  ///      * settings will override global settings.
  ///      *
  ///      */
  ///     HRESULT EnableJITDebugging([in] BOOL bTrackJITInfo,
  ///                                [in] BOOL bAllowJitOpts);
  /// 
  ///     /*
  ///      * EnableClassLoadCallbacks controls whether on not LoadClass and
  ///      * UnloadClass callbacks are called for the particular module.
  ///      * For non-dynamic modules, they are off by default.
  ///      * For dynamic modules, they are on by default and can not be disabled.
  ///      */
  /// 
  ///     HRESULT EnableClassLoadCallbacks([in] BOOL bClassLoadCallbacks);
  /// 
  ///     /*
  ///      * GetFunctionFromToken returns the ICorDebugFunction from
  ///      * metadata information. Returns CORDBG_E_FUNCTION_NOT_IL if
  ///      * called with a methodDef that does not refer to an IL method.
  ///      * In the EnC case, this will return the most recent version of the function.
  ///      */
  /// 
  ///     HRESULT GetFunctionFromToken([in] mdMethodDef methodDef,
  ///                                  [out] ICorDebugFunction **ppFunction);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetFunctionFromRVA([in] CORDB_ADDRESS rva,
  ///                                [out] ICorDebugFunction **ppFunction);
  /// 
  ///     /*
  ///      * GetClassFromToken returns the ICorDebugClass from metadata information.
  ///      */
  /// 
  ///     HRESULT GetClassFromToken([in] mdTypeDef typeDef,
  ///                               [out] ICorDebugClass **ppClass);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT CreateBreakpoint([out] ICorDebugModuleBreakpoint **ppBreakpoint);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  /// 
  ///     HRESULT GetEditAndContinueSnapshot([out] ICorDebugEditAndContinueSnapshot **ppEditAndContinueSnapshot);
  /// 
  ///     /*
  ///      * Return a metadata interface pointer that can be used to examine the
  ///      * metadata for this module.
  ///      */
  ///     HRESULT GetMetaDataInterface([in] REFIID riid, [out] IUnknown **ppObj);
  /// 
  /// 
  ///     /*
  ///      * Return the token for the Module table entry for this object.  The token
  ///      * may then be passed to the meta data import api's.
  ///      */
  ///     HRESULT GetToken([out] mdModule *pToken);
  /// 
  ///     /*
  ///      * If this is a dynamic module, IsDynamic sets *pDynamic to true, otherwise
  ///      * sets *pDynamic to false.
  ///      * Dynamic modules can continue to grow new classes (receive LoadClass callbacks) even after
  ///      * the module is loaded.
  ///      */
  ///     HRESULT IsDynamic([out] BOOL *pDynamic);
  /// 
  ///     /*
  ///      * GetGlobalVariableValue returns a value object for the given global
  ///      * variable.
  ///      */
  ///     HRESULT GetGlobalVariableValue([in] mdFieldDef fieldDef,
  ///                                    [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * GetSize returns the size, in bytes, of the module.
  ///      *
  ///      * For modules loaded from NGEN images, the size will be 0.
  ///      */
  ///     HRESULT GetSize([out] ULONG32 *pcBytes);
  /// 
  ///     /*
  ///      * If this is a module that exists only in the debuggee's memory,
  ///      * then pInMemory will be set to TRUE. The Runtime supports
  ///      * loading assemblies from raw streams of bytes. Such modules are
  ///      * called "in memory" modules and they have no on-disk
  ///      * representation.
  ///      */
  ///     HRESULT IsInMemory([out] BOOL *pInMemory);
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("DBA2D8C1-E5C5-4069-8C13-10A7C6ABF43D")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugModule
  {
    /// <summary>
    /// GetProcess returns the process of which this module is a part.
    /// </summary>
    /// <param name="ppProcess"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetProcess ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

    /// <summary>
    /// GetBaseAddress returns the base address of the module.
    /// For modules loaded from NGEN images, the base address will be 0.
    /// </summary>
    /// <param name="pAddress"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetBaseAddress (UInt64* pAddress);

    /// <summary>
    /// GetAssembly returns the assembly of which this module is a part.
    /// </summary>
    /// <param name="ppAssembly"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAssembly ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAssembly ppAssembly);

    /// <summary>
    /// GetName returns a name identifying the module.
    /// For on-disk modules this is a full path.  For dynamic modules this
    /// is just the filename if one was provided.  Otherwise, and for other
    /// in-memory modules, this is just the simple name stored in the module's
    /// metadata.
    /// </summary>
    /// <param name="cchName"></param>
    /// <param name="pcchName"></param>
    /// <param name="szName"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetName ([In] UInt32 cchName, out UInt32 pcchName, [ComAliasName ("WCHAR[]")] UInt16* szName);

    /// <summary>
    /// EnableJITDebugging controls whether the jitter preserves
    /// debugging information for methods within this module.
    /// If bTrackJITInfo is true, then the jitter preserves
    /// mapping information between the IL version of a function and
    /// the jitted version for functions in the module.  If bAllowJitOpts
    /// is true, then the jitter will generate code with certain (JIT-specific)
    /// optimizations.
    /// JITDebug is enabled by default for all modules loaded when the
    /// debugger is active.  Programmatically enabling/disabling these
    /// settings will override global settings.
    /// </summary>
    /// <param name="bTrackJITInfo"></param>
    /// <param name="bAllowJitOpts"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnableJITDebugging ([In] Int32 bTrackJITInfo, [In] Int32 bAllowJitOpts);

    /// <summary>
    /// EnableClassLoadCallbacks controls whether on not LoadClass and
    /// UnloadClass callbacks are called for the particular module.
    /// For non-dynamic modules, they are off by default.
    /// For dynamic modules, they are on by default and can not be disabled.
    /// </summary>
    /// <param name="bClassLoadCallbacks"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnableClassLoadCallbacks ([In] Int32 bClassLoadCallbacks);

    /// <summary>
    /// GetFunctionFromToken returns the ICorDebugFunction from
    /// metadata information. Returns CORDBG_E_FUNCTION_NOT_IL if
    /// called with a methodDef that does not refer to an IL method.
    /// In the EnC case, this will return the most recent version of the function.
    /// </summary>
    /// <param name="methodDef"></param>
    /// <param name="ppFunction"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFunctionFromToken ([In] [ComAliasName ("mdMethodDef")] UInt32 methodDef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFunctionFromRVA ([In] UInt64 rva, void** ppFunction);

    /// <summary>
    /// GetClassFromToken returns the ICorDebugClass from metadata information.
    /// </summary>
    /// <param name="typeDef"></param>
    /// <param name="ppClass"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetClassFromToken ([In] [ComAliasName ("mdTypeDef")] UInt32 typeDef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugClass ppClass);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateBreakpoint (void** ppBreakpoint);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetEditAndContinueSnapshot (void** ppEditAndContinueSnapshot);

    /// <summary>
    ///  Return a metadata interface pointer that can be used to examine the
    ///  metadata for this module.
    /// </summary>
    /// <param name="riid"></param>
    /// <param name="ppObj"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetMetaDataInterface ([In] Guid* riid, void** ppObj);

    /// <summary>
    /// Return the token for the Module table entry for this object.  The token
    /// may then be passed to the meta data import api's.
    /// </summary>
    /// <param name="pToken"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetToken (UInt32* pToken);

    /// <summary>
    /// If this is a dynamic module, IsDynamic sets *pDynamic to true, otherwise
    /// sets *pDynamic to false.
    /// Dynamic modules can continue to grow new classes (receive LoadClass callbacks) even after
    /// the module is loaded.
    /// </summary>
    /// <param name="pDynamic"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsDynamic (Int32* pDynamic);

    /// <summary>
    /// GetGlobalVariableValue returns a value object for the given global
    /// variable.
    /// </summary>
    /// <param name="fieldDef"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetGlobalVariableValue ([In] [ComAliasName ("mdFieldDef")] UInt32 fieldDef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// GetSize returns the size, in bytes, of the module.
    /// For modules loaded from NGEN images, the size will be 0.
    /// </summary>
    /// <param name="pcBytes"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetSize (UInt32* pcBytes);

    /// <summary>
    /// If this is a module that exists only in the debuggee's memory,
    /// then pInMemory will be set to TRUE. The Runtime supports
    /// loading assemblies from raw streams of bytes. Such modules are
    /// called "in memory" modules and they have no on-disk
    /// representation.
    /// </summary>
    /// <param name="pInMemory"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsInMemory (Int32* pInMemory);
  }
}