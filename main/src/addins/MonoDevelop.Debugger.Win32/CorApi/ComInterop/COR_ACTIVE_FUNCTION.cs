using System;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// cordebug.idl:
  ///    typedef struct _COR_ACTIVE_FUNCTION
  ///    {
  ///        ICorDebugAppDomain *pAppDomain;   // Pointer to the owning AppDomain of the below IL Offset.
  ///        ICorDebugModule *pModule;         // Pointer to the owning Module of the below IL Offset.
  ///        ICorDebugFunction2 *pFunction;    // Pointer to the owning Function of the below IL Offset.
  ///        ULONG32 ilOffset;                 // IL Offset of the frame.
  ///        ULONG32 flags;                    // Bit mask of flags, currently unused.  Reserved.
  ///    } COR_ACTIVE_FUNCTION;
  /// </code></example>
  [StructLayout (LayoutKind.Sequential, Pack = 4)]
  public unsafe struct COR_ACTIVE_FUNCTION
  {
    /// <summary>
    /// Pointer to the owning AppDomain of the below IL Offset.
    /// </summary>
    public void* pAppDomain;

    /// <summary>
    /// Pointer to the owning Module of the below IL Offset.
    /// </summary>
    public void* pModule;

    /// <summary>
    /// Pointer to the owning Function of the below IL Offset.
    /// </summary>
    public void* pFunction;

    /// <summary>
    /// IL Offset of the frame.
    /// </summary>
    public uint ilOffset;

    /// <summary>
    /// Bit mask of flags, currently unused.  Reserved.
    /// </summary>
    [Obsolete ("Currently unused. Reserved.")]
    public uint Flags;
  }
}