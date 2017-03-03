using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    /// </summary>
    /// <example><code>
    /// [
    ///     object,
    ///     local,
    ///     uuid(55E96461-9645-45e4-A2FF-0367877ABCDE),
    ///     pointer_default(unique)
    /// ]
    /// interface ICorDebugCodeEnum : ICorDebugEnum
    /// {
    ///     /*
    ///      * Gets the next "celt" number of code objects in the enumeration.
    ///      * The actual number of code objects retrieved is returned in "pceltFetched".
    ///      * Returns S_FALSE if the actual number of code objects retrieved is smaller
    ///      * than the number of code objects requested.
    ///      */
    ///     HRESULT Next([in] ULONG celt,
    ///                  [out, size_is(celt), length_is(*pceltFetched)]
    ///                     ICorDebugCode *values[],
    ///                  [out] ULONG *pceltFetched);
    /// }; </code></example>
    [Guid("55E96461-9645-45E4-A2FF-0367877ABCDE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugCodeEnum : ICorDebugEnum
    {
        /// <summary>
        /// Moves the current position forward the given number of elements.
        /// </summary>
        /// <param name="celt">the given number of elements</param>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        new Int32 Skip([In] UInt32 celt);

        /// <summary>
        /// Sets the position of the enumerator to the beginning of the enumeration.
        /// </summary>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        new Int32 Reset();

        /// <summary>
        /// Creates another enumerator with the same current position as this one.
        /// </summary>
        /// <param name="ppEnum">another enumerator</param>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        new Int32 Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);

        /// <summary>
        /// Gets the number of elements in the enumeration.
        /// </summary>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        new Int32 GetCount([Out] UInt32* pcelt);

        /// <summary>
        /// Gets the next "celt" number of code objects in the enumeration.
        /// The actual number of code objects retrieved is returned in "pceltFetched".
        /// Returns S_FALSE if the actual number of code objects retrieved is smaller than the number of code objects requested. .
        /// </summary>
        /// <param name="celt"></param>
        /// <param name="values"></param>
        /// <param name="pceltFetched"></param>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        Int32 Next([In] UInt32 celt, void** values, [Out] UInt32* pceltFetched);
    }
}