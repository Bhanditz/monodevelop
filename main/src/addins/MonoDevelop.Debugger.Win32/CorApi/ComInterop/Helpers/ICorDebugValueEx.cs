using System;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static class ICorDebugValueEx
    {
        public static ICorDebugType GetExactType([NotNull] this ICorDebugValue corvalue)
        {
            if(corvalue == null)
                throw new ArgumentNullException(nameof(corvalue));
            ICorDebugType exacttype;
            Com.QueryInteface<ICorDebugValue2>(corvalue).GetExactType(out exacttype).AssertSucceeded("Could not get the Exact Type of a Value.");
            return exacttype;
        }
    }
}