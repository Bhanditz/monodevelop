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
    ///   /* ------------------------------------------------------------------------- *
    ///  * BlockingObjectEnum interface
    ///  * ------------------------------------------------------------------------- */
    /// 
    /// [
    ///     object,
    ///     local,
    ///     uuid(976A6278-134A-4a81-81A3-8F277943F4C3),
    ///     pointer_default(unique)
    /// ]
    /// 
    /// interface ICorDebugBlockingObjectEnum : ICorDebugEnum
    /// {
    ///     /*
    ///      * Gets the next "celt" blocking objects in the enumeration
    ///      */
    ///     HRESULT Next([in] ULONG celt,
    ///                  [out, size_is(celt), length_is(*pceltFetched)]
    ///                  CorDebugBlockingObject values[],
    ///                  [out] ULONG *pceltFetched);
    /// 
    /// };
    ///  </code></example>
    [Guid("976A6278-134A-4a81-81A3-8F277943F4C3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugBlockingObjectEnum : ICorDebugEnum
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
        /// Gets the next "celt" blocking objects in the enumeration
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