using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// [
  ///    object,
  ///    local,
  ///    uuid(5E0B54E7-D88A-4626-9420-A691E0A78B49),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugValue2 : IUnknown
  ///{
  ///    /*
  ///     * GetExactType returns the runtime type of the object in the value.
  ///     */
  ///
  ///    HRESULT GetExactType([out] ICorDebugType **ppType);
  ///
  ///};
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("5E0B54E7-D88A-4626-9420-A691E0A78B49")]
    [ComImport]
    public unsafe interface ICorDebugValue2
    {
      /// <summary>
      /// GetExactType returns the runtime type of the object in the value.
      /// </summary>
      /// <param name="ppType"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetExactType ([MarshalAs (UnmanagedType.Interface)] out ICorDebugType ppType);
    }
}