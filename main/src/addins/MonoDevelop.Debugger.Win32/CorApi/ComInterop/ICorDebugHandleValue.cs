using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugHandleValue represents a reference value that the debugger has
  ///  explicitly created a GC handle to. It does not represent GC Handles in the debuggee process,
  ///  A normal ICorDebugReference becomes neutered after the debuggee has been
  ///  continued. A ICorDebugHandleValue will survive across continues and can be
  ///  dereferenced until the client explcitly disposes the handle.
  ///  ICorDebugHeapValu2::CreateHandle will create ICorDebugHandleValue
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugHandleValue represents a reference value that the debugger has
  ///  * explicitly created a GC handle to. It does not represent GC Handles in the debuggee process,
  /// 
  ///  * A normal ICorDebugReference becomes neutered after the debuggee has been
  ///  * continued. A ICorDebugHandleValue will survive across continues and can be
  ///  * dereferenced until the client explcitly disposes the handle.
  ///  *
  ///  *
  ///  * ICorDebugHeapValu2::CreateHandle will create ICorDebugHandleValue
  ///  */
  /// [
  ///     object,
  ///     local,
  ///     uuid(029596E8-276B-46a1-9821-732E96BBB00B),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugHandleValue : ICorDebugReferenceValue
  /// {
  ///     /*
  ///       * returns the type of this handle.
  ///       *
  ///       */
  ///     HRESULT GetHandleType([out] CorDebugHandleType *pType);
  /// 
  /// 
  ///     /*
  ///       * The final release of the interface will also dispose of the handle. This
  ///       * API provides the ability for client to early dispose the handle.
  ///       *
  ///       */
  ///     HRESULT Dispose();
  /// 
  /// };
  /// 
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("029596E8-276B-46A1-9821-732E96BBB00B")]
  [ComImport]
  public unsafe interface ICorDebugHandleValue : ICorDebugReferenceValue
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
    /// IsNull tests whether the reference is null.
    /// </summary>
    /// <param name="pbNull"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 IsNull (int* pbNull);

    /// <summary>
    /// GetValue returns the current address of the object referred to by this reference.
    /// </summary>
    /// <param name="pValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 GetValue ([ComAliasName ("CORDB_ADDRESS")] ulong* pValue);

    /// <summary>
    /// SetValue sets this reference to refer to a different address.
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 SetValue ([In] [ComAliasName ("CORDB_ADDRESS")] ulong value);

    /// <summary>
    /// Dereference returns a ICorDebugValue representing the value
    /// referenced. This is only valid while the interface has not yet been neutered.
    /// </summary>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Dereference ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="ppValue"></param>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 DereferenceStrong ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// returns the type of this handle.
    /// </summary>
    /// <param name="pType"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetHandleType (CorDebugHandleType* pType);

    /// <summary>
    /// The final release of the interface will also dispose of the handle. This
    /// API provides the ability for client to early dispose the handle.
    /// </summary>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Dispose ();
  }
}