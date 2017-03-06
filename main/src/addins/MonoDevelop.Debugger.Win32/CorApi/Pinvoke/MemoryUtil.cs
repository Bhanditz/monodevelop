using System.Runtime.CompilerServices;

namespace CorApi.Pinvoke
{
  public static class MemoryUtil
  {
    public const int ByteBufferUnderlohSize = 81920;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void ZeroMemory(void* p, uint cb)
    {
      var pb = (byte*)p;
      if(cb >= 0x10)
      {
        do
        {
          *((int*)pb) = 0;
          *((int*)(pb + 4)) = 0;
          *((int*)(pb + 8)) = 0;
          *((int*)(pb + 12)) = 0;
          pb += 0x10;
        } while((cb -= 0x10) >= 0x10);
      }
      if(cb > 0)
      {
        if((cb & 8) != 0)
        {
          *((int*)pb) = 0;
          *((int*)(pb + 4)) = 0;
          pb += 8;
        }
        if((cb & 4) != 0)
        {
          *((int*)pb) = 0;
          pb += 4;
        }
        if((cb & 2) != 0)
        {
          *((short*)pb) = 0;
          pb += 2;
        }
        if((cb & 1) != 0)
        {
          pb[0] = 0;
          pb++;
        }
      }
    }
    
  }
}