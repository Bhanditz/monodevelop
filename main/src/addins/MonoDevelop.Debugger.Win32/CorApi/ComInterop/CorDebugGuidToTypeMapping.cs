using System;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// </summary>
  /// <example><code>
  /// typedef struct CorDebugGuidToTypeMapping
  /// {
  /// 	GUID iid;
  /// 	ICorDebugType * pType;
  /// } CorDebugGuidToTypeMapping; </code></example>
  [StructLayout (LayoutKind.Sequential)]
  public struct CorDebugGuidToTypeMapping
  {
    public Guid iid;

    public ICorDebugType pType;
  }
}