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
    ///    uuid(CC7BCB05-8A68-11d2-983C-0000F808342D),
    ///    pointer_default(unique)
    ///]
    ///interface ICorDebugProcessEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * Gets the next "celt" number of processes in the enumeration.
    ///     * The actual number of processes retrieved is returned in "pceltFetched".
    ///     * Returns S_FALSE if the actual number of processes retrieved is smaller
    ///     * than the number of processes requested.
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                    ICorDebugProcess *processes[],
    ///                 [out] ULONG *pceltFetched);
    ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("CC7BCB05-8A68-11D2-983C-0000F808342D")]
    [ComImport]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugProcessEnum : ICorDebugEnum
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
      /// Gets the next "celt" number of processes in the enumeration.
      /// The actual number of processes retrieved is returned in "pceltFetched".
      /// Returns S_FALSE if the actual number of processes retrieved is smaller than the number of processes requested.
      /// </summary>
      /// <param name="celt"></param>
      /// <param name="processes"></param>
      /// <param name="pceltFetched"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Next ([In] UInt32 celt, [MarshalAs (UnmanagedType.Interface), Out] ICorDebugProcess[] processes, [Out] UInt32* pceltFetched);
    }
}