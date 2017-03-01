using System;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// Helper methods related to the <c>HRESULT</c> type.
  /// </summary>
  public static class HResultHelpers
  {
    #region Operations

    /// <summary>
    /// Checks the given <c>HRESULT</c>, if it's a failure one (<see cref="FAILED(int)"/>), and is not contained in <paramref name="expectedHRFailure"/>, then an exception-from-hresult is thrown.
    /// </summary>
    public static void Assert(int hresult, params int[] expectedHRFailure)
    {
      if(FAILED(hresult) && ((expectedHRFailure == null) || (Array.IndexOf(expectedHRFailure, hresult) < 0)))
        Marshal.ThrowExceptionForHR(hresult);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)"/>).
    /// </summary>
    public static void Assert(int hresult)
    {
      Marshal.ThrowExceptionForHR(hresult); // Won't throw on success
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)"/>).
    /// </summary>
    public static void Assert(HResults hresult)
    {
      Marshal.ThrowExceptionForHR((int)hresult); // Won't throw on success
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    public static void Assert(int hresult, string comment)
    {
      Exception exCom = Marshal.GetExceptionForHR(hresult); // Yields Null for success codes
      if(exCom == null)
        return; // Was OK
      throw new InvalidOperationException(comment + " " + exCom.Message, exCom);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    public static void Assert(HResults hresult, string comment)
    {
      Exception exCom = Marshal.GetExceptionForHR((int)hresult); // Yields Null for success codes
      if(exCom == null)
        return; // Was OK
      throw new InvalidOperationException(comment + " " + exCom.Message, exCom);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)"/>).
    /// The FComment function is called in case of the failure only, should use <c>AddData</c> to contribute to the exception report.
    /// </summary>
    public static void Assert(HResults hresult, Action<Exception> FComment)
    {
      Exception exCom = Marshal.GetExceptionForHR((int)hresult); // Yields Null for success codes
      if(exCom == null)
        return; // Was OK
      try
      {
        FComment(exCom);
      }
      catch
      {
        // NOP
      }
      throw exCom;
    }

    /// <summary>
    /// The <c>FAILED</c> WinAPI Macro.
    /// </summary>
    public static bool FAILED(HResults hresult)
    {
      return FAILED((int)hresult);
    }

    /// <summary>
    /// The <c>FAILED</c> WinAPI Macro.
    /// </summary>
    public static bool FAILED(int hresult)
    {
      return hresult < 0;
    }

    /// <summary>
    /// The <c>SUCCEEDED</c> WinAPI Macro.
    /// </summary>
    public static bool SUCCEEDED(HResults hresult)
    {
      return SUCCEEDED((int)hresult);
    }

    /// <summary>
    /// The <c>SUCCEEDED</c> WinAPI Macro.
    /// </summary>
    public static bool SUCCEEDED(int hresult)
    {
      return hresult >= 0;
    }

    #endregion
  }
}