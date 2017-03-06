using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugHeapValue is a subclass of ICorDebugValue which represents a garbage collected object.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugHeapValue is a subclass of ICorDebugValue which represents
  ///  * a garbage collected object
  ///  */
  /// 
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAFA-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugHeapValue : ICorDebugValue
  /// {
  ///     /*
  ///      * DEPRECATED.
  ///      * All objects are only valid until Continue is called, at which time they are neutered.
  ///      */
  /// 
  ///     HRESULT IsValid([out] BOOL *pbValid);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT CreateRelocBreakpoint([out]
  ///                                   ICorDebugValueBreakpoint **ppBreakpoint);
  /// 
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("CC7BCAFA-8A68-11D2-983C-0000F808342D")]
  [ComImport]
  public unsafe interface ICorDebugHeapValue : ICorDebugValue
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
    /// DEPRECATED.
    ///     * All objects are only valid until Continue is called, at which time they are neutered.
    /// </summary>
    /// <param name="pbValid"></param>
    [Obsolete ("DEPRECATED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsValid (int* pbValid);

    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateRelocBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);
  }
}