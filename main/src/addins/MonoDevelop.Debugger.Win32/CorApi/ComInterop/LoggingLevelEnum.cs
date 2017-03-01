namespace CorApi.ComInterop
{
  /// <summary>
  /// Enum defining log message LoggingLevels
  /// </summary>
  /// <example><code>
  ///     /*
  ///     * Enum defining log message LoggingLevels
  ///     */
  ///    typedef enum LoggingLevelEnum
  ///    {
  ///        LTraceLevel0 = 0,
  ///        LTraceLevel1,
  ///        LTraceLevel2,
  ///        LTraceLevel3,
  ///        LTraceLevel4,
  ///        LStatusLevel0 = 20,
  ///        LStatusLevel1,
  ///        LStatusLevel2,
  ///        LStatusLevel3,
  ///        LStatusLevel4,
  ///        LWarningLevel = 40,
  ///        LErrorLevel = 50,
  ///        LPanicLevel = 100
  ///    } LoggingLevelEnum;
  /// </code></example>
  public enum LoggingLevelEnum
  {
    LTraceLevel0 = 0,
    LTraceLevel1,
    LTraceLevel2,
    LTraceLevel3,
    LTraceLevel4,
    LStatusLevel0 = 20,
    LStatusLevel1,
    LStatusLevel2,
    LStatusLevel3,
    LStatusLevel4,
    LWarningLevel = 40,
    LErrorLevel = 50,
    LPanicLevel = 100
  }
}