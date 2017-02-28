using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugThread4 is a logical extension to ICorDebugThread.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugThread4 is a logical extension to ICorDebugThread.
  ///  */
  /// [
  ///     object,
  ///     local,
  ///     uuid(1A1F204B-1C66-4637-823F-3EE6C744A69C),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugThread4 : IUnknown
  /// {
  ///     /*
  ///      * Returns S_OK if ICorDebugThread::GetCurrentException() is non-NULL and the exception
  ///      * it refers to has completed the first pass of exception handling without locating 
  ///      * a catch clause.
  ///      * Returns S_FALSE if there is no exception, it hasn't completed first pass handling,
  ///      * or a catch handler was located
  ///      * Returns an appropriate error HRESULT when the answer can not be determined
  ///      */
  ///     HRESULT HasUnhandledException();
  /// 
  ///     HRESULT GetBlockingObjects([out] ICorDebugBlockingObjectEnum **ppBlockingObjectEnum);
  ///     /* 
  ///      * Gets the current CustomNotification object on the current thread. This could be NULL if no
  ///      * current notification object exists. If we aren't currently inside a CustomNotification callback,
  ///      * this will always return NULL.
  ///      * A debugger can examine this object to determine how to handle the notification. 
  ///      * See ICorDebugManagedCallback3::CustomNotification for more information about 
  ///      * custom notifications. 
  ///      */
  ///     HRESULT GetCurrentCustomDebuggerNotification([out] ICorDebugValue ** ppNotificationObject);
  /// };
  /// 
  ///  </code></example>
  [Guid ("1A1F204B-1C66-4637-823F-3EE6C744A69C")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface ICorDebugThread4
  {
    [PreserveSig]
    int GetBlockingObjects (out ICorDebugBlockingObjectEnum ppBlockingObjectEnum);

    /// <summary>
    ///  Gets the current CustomNotification object on the current thread. This could be NULL if no
    ///  current notification object exists. If we aren't currently inside a CustomNotification callback,
    ///  this will always return NULL.
    ///  A debugger can examine this object to determine how to handle the notification.
    ///  See ICorDebugManagedCallback3::CustomNotification for more information about
    ///  custom notifications.
    /// </summary>
    /// <param name="ppNotificationObject"></param>
    /// <returns></returns>
    [PreserveSig]
    int GetCurrentCustomDebuggerNotification (out ICorDebugValue ppNotificationObject);

    /// <summary>
    /// Returns S_OK if ICorDebugThread::GetCurrentException() is non-NULL and the exception
    /// it refers to has completed the first pass of exception handling without locating
    /// a catch clause.
    /// Returns S_FALSE if there is no exception, it hasn't completed first pass handling,
    /// or a catch handler was located
    /// Returns an appropriate error HRESULT when the answer can not be determined
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int HasUnhandledException ();
  }
}