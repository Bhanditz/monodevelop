using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    /// 
    /// </summary>
    /// <example><code>
    ////* ------------------------------------------------------------------------- *
    /// * AssemblyEnum interface
    /// * ------------------------------------------------------------------------- */
    ///
    ///[
    ///    object,
    ///    local,
    ///    uuid(4a2a1ec9-85ec-4bfb-9f15-a89fdfe0fe83),
    ///    pointer_default(unique)
    ///]
    ///
    ///interface ICorDebugAssemblyEnum : ICorDebugEnum
    ///{
    ///    /*
    ///     * Gets the next "celt" assemblies in the enumeration
    ///     */
    ///    HRESULT Next([in] ULONG celt,
    ///                 [out, size_is(celt), length_is(*pceltFetched)]
    ///                    ICorDebugAssembly *values[],
    ///                 [out] ULONG *pceltFetched);
    ///
    ///}; </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("4A2A1EC9-85EC-4BFB-9F15-A89FDFE0FE83")]
    [ComImport]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugAssemblyEnum : ICorDebugEnum
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
        /// Gets the next "celt" app domains in the enumeration
        /// </summary>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        Int32 Next([In] UInt32 celt, void** values, [Out] UInt32* pceltFetched);
    }
}