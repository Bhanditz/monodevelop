using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
  /// <summary>
  /// A thread's DebugState determines whether the debugger lets a thread
  /// run or not.  Possible states are:
  /// 
  /// THREAD_RUN - thread runs freely, unless a debug event occurs
  /// THREAD_SUSPEND - thread cannot run.
  /// 
  /// NOTE: We allow for message pumping via a callback provided to the Hosting
  ///      API, thus we don't need an 'interrupted' state here.
  /// </summary>
  /// <example><code>
  /// from: &lt;cordebug.idl&gt;
  ///        /*
  ///        * A thread's DebugState determines whether the debugger lets a thread
  ///        * run or not.  Possible states are:
  ///        *
  ///        * THREAD_RUN - thread runs freely, unless a debug event occurs
  ///        * THREAD_SUSPEND - thread cannot run.
  ///        *
  ///        * NOTE: We allow for message pumping via a callback provided to the Hosting
  ///        *      API, thus we don't need an 'interrupted' state here.
  ///        * /
  ///
  ///        typedef enum CorDebugThreadState
  ///        {
  ///            THREAD_RUN,
  ///            THREAD_SUSPEND
  ///        } CorDebugThreadState;
  /// </code></example>
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugThreadState
    {
        /// <summary>
        /// thread runs freely, unless a debug event occurs
        /// </summary>
        THREAD_RUN,
        /// <summary>
        /// thread cannot run
        /// </summary>
        THREAD_SUSPEND,
    }
}