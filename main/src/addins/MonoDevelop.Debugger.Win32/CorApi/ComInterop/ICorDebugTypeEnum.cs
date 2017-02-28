using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /// <summary>
    /// 
    /// </summary>
    /// <example><code>
    ///[
    ///    object,
    ///    local,
    ///    uuid(10F27499-9DF2-43ce-8333-A321D7C99CB4),
    ///    pointer_default(unique)
    ///]
    ///interface ICorDebugTypeEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * Gets the next "celt" number of types in the enumeration.
    ///     * The actual number of types retrieved is returned in "pceltFetched".
    ///     * Returns S_FALSE if the actual number of types retrieved is smaller
    ///     * than the number of types requested.
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                    ICorDebugType *values[],
    ///                 [out] ULONG *pceltFetched);
    ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("10F27499-9DF2-43CE-8333-A321D7C99CB4")]
    [ComImport]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugTypeEnum : ICorDebugEnum
    {
        /// <summary>
        /// Moves the current position forward the given number of elements.
        /// </summary>
        /// <param name="celt">the given number of elements</param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Skip ([In] UInt32 celt);

        /// <summary>
        /// Sets the position of the enumerator to the beginning of the enumeration.
        /// </summary>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Reset ();

        /// <summary>
        /// Creates another enumerator with the same current position as this one.
        /// </summary>
        /// <param name="ppEnum">another enumerator</param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void Clone ([MarshalAs (UnmanagedType.Interface)] out ICorDebugEnum ppEnum);

        /// <summary>
        /// Gets the number of elements in the enumeration.
        /// </summary>
        /// <param name="pcelt">the number of elements in the enumeration</param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        new void GetCount (UInt32* pcelt);

      /// <summary>
      /// Gets the next "celt" number of types in the enumeration.
      /// The actual number of types retrieved is returned in "pceltFetched".
      /// Returns S_FALSE if the actual number of types retrieved is smaller than the number of types requested.
      /// </summary>
      /// <param name="celt"></param>
      /// <param name="values"></param>
      /// <param name="pceltFetched"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Next ([In] UInt32 celt, [MarshalAs (UnmanagedType.Interface), Out] ICorDebugType[] values, [Out] UInt32* pceltFetched);
    }
}