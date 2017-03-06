using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugValue represents a value in the remote process.  Note that
  /// the values can be both Get and Set; they are "lvalues".
  /// 
  /// In general, ownership of a value object is passed when it is returned. The
  /// recipient is responsible for removing a reference from the object when
  /// finished with it.
  /// 
  /// Depending on where the value was retrieved from, the value may not remain
  /// valid after the process is resumed,
  /// so in general they shouldn't be held across continues. 
  /// </summary>
  /// <example><code>
  /// 
  ////* ------------------------------------------------------------------------- *
  /// * Runtime value interfaces
  /// * ------------------------------------------------------------------------- */
  ///
  ////*
  /// * ICorDebugValue represents a value in the remote process.  Note that
  /// * the values can be both Get and Set; they are "lvalues".
  /// *
  /// * In general, ownership of a value object is passed when it is returned. The
  /// * recipient is responsible for removing a reference from the object when
  /// * finished with it.
  /// *
  /// * Depending on where the value was retrieved from, the value may not remain
  /// * valid after the process is resumed,
  /// * so in general they shouldn't be held across continues.
  /// */
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(CC7BCAF7-8A68-11d2-983C-0000F808342D),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugValue : IUnknown
  ///{
  ///    /*
  ///     * GetType returns the simple type of the value.  If the object
  ///     * has a more complex runtime type, that type may be examined through the
  ///     * appropriate subclasses (e.g. ICorDebugObjectValue can get the class of
  ///     * an object.)
  ///     */
  ///
  ///    HRESULT GetType([out] CorElementType *pType);
  ///
  ///    /*
  ///     * GetSize returns the size of the value in bytes. Note that for reference
  ///     * types this will be the size of the pointer rather than the size of
  ///     * the object.
  ///     */
  ///
  ///    HRESULT GetSize([out] ULONG32 *pSize);
  ///
  ///    /*
  ///     * GetAddress returns the address of the value in the debugee
  ///     * process.  This might be useful information for the debugger to
  ///     * show.
  ///     *
  ///     * If the value is unavailable, 0 is returned. This could happen if
  ///     * it is at least partly in registers or stored in a GC Handle.
  ///     */
  ///
  ///    HRESULT GetAddress([out] CORDB_ADDRESS *pAddress);
  ///
  ///    /*
  ///     * NOT YET IMPLEMENTED
  ///     */
  ///
  ///    HRESULT CreateBreakpoint([out] ICorDebugValueBreakpoint **ppBreakpoint);
  ///
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCAF7-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugValue
    {
      /// <summary>
      /// GetType returns the simple type of the value.  If the object
      /// has a more complex runtime type, that type may be examined through the
      /// appropriate subclasses (e.g. ICorDebugObjectValue can get the class of
      /// an object.) 
      /// </summary>
      /// <param name="elementType"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetType([Out] CorElementType *elementType);

      /// <summary>
      /// GetSize returns the size of the value in bytes. Note that for reference
      /// types this will be the size of the pointer rather than the size of
      /// the object.
      /// </summary>
      /// <param name="pSize"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetSize (UInt32 *pSize);

      /// <summary>
      /// GetAddress returns the address of the value in the debugee
      /// process.  This might be useful information for the debugger to
      /// show.
      /// 
      /// If the value is unavailable, 0 is returned. This could happen if
      /// it is at least partly in registers or stored in a GC Handle. 
      /// </summary>
      /// <param name="pAddress"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetAddress ([ComAliasName("CORDB_ADDRESS")] UInt64 *pAddress);

      /// <summary>
      /// NOT YET IMPLEMENTED
      /// </summary>
      /// <param name="ppBreakpoint"></param>
      [Obsolete("NOT YET IMPLEMENTED")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);
    }
}