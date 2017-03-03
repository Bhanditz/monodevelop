using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Samples.Debugging.CorMetadata.NativeApi.Infra;

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

    delegate Int32 NextDelegate<TCorDebugEnum>(TCorDebugEnum corenum,[In] UInt32 celt, void** values, [Out] UInt32* pceltFetched);

    private static IList<TItem> ToList<TCorDebugEnum, TItem>([NotNull] this ICorDebugEnum comenumShared, [NotNull] NextDelegate<TCorDebugEnum> FNext) where TCorDebugEnum : class, ICorDebugEnum where TItem : class
    {
      if(comenumShared == null)
        throw new ArgumentNullException(nameof(comenumShared));
      if(FNext == null)
        throw new ArgumentNullException(nameof(FNext));

      Tracepoints.Say say = Tracepoints.None("ToList");

      ICorDebugEnum comenum;
      say();
      comenumShared.Clone(out comenum).AssertSucceeded("Can't clone the enum.");
      say();
      comenum.Reset().AssertSucceeded("Can't reset the enum.");
      say();

      uint celt = 0;
      comenum.GetCount(&celt).AssertSucceeded("Can't get the number of items in the enum.");
      void** items = stackalloc void*[(int)celt];
      uint celtActual = 0;
      FNext(Com.QueryInteface<TCorDebugEnum>(comenum), celt, items, &celtActual).AssertSucceeded("Failed to fetch the next items from the enumeration.");

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
            throw new NullReferenceException("A NULL items has been returned from the enumeration.");
        }
      }
      finally
      {
        for(uint a = Math.Min(celt, celtActual); a-- > 0;)
          Com.UnknownRelease(items[a]);
      }
      return retval;
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

    [CanBeNull]
    public delegate TItem TryGetNextDelegate<TCorDebugEnum, TItem>([NotNull] TCorDebugEnum oEnum) where TCorDebugEnum : ICorDebugEnum;
  }

  public static unsafe class Com
  {
    public static TInterface QueryInteface<TInterface>([NotNull] object comobj) where TInterface : class
    {
      if(comobj == null)
        throw new ArgumentNullException("The COM object instance is NULL.", default(Exception));
      if(!Marshal.IsComObject(comobj))
        throw new ArgumentOutOfRangeException("comobj", comobj, "This object is not a COM object instance.");
      var comobjQI = comobj as TInterface;
      if(comobjQI == null)
        throw new ArgumentOutOfRangeException("comobj", comobj, string.Format("This COM object does not implement the {0} ({1}) interface.", typeof(TInterface).Name, typeof(TInterface).GUID.ToString("B").ToUpperInvariant()));
      return comobjQI;
    }

    public static TInterface QueryInteface<TInterface>([NotNull] void* punk) where TInterface : class
    {
      if(punk == null)
        throw new ArgumentNullException("The COM object instance is NULL.", default(Exception));
      object oUnk;
      try
      {
        oUnk = Marshal.GetObjectForIUnknown((IntPtr)punk);
      }
      catch(Exception ex)
      {
        throw new ArgumentOutOfRangeException("The IUnknown pointer is not a COM object. " + ex.Message, ex);
      }
      return QueryInteface<TInterface>(oUnk);
    }

    public static void UnknownRelease([CanBeNull] void* punk)
    {
      if(punk != null)
        Marshal.Release((IntPtr)punk);
    }

    public static ReleaseToken UsingReference([NotNull] void** punk)
    {
      return new ReleaseToken(punk);
    }

    public struct ReleaseToken : IDisposable
    {
      private void** _punk;

      public ReleaseToken(void** punk)
      {
        if(punk == null)
          throw new ArgumentNullException("punk");
        _punk = punk;
      }

      /// <inheritdoc />
      public void Dispose()
      {
        void** punk = _punk;
        _punk = null;
        if(punk == null)
          return;
        UnknownRelease(*punk);
      }
    }
  }
}