using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  Assembly interface
  ///  An ICorDebugAssembly instance corresponds to a a managed assembly loaded
  ///  into a specific AppDomain in the CLR.  For assemblies shared between multiple
  ///  AppDomains (eg. mscorlib), there will be a separate ICorDebugAssembly instance
  ///  per AppDomain in which it is used.
  /// </summary>
  /// <example><code>
  ///  * ------------------------------------------------------------------------- *
  ///  * Assembly interface
  ///  * An ICorDebugAssembly instance corresponds to a a managed assembly loaded
  ///  * into a specific AppDomain in the CLR.  For assemblies shared between multiple
  ///  * AppDomains (eg. mscorlib), there will be a separate ICorDebugAssembly instance
  ///  * per AppDomain in which it is used.
  ///  * ------------------------------------------------------------------------- */
  /// [
  ///     object,
  ///     local,
  ///     uuid(df59507c-d47a-459e-bce2-6427eac8fd06),
  ///     pointer_default(unique)
  /// ]
  /// 
  /// interface ICorDebugAssembly : IUnknown
  /// {
  ///     /*
  ///      * GetProcess returns the process containing the assembly
  ///      */
  /// 
  ///     HRESULT GetProcess([out] ICorDebugProcess **ppProcess);
  /// 
  ///     /*
  ///      * GetAppDomain returns the app domain containing the assembly.
  ///      */
  /// 
  ///     HRESULT GetAppDomain([out] ICorDebugAppDomain **ppAppDomain);
  /// 
  ///     /*
  ///      * EnumerateModules enumerates all modules in the assembly
  ///      *
  ///      */
  /// 
  ///     HRESULT EnumerateModules([out] ICorDebugModuleEnum **ppModules);
  /// 
  ///     /*
  ///      * NOT YET IMPLEMENTED
  ///      */
  /// 
  ///     HRESULT GetCodeBase([in] ULONG32 cchName,
  ///                         [out] ULONG32 *pcchName,
  ///                         [out, size_is(cchName),
  ///                         length_is(*pcchName)] WCHAR szName[]);
  /// 
  ///     /*
  ///      * GetName returns the full path and filename of the assembly.
  ///      * If the assembly has no filename (i.e. it is in-memory only),
  ///      * S_FALSE is returned, and a fabricated string is stored into szName. 
  ///      */
  /// 
  ///     HRESULT GetName([in] ULONG32 cchName,
  ///                     [out] ULONG32 *pcchName,
  ///                     [out, size_is(cchName),
  ///                     length_is(*pcchName)] WCHAR szName[]);
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("DF59507C-D47A-459E-BCE2-6427EAC8FD06")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugAssembly
  {
    /// <summary>
    /// GetProcess returns the process containing the assembly.
    /// </summary>
    /// <param name="ppProcess"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetProcess ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

    /// <summary>
    /// GetAppDomain returns the app domain containing the assembly.
    /// </summary>
    /// <param name="ppAppDomain"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetAppDomain ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAppDomain ppAppDomain);

    /// <summary>
    /// EnumerateModules enumerates all modules in the assembly.
    /// </summary>
    /// <param name="ppModules"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EnumerateModules ([MarshalAs (UnmanagedType.Interface)] out ICorDebugModuleEnum ppModules);

    /// <summary>
    /// NOT YET IMPLEMENTED
    /// </summary>
    /// <param name="cchName"></param>
    /// <param name="pcchName"></param>
    /// <param name="szName"></param>
    [Obsolete ("NOT YET IMPLEMENTED")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCodeBase ([In] UInt32 cchName, UInt32* pcchName, UInt16* szName);

    /// <summary>
    /// GetName returns the full path and filename of the assembly.
    /// If the assembly has no filename (i.e. it is in-memory only),
    /// S_FALSE is returned, and a fabricated string is stored into szName.
    /// </summary>
    /// <param name="cchName"></param>
    /// <param name="pcchName"></param>
    /// <param name="szName"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetName ([In] UInt32 cchName, UInt32* pcchName, UInt16* szName);
  }
}