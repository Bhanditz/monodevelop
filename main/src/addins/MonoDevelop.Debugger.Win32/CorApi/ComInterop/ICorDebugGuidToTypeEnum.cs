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
    ///    uuid(6164D242-1015-4BD6-8CBE-D0DBD4B8275A),
    ///    pointer_default(unique)
    ///]
    ///interface ICorDebugGuidToTypeEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * Gets the next "celt" number of IID / Type pairs from the app domain cache.
    ///     * The actual number of frames retrieved is returned in "pceltFetched".
    ///     * Returns S_FALSE if the actual number of pairs retrieved is smaller
    ///     * than the number of pairs requested.
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                 CorDebugGuidToTypeMapping values[],
    ///                 [out] ULONG* pceltFetched);
    ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("6164D242-1015-4BD6-8CBE-D0DBD4B8275A")]
    [ComImport]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugGuidToTypeEnum : ICorDebugEnum
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
        /// Gets the next "celt" number of IID / Type pairs from the app domain cache.
        /// The actual number of frames retrieved is returned in "pceltFetched".
        /// Returns S_FALSE if the actual number of pairs retrieved is smaller than the number of pairs requested.
        /// </summary>
        /// <param name="celt"></param>
        /// <param name="values"></param>
        /// <param name="pceltFetched"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Next ([In] UInt32 celt, [MarshalAs (UnmanagedType.Interface), Out] CorDebugGuidToTypeMapping[] values, [Out] UInt32* pceltFetched);
    }
}