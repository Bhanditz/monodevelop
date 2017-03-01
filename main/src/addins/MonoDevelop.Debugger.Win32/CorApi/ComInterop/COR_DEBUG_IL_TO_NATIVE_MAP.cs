using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  ///  typedef struct COR_DEBUG_IL_TO_NATIVE_MAP
  /// {
  ///     ULONG32 ilOffset;
  ///     ULONG32 nativeStartOffset;
  ///     ULONG32 nativeEndOffset;
  /// } COR_DEBUG_IL_TO_NATIVE_MAP;
  ///  </code></example>
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  [StructLayout (LayoutKind.Sequential)]
  public struct COR_DEBUG_IL_TO_NATIVE_MAP
  {
    public UInt32 ilOffset;

    public UInt32 nativeStartOffset;

    public UInt32 nativeEndOffset;
  }
}