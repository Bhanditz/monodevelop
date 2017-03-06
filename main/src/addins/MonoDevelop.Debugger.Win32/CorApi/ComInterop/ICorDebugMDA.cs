using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  Represent data for an Managed Debugging Assistant (MDA) notification. See the MDA documentation for MDA-specific information like:
  ///  - enabling / disabling MDAs
  ///  - MDA naming conventions
  ///  - What the contents of an MDA look like, schemas, etc.
  /// </summary>
  /// <example><code>
  ///  // Represent data for an Managed Debugging Assistant (MDA) notification. See the MDA documentation for MDA-specific information like:
  ///  // - enabling / disabling MDAs
  ///  // - MDA naming conventions
  ///  // - What the contents of an MDA look like, schemas, etc.
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC726F2F-1DB7-459b-B0EC-05F01D841B42),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugMDA : IUnknown
  /// {
  ///     // Get the string for the type of the MDA. Never empty.
  ///     // This is a convenient performant alternative to getting the XML stream and extracting
  ///     // the type from that based off the schema.
  ///     HRESULT GetName(
  ///         [in] ULONG32 cchName,
  ///         [out] ULONG32 * pcchName,
  ///         [out, size_is(cchName), length_is(*pcchName)] WCHAR szName[]);
  /// 
  ///     // Get a string description of the MDA. This may be empty (0-length).
  ///     HRESULT GetDescription(
  ///         [in] ULONG32 cchName,
  ///         [out] ULONG32 * pcchName,
  ///         [out, size_is(cchName), length_is(*pcchName)] WCHAR szName[]);
  /// 
  ///     // Get the full associated XML for the MDA. This may be empty.
  ///     // This could be a potentially expensive operation if the xml stream is large.
  ///     // See the MDA documentation for the schema for this XML stream.
  ///     HRESULT GetXML(
  ///         [in] ULONG32 cchName,
  ///         [out] ULONG32 * pcchName,
  ///         [out, size_is(cchName), length_is(*pcchName)] WCHAR szName[]);
  /// 
  ///     // Get the flags associated w/ the MDA. New flags may be added in future versions.
  ///     typedef enum CorDebugMDAFlags
  ///     {
  /// 	// If this flag is high, then the thread may have slipped since the MDA was fired. 
  /// 	MDA_FLAG_SLIP = 0x2
  ///     } CorDebugMDAFlags;
  ///     HRESULT GetFlags([in] CorDebugMDAFlags * pFlags);
  /// 
  ///     // Thread that the MDA is fired on. We use the os tid instead of an ICDThread in case an MDA is fired on a
  ///     // native thread (or a managed thread that hasn't yet entered managed code and so we don't have a ICDThread
  ///     // object for it yet)
  ///     HRESULT GetOSThreadId([out] DWORD * pOsTid);
  /// };
  /// 
  ///  </code></example>
  [Guid ("CC726F2F-1DB7-459B-B0EC-05F01D841B42")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugMDA
  {
    /// <summary>
    /// Get the string for the type of the MDA. Never empty.
    /// This is a convenient performant alternative to getting the XML stream and extracting
    /// the type from that based off the schema.
    /// </summary>
    /// <param name="cchName"></param>
    /// <param name="pcchName"></param>
    /// <param name="szName"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetName ([In] UInt32 cchName, UInt32* pcchName, UInt16* szName);

    /// <summary>
    /// Get a string description of the MDA. This may be empty (0-length).
    /// </summary>
    /// <param name="cchName"></param>
    /// <param name="pcchName"></param>
    /// <param name="szName"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetDescription ([In] UInt32 cchName, UInt32* pcchName, UInt16* szName);

    /// <summary>
    /// Get the full associated XML for the MDA. This may be empty.
    /// This could be a potentially expensive operation if the xml stream is large.
    /// See the MDA documentation for the schema for this XML stream.
    /// </summary>
    /// <param name="cchName"></param>
    /// <param name="pcchName"></param>
    /// <param name="szName"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetXML ([In] UInt32 cchName, UInt32* pcchName, UInt16* szName);

    /// <summary>
    /// Get the flags associated w/ the MDA. New flags may be added in future versions.
    /// </summary>
    /// <param name="pFlags"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFlags ([In] CorDebugMDAFlags* pFlags);

    /// <summary>
    /// Thread that the MDA is fired on. We use the os tid instead of an ICDThread in case an MDA is fired on a
    /// native thread (or a managed thread that hasn't yet entered managed code and so we don't have a ICDThread
    /// object for it yet)
    /// </summary>
    /// <param name="pOsTid"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetOSThreadId (UInt32* pOsTid);
  }
}