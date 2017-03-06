using System;

namespace CorApi.ComInterop
{
    /// <summary>
    /// 
    /// </summary>
    /// <example><code>
    /// typedef enum CorDebugJITCompilerFlags
    ///{
    ///   CORDEBUG_JIT_DEFAULT = 0x1,                  // Track info, enable optimizations
    ///   CORDEBUG_JIT_DISABLE_OPTIMIZATION = 0x3,     // Includes track info, disable opts,
    ///   CORDEBUG_JIT_ENABLE_ENC = 0x7                // Includes track &amp; disable opt &amp; Edit and Continue.
    ///} CorDebugJITCompilerFlags;
    ///
    /// </code></example>
    [Flags]
    public enum CorDebugJITCompilerFlags
    {
        /// <summary>
        /// Track info, enable optimizations
        /// </summary>
        CORDEBUG_JIT_DEFAULT = 0x1,
        /// <summary>
        /// Includes track info, disable opts,
        /// </summary>
        CORDEBUG_JIT_DISABLE_OPTIMIZATION = 0x3,
        /// <summary>
        /// Includes track &amp; disable opt &amp; Edit and Continue.
        /// </summary>
        CORDEBUG_JIT_ENABLE_ENC = 0x7
    }
}