using System;
using System.Collections.Generic;

using CorApi.Pinvoke;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugThreadEx
    {
        [NotNull]
        public static CorActiveFunction[] GetActiveFunctions([NotNull] this ICorDebugThread thread)
        {
            if(thread == null)
                throw new ArgumentNullException(nameof(thread));
            var thread2 = Com.QueryInteface<ICorDebugThread2>(thread);

            uint cFunctions;
            thread2.GetActiveFunctions(0, &cFunctions, null).AssertSucceeded("Could not query for the number of active functions.");
            if(cFunctions == 0)
                return new CorActiveFunction[] { };

            COR_ACTIVE_FUNCTION* afunctions = stackalloc COR_ACTIVE_FUNCTION[(int)cFunctions];
            MemoryUtil.ZeroMemory(afunctions, ((uint)sizeof(COR_ACTIVE_FUNCTION*)) * cFunctions);
            thread2.GetActiveFunctions(cFunctions, &cFunctions, afunctions).AssertSucceeded("Could not query for the list of active functions.");

            try
            {
                var caf = new CorActiveFunction[cFunctions];
                for(uint a = cFunctions; a-- > 0;)
                    caf[a] = new CorActiveFunction((int)afunctions[a].ilOffset, Com.QueryInteface<ICorDebugFunction>(afunctions[a].pFunction), (afunctions[a].pModule != null ? Com.QueryInteface<ICorDebugModule>(afunctions[a].pModule) : null));
                return caf;
            }
            finally
            {
                for(uint a = cFunctions; a-- > 0;)
                {
                    Com.UnknownRelease(afunctions[a].pAppDomain);
                    Com.UnknownRelease(afunctions[a].pFunction);
                    Com.UnknownRelease(afunctions[a].pModule);
                }
            }
        }

        public static IList<ICorDebugChain> GetChains(this ICorDebugThread @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            ICorDebugChainEnum @enum;
            @this.EnumerateChains(out @enum).AssertSucceeded("Cannot enumerate chain frames.");

            return @enum.ToList<ICorDebugChainEnum, ICorDebugChain>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }

        public struct CorActiveFunction
        {
            internal CorActiveFunction(int ilOffset, ICorDebugFunction managedFunction, ICorDebugModule managedModule)
            {
                ILoffset = ilOffset;
                Function = managedFunction;
                Module = managedModule;
            }

            public readonly ICorDebugFunction Function;

            public readonly int ILoffset;

            public readonly ICorDebugModule Module;
        }
    }
}