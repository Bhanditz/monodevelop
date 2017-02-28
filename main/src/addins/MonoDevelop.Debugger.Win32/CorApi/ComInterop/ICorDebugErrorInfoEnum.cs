using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /// <summary>
    /// 
    /// </summary>
    /// <example><code>
    ////* ------------------------------------------------------------------------- *
    /// * DEPRECATED
    /// *
    /// * ICorDebugErrorInfoEnum interface
    /// *
    /// * ------------------------------------------------------------------------- */
    ///[
    ///    object,
    ///    local,
    ///    uuid(F0E18809-72B5-11d2-976F-00A0C9B4D50C),
    ///    pointer_default(unique)
    ///]
    ///interface ICorDebugErrorInfoEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * DEPRECATED
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                    ICorDebugEditAndContinueErrorInfo *errors[],
    ///                 [out] ULONG *pceltFetched);
    ///}; </code></example>
    [Guid ("F0E18809-72B5-11D2-976F-00A0C9B4D50C")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
  [Obsolete("DEPRECATED")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugErrorInfoEnum : ICorDebugEnum
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
        /// DEPRECATED
        /// </summary>
        /// <param name="celt"></param>
        /// <param name="errors"></param>
        /// <param name="pceltFetched"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Next ([In] UInt32 celt, void** errors, [Out] UInt32* pceltFetched);
    }
}