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
    ///    uuid(CC7BCB06-8A68-11d2-983C-0000F808342D),
    ///    pointer_default(unique)
    ///]
    ///interface ICorDebugThreadEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * Gets the next "celt" number of threads in the enumeration.
    ///     * The actual number of threads retrieved is returned in "pceltFetched".
    ///     * Returns S_FALSE if the actual number of threads retrieved is smaller
    ///     * than the number of threads requested.
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                    ICorDebugThread *threads[],
    ///                 [out] ULONG *pceltFetched);
    ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCB06-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugThreadEnum : ICorDebugEnum
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
      /// Gets the next "celt" number of threads in the enumeration.
      /// The actual number of threads retrieved is returned in "pceltFetched".
      /// Returns S_FALSE if the actual number of threads retrieved is smaller than the number of threads requested.
      /// </summary>
      /// <param name="celt"></param>
      /// <param name="threads"></param>
      /// <param name="pceltFetched"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Next ([In] UInt32 celt, [MarshalAs (UnmanagedType.Interface), Out] ICorDebugThread[] threads, [Out] UInt32* pceltFetched);
    }
}