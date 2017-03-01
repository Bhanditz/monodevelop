using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
  public static unsafe class ICorDebugEnumEx
  {
    private static IEnumerable<ICorDebugAppDomain> AsEnumerable([NotNull] this ICorDebugAppDomainEnum comenumShared)
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