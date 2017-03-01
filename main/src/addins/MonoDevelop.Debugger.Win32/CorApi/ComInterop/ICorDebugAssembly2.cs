using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  [
  ///     object,
  ///     local,
  ///     uuid(426d1f9e-6dd4-44c8-aec7-26cdbaf4e398),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugAssembly2 : IUnknown
  /// {
  ///     /*
  ///      * IsFullyTrusted sets a flag indicating whether the assembly has
  ///      * been granted full trust by the runtime security system.
  ///      * This may return CORDBG_E_NOTREADY if the security policy for
  ///      * the assembly has not yet been resolved (eg. no code in the
  ///      * assembly has been run yet).
  ///      */
  ///     HRESULT IsFullyTrusted([out] BOOL *pbFullyTrusted);
  /// };
  /// 
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("426D1F9E-6DD4-44C8-AEC7-26CDBAF4E398")]
  [ComImport]
  public unsafe interface ICorDebugAssembly2
  {
    /// <summary>
    /// IsFullyTrusted sets a flag indicating whether the assembly has
    /// been granted full trust by the runtime security system.
    /// This may return CORDBG_E_NOTREADY if the security policy for
    /// the assembly has not yet been resolved (eg. no code in the
    /// assembly has been run yet).
    /// </summary>
    /// <param name="pbFullyTrusted"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void IsFullyTrusted (int* pbFullyTrusted);
  }
}