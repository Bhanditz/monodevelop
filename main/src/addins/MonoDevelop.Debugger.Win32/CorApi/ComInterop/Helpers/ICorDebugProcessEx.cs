using System;
using System.Collections.Generic;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugProcessEx
    {
        public static IList<ICorDebugAppDomain> GetAppDomains(this ICorDebugProcess @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            ICorDebugAppDomainEnum @enum;
            @this.EnumerateAppDomains(out @enum).AssertSucceeded("Cannot enumerate process appdomains.");

            return @enum.ToList<ICorDebugAppDomainEnum, ICorDebugAppDomain>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }

        public static IList<ICorDebugThread> GetThreads(this ICorDebugProcess @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            ICorDebugThreadEnum @enum;
            @this.EnumerateThreads(out @enum).AssertSucceeded("Cannot enumerate process trhreads.");

            return @enum.ToList<ICorDebugThreadEnum, ICorDebugThread>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }
    }
}