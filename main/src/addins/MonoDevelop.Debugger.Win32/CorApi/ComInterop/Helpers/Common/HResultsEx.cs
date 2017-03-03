using System;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
  public static class HResultsEx
  {
    #region Operations

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="HResultHelpers.FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    /// <remarks>Note: located here because we do not have ExtensionAttribute in Interop.WinAPI. In there, use <see cref="HResultHelpers.Assert(int,int[])"/>.</remarks>
    public static void Assert(this HResults hresult)
    {
      HResultHelpers.Assert(hresult);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="HResultHelpers.FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    /// <remarks>Note: located here because we do not have ExtensionAttribute in Interop.WinAPI. In there, use <see cref="HResultHelpers.Assert(int,int[])"/>.</remarks>
    public static void Assert(this HResults hresult, [NotNull] string comment)
    {
      HResultHelpers.Assert(hresult, comment);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="HResultHelpers.FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    /// <remarks>Note: located here because we do not have ExtensionAttribute in Interop.WinAPI. In there, use <see cref="HResultHelpers.Assert(int,int[])"/>.</remarks>
    public static void AssertSucceeded(this int hresult, [NotNull] string comment)
    {
      HResultHelpers.Assert(hresult, comment);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="HResultHelpers.FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    /// <remarks>Note: located here because we do not have ExtensionAttribute in Interop.WinAPI. In there, use <see cref="HResultHelpers.Assert(int,int[])"/>.</remarks>
    public static void AssertSucceeded(this UInt32 hresult, [NotNull] string comment)
    {
      HResultHelpers.Assert(unchecked((HResults)hresult), comment);
    }

    /// <summary>
    /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="HResultHelpers.FAILED(int)"/>).
    /// The comment is used in case of the failure only.
    /// </summary>
    /// <remarks>Note: located here because we do not have ExtensionAttribute in Interop.WinAPI. In there, use <see cref="HResultHelpers.Assert(int,int[])"/>.</remarks>
    public static void AssertSucceeded(this int hresult)
    {
      HResultHelpers.Assert(hresult);
    }

    /// <summary>
    /// The <c>FAILED</c> WinAPI Macro.
    /// </summary>
    public static bool Failed(this HResults hresult)
    {
      return HResultHelpers.FAILED(hresult);
    }

    /// <summary>
    /// The <c>SUCCEEDED</c> WinAPI Macro.
    /// </summary>
    public static bool Succeeded(this HResults hresult)
    {
      return HResultHelpers.SUCCEEDED(hresult);
    }

    #endregion
  }
}