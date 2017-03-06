using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CorApi.Pinvoke;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugEnumEx
    {
        public static IEnumerable<ICorDebugAppDomain> AsEnumerable([NotNull] this ICorDebugAppDomainEnum comenumShared)
        {
            return AsEnumerable<ICorDebugAppDomainEnum, ICorDebugAppDomain>(comenumShared, comenum =>
            {
                void* pNext = null;
                using(Com.UsingReference(&pNext))
                {
                    uint celtFetched = 0;
                    comenum.Next(1, &pNext, &celtFetched).AssertSucceeded("Can't query for the next item.");
                    if((celtFetched == 0) || (pNext == null))
                        return null;
                    return Com.QueryInteface<ICorDebugAppDomain>(pNext);
                }
            });
        }

        public static IList<ICorDebugAppDomain> ToList([NotNull] this ICorDebugAppDomainEnum comenumShared)
        {
            return ToList<ICorDebugAppDomainEnum, ICorDebugAppDomain>(comenumShared, (corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }

        public static IList<ICorDebugChain> ToList([NotNull] this ICorDebugChainEnum comenumShared)
        {
            return ToList<ICorDebugChainEnum, ICorDebugChain>(comenumShared, (corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }

        public static IList<TItem> ToList<TCorDebugEnum, TItem>([NotNull] this TCorDebugEnum comenumShared, [NotNull] NextDelegate<TCorDebugEnum> FNext) where TCorDebugEnum : class, ICorDebugEnum where TItem : class
        {
            if(comenumShared == null)
                throw new ArgumentNullException(nameof(comenumShared));
            if(FNext == null)
                throw new ArgumentNullException(nameof(FNext));

            // ToList implementation options:
            //  • Only call Next(1) one-by-one (either by Count or until it returns none). I think that's no much use calling it this way.
            // >• Call Count, alloc buffer, call Next(count). Just one call for all. The current impl. Possible problems: race between getting Count and fetching items if not stopped. Possible problems: none because if it gets more items the impl would just return requested number, and if it gets fewer the impl would indicate this in celtFetched, ok.
            //  • Call Next(N) with reasonable N (say 0x10) until returns less than 0x10. Better than marshalling one-by-one, does not depend ou Count (tho it's deemed to be not important).

            // Assume we might be given a reused enumeration object in any state
            // Make us our own copy without no racing for Next
            // This is analogous to calling .NET's IEnumerable::GetEnumerator actually
            ICorDebugEnum comenum;
            comenumShared.Clone(out comenum).AssertSucceeded("Can't clone the enum.");
            comenum.Reset().AssertSucceeded("Can't reset the enum.");

            // Count & alloc
            uint celt = 0;
            comenum.GetCount(&celt).AssertSucceeded("Can't get the number of items in the enum.");
            if(celt == 0)
                return new TItem[] { };
            void** items = stackalloc void*[(int)celt];
            MemoryUtil.ZeroMemory(items, (uint)sizeof(void*) * celt);

            // Fetch
            uint celtActual = 0;
            FNext(Com.QueryInteface<TCorDebugEnum>(comenum), celt, items, &celtActual).AssertSucceeded("Failed to fetch the next items from the enumeration.");

            // Cast
            TItem[] retval;
            try
            {
                if(celtActual > celt)
                    throw new InvalidOperationException("Too many items rertieved.");

                retval = new TItem[celtActual];
                for(uint a = celtActual; a-- > 0;)
                {
                    if(items[a] != null)
                        retval[a] = Com.QueryInteface<TItem>(items[a]);
                    else
                        throw new NullReferenceException("A NULL item has been returned from the enumeration.");
                }
            }
            finally
            {
                for(uint a = Math.Min(celt, celtActual); a-- > 0;)
                    Com.UnknownRelease(items[a]);
            }
            return retval;
        }

        private static IEnumerable<TItem> AsEnumerable<TCorDebugEnum, TItem>([NotNull] this ICorDebugEnum comenumShared, [NotNull] TryGetNextDelegate<TCorDebugEnum, TItem> FNext) where TCorDebugEnum : class, ICorDebugEnum
        {
            if(comenumShared == null)
                throw new ArgumentNullException("comenumShared");
            if(FNext == null)
                throw new ArgumentNullException("FNext");

            ICorDebugEnum comenum;
            comenumShared.Clone(out comenum).AssertSucceeded("Can't clone the enum.");
            comenum.Reset().AssertSucceeded("Can't reset the enum.");
            var comenumT = Com.QueryInteface<TCorDebugEnum>(comenum);

            return AsEnumerableCore(comenumT, FNext);
        }

        private static IEnumerable<TItem> AsEnumerableCore<TCorDebugEnum, TItem>(TCorDebugEnum comenumT, [NotNull] TryGetNextDelegate<TCorDebugEnum, TItem> FNext) where TCorDebugEnum : ICorDebugEnum
        {
            for(;;)
            {
                TItem item = FNext(comenumT);
                if(item != null)
                    yield return item;
                else
                    yield break;
            }
        }

        public delegate int NextDelegate<TCorDebugEnum>(TCorDebugEnum corenum, [In] uint celt, void** values, [Out] uint* pceltFetched);

        [CanBeNull]
        public delegate TItem TryGetNextDelegate<TCorDebugEnum, TItem>([NotNull] TCorDebugEnum oEnum) where TCorDebugEnum : ICorDebugEnum;
    }
}