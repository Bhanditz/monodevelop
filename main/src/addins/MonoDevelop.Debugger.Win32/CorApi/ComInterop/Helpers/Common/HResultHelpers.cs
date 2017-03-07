using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    /// Helper methods related to the <c>HRESULT</c> type.
    /// </summary>
    public static class HResultHelpers
    {
        /// <summary>
        /// Checks the given <c>HRESULT</c>, if it's a failure one (<see cref="FAILED(int)" />), and is not contained in <paramref name="expectedHRFailure" />, then an exception-from-hresult is thrown.
        /// </summary>
        public static void Assert(int hresult, params int[] expectedHRFailure)
        {
            if(FAILED(hresult) && ((expectedHRFailure == null) || (Array.IndexOf(expectedHRFailure, hresult) < 0)))
                ThrowExceptionIfFailed(hresult);
        }

        /// <summary>
        /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)" />).
        /// </summary>
        public static void Assert(int hresult)
        {
            ThrowExceptionIfFailed(hresult); // Won't throw on success
        }

        /// <summary>
        /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)" />).
        /// </summary>
        public static void Assert(HResults hresult)
        {
            ThrowExceptionIfFailed((int)hresult); // Won't throw on success
        }

        /// <summary>
        /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)" />).
        /// The comment is used in case of the failure only.
        /// </summary>
        public static void Assert(int hresult, string comment)
        {
            ThrowExceptionIfFailed(hresult, comment);
        }

        /// <summary>
        /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)" />).
        /// The comment is used in case of the failure only.
        /// </summary>
        public static void Assert(HResults hresult, string comment)
        {
            ThrowExceptionIfFailed(((int)hresult), comment);
        }

        /// <summary>
        /// Checks the given <c>HRESULT</c>, and throws an exception if it's a failure one (<see cref="FAILED(int)" />).
        /// The FComment function is called in case of the failure only, should use <c>AddData</c> to contribute to the exception report.
        /// </summary>
        public static void Assert(HResults hresult, Action<Exception> FComment)
        {
            Exception exCom = GetExceptionIfFailed((int)hresult); // Yields Null for success codes
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
        /// Wrapper for <see cref="Marshal.GetExceptionForHR(int)" />.
        /// </summary>
        [CanBeNull]
        public static Exception GetExceptionIfFailed(int hresult)
        {
            return Marshal.GetExceptionForHR(hresult); // TODO: mono makes an empty message for HResults it does not know, so (a) make own hresults lookup, our list is larger; (b) call for ErrorInfo on win
        }

        /// <summary>
        /// Wrapper for creating an exception over <see cref="Marshal.GetExceptionForHR(int)" /> with a custom details message.
        /// </summary>
        [CanBeNull]
        public static Exception GetExceptionIfFailed(int hresult, string comment)
        {
            Exception exCom = GetExceptionIfFailed(hresult); // Yields Null for success codes
            if(exCom == null)
                return null; // Was OK
            return new InvalidOperationException(comment + " " + exCom.Message, exCom);
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

        /// <summary>
        /// Core wrapper for <see cref="Marshal.ThrowExceptionForHR(int)" />.
        /// </summary>
        /// <param name="hresult"></param>
        public static void ThrowExceptionIfFailed(int hresult)
        {
            Marshal.ThrowExceptionForHR(hresult); // TODO: mono makes an empty message for HResults it does not know, so (a) make own hresults lookup, our list is larger; (b) call for ErrorInfo on win
        }

        /// <summary>
        /// Creates a wrapping exception with a custom message over the COM exception.
        /// </summary>
        public static void ThrowExceptionIfFailed(int hresult, string comment)
        {
            Exception ex = GetExceptionIfFailed(hresult, comment);
            if(ex != null)
                throw ex;
        }
    }
}