using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugGenericValue is a subclass of ICorDebugValue which applies to
  ///  all values, and can be used to get &amp; set the value.  It is a
  ///  separate subinterface because it is non-remotable.
  ///  Note that for reference types, the value is the reference rather than
  ///  the contents.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugGenericValue is a subclass of ICorDebugValue which applies to
  ///  * all values, and can be used to get &amp; set the value.  It is a
  ///  * separate subinterface because it is non-remotable.
  ///  *
  ///  * Note that for reference types, the value is the reference rather than
  ///  * the contents.
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAF8-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugGenericValue : ICorDebugValue
  /// {
  ///     /*
  ///      * GetValue copies the value into the specified buffer.  The buffer should
  ///      * be the appropriate size for the simple type.
  ///      */
  /// 
  ///     HRESULT GetValue([out] void *pTo);
  /// 
  ///     /*
  ///      * SetValue copies a new value from the specified buffer. The buffer should
  ///      * be the approprirate size for the simple type.
  ///      *
  ///      */
  /// 
  ///     HRESULT SetValue([in] void *pFrom);
  /// };
  /// 
  ///  </code></example>
  [Guid ("CC7BCAF8-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public unsafe interface ICorDebugGenericValue : ICorDebugValue
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
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetSize (uint* pSize);

    /// <summary>
    /// GetAddress returns the address of the value in the debugee
    /// process.  This might be useful information for the debugger to
    /// show.
    /// If the value is unavailable, 0 is returned. This could happen if
    /// it is at least partly in registers or stored in a GC Handle.
    /// </summary>
    /// <param name="pAddress"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetAddress ([ComAliasName ("CORDB_ADDRESS")] ulong* pAddress);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="ppBreakpoint"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);

    /// <summary>
    /// GetValue copies the value into the specified buffer.  The buffer should
    /// be the appropriate size for the simple type.
    /// </summary>
    /// <param name="pTo"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetValue ([Out] void* pTo);

    /// <summary>
    /// SetValue copies a new value from the specified buffer. The buffer should
    /// be the approprirate size for the simple type.
    /// </summary>
    /// <param name="pFrom"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetValue ([In] void* pFrom);
  }
}