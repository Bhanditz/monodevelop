using System;

namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// typedef struct CorDebugBlockingObject {
  ///    ICorDebugValue         *pBlockingObject;
  ///    DWORD                   dwTimeout;
  ///    CorDebugBlockingReason  blockingReason;
  ///} CorDebugBlockingObject;
  /// </code></example>
  public unsafe struct CorDebugBlockingObject {
    public /*ICorDebugValue*/ void         *pBlockingObject;
    public UInt32                   dwTimeout;
    public CorDebugBlockingReason  blockingReason;
  }
}