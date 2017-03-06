using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugArrayValue is a subclass of ICorDebugValue which applies
  /// to values which contain an array. This interface supports both
  /// single and multidimension arrays.
  /// </summary>
  /// <example><code>
  /// /*
  /// * ICorDebugArrayValue is a subclass of ICorDebugValue which applies
  /// * to values which contain an array. This interface supports both
  /// * single and multidimension arrays.
  /// */
  ///
  ///[
  ///    object,
  ///    local,
  ///    uuid(0405B0DF-A660-11d2-BD02-0000F80849BD),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugArrayValue : ICorDebugHeapValue
  ///{
  ///    /*
  ///     * GetElementType returns the simple type of the elements in the
  ///     * array.
  ///     */
  ///
  ///    HRESULT GetElementType([out] CorElementType *pType);
  ///
  ///    /*
  ///     * GetRank returns the number of dimensions in the array.
  ///     */
  ///
  ///    HRESULT GetRank([out] ULONG32 *pnRank);
  ///
  ///    /*
  ///     * GetCount returns the total number of elements in the array.
  ///     */
  ///
  ///    HRESULT GetCount([out] ULONG32 *pnCount);
  ///
  ///    /*
  ///     * GetDimensions returns the dimensions of the array.
  ///     */
  ///
  ///    HRESULT GetDimensions([in] ULONG32 cdim,
  ///                          [out, size_is(cdim),
  ///                           length_is(cdim)] ULONG32 dims[]);
  ///
  ///    /*
  ///     * HasBaseIndicies returns whether or not the array has base indicies.
  ///     * If the answer is no, then all dimensions have a base index of 0.
  ///     */
  ///
  ///    HRESULT HasBaseIndicies([out] BOOL *pbHasBaseIndicies);
  ///
  ///    /*
  ///     * GetBaseIndicies returns the base index of each dimension in
  ///     * the array
  ///     */
  ///
  ///    HRESULT GetBaseIndicies([in] ULONG32 cdim,
  ///                            [out, size_is(cdim),
  ///                            length_is(cdim)] ULONG32 indicies[]);
  ///
  ///    /*
  ///     * GetElement returns a value representing the given element in the array.
  ///     * The indices array must not be null.
  ///     */
  ///
  ///    HRESULT GetElement([in] ULONG32 cdim,
  ///                       [in, size_is(cdim),
  ///                        length_is(cdim)] ULONG32 indices[],
  ///                       [out] ICorDebugValue **ppValue);
  ///    /*
  ///     * GetElementAtPosition returns the element at the given position,
  ///     * treating the array as a zero-based, single-dimensional array.
  ///     *
  ///     * Multidimensional array layout follows the C++ style of array layout.
  ///     */
  ///
  ///    HRESULT GetElementAtPosition([in] ULONG32 nPosition,
  ///                                 [out] ICorDebugValue **ppValue);
  ///};
  ///
  /// </code></example>
    [Guid ("0405B0DF-A660-11D2-BD02-0000F80849BD")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugArrayValue : ICorDebugHeapValue
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
      /// DEPRECATED.
      ///     * All objects are only valid until Continue is called, at which time they are neutered.
      /// </summary>
      /// <param name="pbValid"></param>
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsValid (Int32* pbValid);

      [Obsolete ("NOT YET IMPLEMENTED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      new [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateRelocBreakpoint ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);

      /// <summary>
      /// GetElementType returns the simple type of the elements in the
      /// array.
      /// </summary>
      /// <param name="elementType"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetElementType (CorElementType* elementType);

      /// <summary>
      /// GetRank returns the number of dimensions in the array.
      /// </summary>
      /// <param name="pnRank"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetRank (UInt32* pnRank);

      /// <summary>
      /// GetCount returns the total number of elements in the array.
      /// </summary>
      /// <param name="pnCount"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCount (UInt32* pnCount);

      /// <summary>
      /// GetDimensions returns the dimensions of the array.
      /// </summary>
      /// <param name="cdim"></param>
      /// <param name="dims"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetDimensions ([In] UInt32 cdim, [Out] UInt32* dims);

      /// <summary>
      /// HasBaseIndicies returns whether or not the array has base indicies.
      /// If the answer is no, then all dimensions have a base index of 0.
      /// </summary>
      /// <param name="pbHasBaseIndicies"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 HasBaseIndicies (Int32* pbHasBaseIndicies);

      /// <summary>
      /// GetBaseIndicies returns the base index of each dimension in
      /// the array
      /// </summary>
      /// <param name="cdim"></param>
      /// <param name="indicies"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetBaseIndicies ([In] UInt32 cdim, [Out] UInt32* indicies);

      /// <summary>
      /// GetElement returns a value representing the given element in the array.
      /// The indices array must not be null.
      /// </summary>
      /// <param name="cdim"></param>
      /// <param name="indices"></param>
      /// <param name="ppValue"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetElement ([In] UInt32 cdim, [In] UInt32* indices, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);

      /// <summary>
      /// GetElementAtPosition returns the element at the given position,
      /// treating the array as a zero-based, single-dimensional array.
      /// Multidimensional array layout follows the C++ style of array layout.
      /// </summary>
      /// <param name="nPosition"></param>
      /// <param name="ppValue"></param>
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetElementAtPosition ([In] UInt32 nPosition, [MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppValue);
    }
}