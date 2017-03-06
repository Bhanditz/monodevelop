using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugReferenceValue is a subclass of ICorDebugValue which applies to
  ///  a reference type.
  ///  The runtime may Garbage Collect objects once the debuggee is continued. The GC may
  ///  move objects around in memory.
  ///  An ICorDebugReference will either cooperate with GCs such that its information is updated
  ///  after the GC, or it will be implicitly neutered before the GC.
  ///  The ICorDebugReferenceValue inteface may be implicitly neutered after the debuggee
  ///  has been continued. The derived ICorDebugHandleValue is not neutered until explicitly
  ///  released or exposed.
  /// </summary>
  /// <example><code>
  ///  
  ///  * ICorDebugReferenceValue is a subclass of ICorDebugValue which applies to
  ///  * a reference type.
  ///  * The runtime may Garbage Collect objects once the debuggee is continued. The GC may
  ///  * move objects around in memory.
  ///  *
  ///  * An ICorDebugReference will either cooperate with GCs such that its information is updated
  ///  * after the GC, or it will be implicitly neutered before the GC.
  ///  *
  ///  * The ICorDebugReferenceValue inteface may be implicitly neutered after the debuggee
  ///  * has been continued. The derived ICorDebugHandleValue is not neutered until explicitly
  ///  * released or exposed.
  ///  *
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAF9-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugReferenceValue : ICorDebugValue
  /// {
  ///     /*
  ///      * IsNull tests whether the reference is null.
  ///      */
  /// 
  ///     HRESULT IsNull([out] BOOL *pbNull);
  /// 
  ///     /*
  ///      * GetValue returns the current address of the object referred to by this
  ///      * reference.
  ///      */
  /// 
  ///     HRESULT GetValue([out] CORDB_ADDRESS *pValue);
  /// 
  ///     /*
  ///      * SetValue sets this reference to refer to a different address.
  ///      */
  /// 
  ///     HRESULT SetValue([in] CORDB_ADDRESS value);
  /// 
  ///     /*
  ///      * Dereference returns a ICorDebugValue representing the value
  ///      * referenced. This is only valid while the interface has not yet been neutered.
  ///      */
  /// 
  ///     HRESULT Dereference([out] ICorDebugValue **ppValue);
  /// 
  ///     /*
  ///      * DEPRECATED
  ///      */
  /// 
  ///     HRESULT DereferenceStrong([out] ICorDebugValue **ppValue);
  /// };
  ///  </code></example>
  [Guid ("CC7BCAF9-8A68-11D2-983C-0000F808342D")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugReferenceValue : ICorDebugValue
  {
    /// <summary>
    /// GetType returns the simple type of the value.  If the object
    /// has a more complex runtime type, that type may be examined through the
    /// appropriate subclasses (e.g. ICorDebugObjectValue can get the class of
    /// an object.)
    /// </summary>
    /// <param name="elementType"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetType ([Out] CorElementType* elementType);

    /// <summary>
    /// GetSize returns the size of the value in bytes. Note that for reference
    /// types this will be the size of the pointer rather than the size of
    /// the object.
    /// </summary>
    /// <param name="pSize"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetSize (UInt32* pSize);

    /// <summary>
    /// GetAddress returns the address of the value in the debugee
    /// process.  This might be useful information for the debugger to
    /// show.
    /// If the value is unavailable, 0 is returned. This could happen if
    /// it is at least partly in registers or stored in a GC Handle.
    /// </summary>
    /// <param name="pAddress"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetAddress ([ComAliasName ("CORDB_ADDRESS")] UInt64* pAddress);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="ppBreakpoint"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);

    /// <summary>
    /// IsNull tests whether the reference is null.
    /// </summary>
    /// <param name="pbNull"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsNull (Int32* pbNull);

    /// <summary>
    /// GetValue returns the current address of the object referred to by this reference.
    /// </summary>
    /// <param name="pValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetValue ([ComAliasName ("CORDB_ADDRESS")] UInt64* pValue);

    /// <summary>
    /// SetValue sets this reference to refer to a different address.
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetValue ([In] [ComAliasName ("CORDB_ADDRESS")] UInt64 value);

    /// <summary>
    /// Dereference returns a ICorDebugValue representing the value
    /// referenced. This is only valid while the interface has not yet been neutered.
    /// </summary>
    /// <param name="ppValue"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Dereference ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="ppValue"></param>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 DereferenceStrong ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);
  }
}