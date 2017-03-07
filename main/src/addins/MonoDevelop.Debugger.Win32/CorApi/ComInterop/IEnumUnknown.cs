using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    ///   Enumerates objects with the IUnknown interface. It can be used to enumerate through the objects in a component containing multiple objects.
    /// </summary>
    /// <example><code>
    ///   [
    ///     object,
    ///     uuid(00000100-0000-0000-C000-000000000046),
    ///     pointer_default(unique)
    /// ]
    /// 
    /// interface IEnumUnknown : IUnknown
    /// {
    /// 
    ///     typedef [unique] IEnumUnknown *LPENUMUNKNOWN;
    /// 
    ///     [local]
    ///     HRESULT Next(
    ///         [in, annotation("_In_")] ULONG celt,
    ///         [out, annotation("_Out_writes_to_(celt,*pceltFetched)")] IUnknown **rgelt,
    ///         [out, annotation("_Out_opt_")] ULONG *pceltFetched);
    /// 
    ///     [call_as(Next)]
    ///     HRESULT RemoteNext(
    ///         [in] ULONG celt,
    ///         [out, size_is(celt), length_is(*pceltFetched)] IUnknown **rgelt,
    ///         [out] ULONG *pceltFetched);
    /// 
    ///     HRESULT Skip(
    ///         [in] ULONG celt);
    /// 
    ///     HRESULT Reset();
    /// 
    ///     HRESULT Clone(
    ///         [out] IEnumUnknown **ppenum);
    /// }  
    ///   </code></example>
    [ComImport]
    [Guid("00000100-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public unsafe interface IEnumUnknown
    {
        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int Next([In] uint celt, [Out] void**rgelt, [Out] uint*pceltFetched);

        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int Skip([In] uint celt);

        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int Reset();

        [MustUseReturnValue("HResult")]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        int Clone([Out] void**ppenum);
    }
}