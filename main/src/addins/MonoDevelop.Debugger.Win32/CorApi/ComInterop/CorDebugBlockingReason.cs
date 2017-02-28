namespace CorApi.ComInterop
{
  /// <summary>
  /// 
  /// </summary>
  /// <example><code>
  /// typedef enum CorDebugBlockingReason {
  ///    BLOCKING_NONE                     = 0x0,
  ///    BLOCKING_MONITOR_CRITICAL_SECTION = 0x1,
  ///    BLOCKING_MONITOR_EVENT            = 0x2
  ///} CorDebugBlockingReason;
  /// </code></example>
  public enum CorDebugBlockingReason {
    BLOCKING_NONE                     = 0x0,
    BLOCKING_MONITOR_CRITICAL_SECTION = 0x1,
    BLOCKING_MONITOR_EVENT            = 0x2
  }
}