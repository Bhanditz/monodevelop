using System;
using System.Collections.Generic;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugChainEx
    {
        public static IList<ICorDebugFrame> GetFrames(this ICorDebugChain @this)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            ICorDebugFrameEnum @enum;
            @this.EnumerateFrames(out @enum).AssertSucceeded("Cannot enumerate chain frames.");

            return @enum.ToList<ICorDebugFrameEnum, ICorDebugFrame>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched));
        }
    }
}