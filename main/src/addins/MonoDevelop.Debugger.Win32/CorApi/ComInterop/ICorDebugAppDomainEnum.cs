using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
    /// <summary>
    /// 
    /// </summary>
    /// <example><code>
    ////* ------------------------------------------------------------------------- *
    /// * AppDomainEnum interface
    /// * ------------------------------------------------------------------------- */
    ///
    ///[
    ///    object,
    ///    local,
    ///    uuid(63ca1b24-4359-4883-bd57-13f815f58744),
    ///    pointer_default(unique)
    ///]
    ///
    ///interface ICorDebugAppDomainEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * Gets the next "celt" app domains in the enumeration
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                    ICorDebugAppDomain *values[],
    ///                 [out] ULONG *pceltFetched);
    ///
    ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("63CA1B24-4359-4883-BD57-13F815F58744")]
    [ComImport]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugAppDomainEnum : ICorDebugEnum
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
        /// Gets the next "celt" app domains in the enumeration
        /// </summary>
        /// <param name="celt"></param>
        /// <param name="values"></param>
        /// <param name="pceltFetched"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Next ([In] UInt32 celt, [MarshalAs (UnmanagedType.Interface), Out] ICorDebugAppDomain[] values, [Out] UInt32 *pceltFetched);
    }
}