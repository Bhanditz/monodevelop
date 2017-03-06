using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugObjectValue is a subclass of ICorDebugValue which applies to
  ///  values which contain an object.
  ///  An ICorDebugObjectValue becomes invalid after the debuggee is continued.
  /// </summary>
  /// <example><code>
  ///  
  ///  * ICorDebugObjectValue is a subclass of ICorDebugValue which applies to
  ///  * values which contain an object.
  ///  * An ICorDebugObjectValue becomes invalid after the debuggee is continued.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(18AD3D6E-B7D2-11d2-BD04-0000F80849BD),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugObjectValue : ICorDebugValue
  /// {
  ///     /*
  ///      * GetClass returns the runtime class of the object in the value.
  ///      */
  /// 
  ///     HRESULT GetClass([out] ICorDebugClass **ppClass);
  /// 
  ///     /*
  ///      * GetFieldValue returns a value for the given field in the given
  ///      * class. The class must be on the class hierarchy of the object's
  ///      * class, and the field must be a field of that class.
  ///      */
  /// 
  ///     HRESULT GetFieldValue([in] ICorDebugClass *pClass,
  ///                           [in] mdFieldDef fieldDef,
  ///                           [out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetVirtualMethod([in] mdMemberRef memberRef,
  ///                              [out] ICorDebugFunction **ppFunction);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetContext([out] ICorDebugContext **ppContext);
  /// 
  ///     /*
  ///      * IsValueClass returns true if the the class of this object is
  ///      * a value class.
  ///      */
  /// 
  ///     HRESULT IsValueClass([out] BOOL *pbIsValueClass);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  /// 
  ///     HRESULT GetManagedCopy([out] IUnknown **ppObject);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  /// 
  ///     HRESULT SetFromManagedCopy([in] IUnknown *pObject);
  /// };
  /// 
  ///  </code></example>
  [Guid ("18AD3D6E-B7D2-11D2-BD04-0000F80849BD")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugObjectValue : ICorDebugValue
  {
    /// <summary>
    /// GetType returns the simple type of the value.  If the object
    /// has a more complex runtime type, that type may be examined through the
    /// appropriate subclasses (e.g. ICorDebugObjectValue can get the class of
    /// an object.)
    /// </summary>
    /// <param name="elementType"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetType ([Out] CorElementType* elementType);

    /// <summary>
    /// GetSize returns the size of the value in bytes. Note that for reference
    /// types this will be the size of the pointer rather than the size of
    /// the object.
    /// </summary>
    /// <param name="pSize"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetSize (UInt32* pSize);

    /// <summary>
    /// GetAddress returns the address of the value in the debugee
    /// process.  This might be useful information for the debugger to
    /// show.
    /// If the value is unavailable, 0 is returned. This could happen if
    /// it is at least partly in registers or stored in a GC Handle.
    /// </summary>
    /// <param name="pAddress"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetAddress ([ComAliasName ("CORDB_ADDRESS")] UInt64* pAddress);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="ppBreakpoint"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);

    /// <summary>
    /// GetClass returns the runtime class of the object in the value.
    /// </summary>
    /// <param name="ppClass"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetClass ([MarshalAs (UnmanagedType.Interface)] out ICorDebugClass ppClass);

    /// <summary>
    /// GetFieldValue returns a value for the given field in the given
    /// class. The class must be on the class hierarchy of the object's
    /// class, and the field must be a field of that class.
    /// </summary>
    /// <param name="pClass"></param>
    /// <param name="fieldDef"></param>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFieldValue ([MarshalAs (UnmanagedType.Interface)] [In] ICorDebugClass pClass, [In] [ComAliasName ("mdFieldDef")] UInt32 fieldDef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="memberRef"></param>
    /// <param name="ppFunction"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetVirtualMethod ([In] [ComAliasName ("mdMemberRef")] UInt32 memberRef, [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="ppContext"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetContext ([MarshalAs (UnmanagedType.Interface)] out ICorDebugContext ppContext);

    /// <summary>
    /// IsValueClass returns true if the the class of this object is
    /// a value class.
    /// </summary>
    /// <param name="pbIsValueClass"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsValueClass (Int32* pbIsValueClass);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="ppObject"></param>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetManagedCopy (void** ppObject);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="pObject"></param>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetFromManagedCopy (void** pObject);
  }
}