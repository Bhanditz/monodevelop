using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugStepper2 exposes JMC functionality.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugStepper2 exposes JMC functionality.
  ///  */
  /// [
  ///     object,
  ///     local,
  ///     uuid(C5B6E9C3-E7D1-4a8e-873B-7F047F0706F7),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugStepper2 : IUnknown
  /// {
  ///     HRESULT SetJMC([in] BOOL fIsJMCStepper);
  /// }
  /// 
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("C5B6E9C3-E7D1-4A8E-873B-7F047F0706F7")]
  [ComImport]
  public interface ICorDebugStepper2
  {
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 SetJMC ([In] int fIsJMCStepper);
  }
}