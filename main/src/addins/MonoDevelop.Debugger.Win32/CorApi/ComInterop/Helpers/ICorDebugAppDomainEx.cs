using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugAppDomainEx
    {
        public static IList<ICorDebugAssembly> GetAssemblies(this ICorDebugAppDomain @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            ICorDebugAssemblyEnum @enum;
            @this.EnumerateAssemblies(out @enum).AssertSucceeded("Cannot enumerate appdomain trhreads.");

            return @enum.ToList<ICorDebugAssemblyEnum, ICorDebugAssembly>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }

        [NotNull]
        public static string GetName([CanBeNull] this ICorDebugAppDomain @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            uint cch = 0;
            @this.GetName(0, &cch, null).AssertSucceeded("Can't measure the appdomain name length.");
            if(cch == 0)
                return"";

            if(cch > 0x1000)
                throw new InvalidOperationException($"Not expecting the appdomain name to be {cch:N0} chars long.");
            ushort* pch = stackalloc ushort[(int)cch];
            uint cchActual = 0;
            @this.GetName(cch, &cchActual, pch).AssertSucceeded("Can't get the appdomain name body.");
            if(cchActual != cch)
                throw new InvalidOperationException($"Error getting appdomain name length, told to expect {cch:N0} chars, got out {cchActual:N0}.");
            if(pch[cchActual - 1] != 0)
                throw new InvalidOperationException($"Error getting appdomain name of length {cch:N0}, the string is not zero-terminated.");

            return new string((char*)pch);
        }

        public static IList<ICorDebugThread> GetThreads(this ICorDebugAppDomain @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            ICorDebugThreadEnum @enum;
            @this.EnumerateThreads(out @enum).AssertSucceeded("Cannot enumerate appdomain trhreads.");

            return @enum.ToList<ICorDebugThreadEnum, ICorDebugThread>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }
    }
}