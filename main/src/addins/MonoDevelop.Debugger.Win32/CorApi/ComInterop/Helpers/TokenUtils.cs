using System;
using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public static class TokenUtils
    {
        public static bool IsNullToken(UInt32 token)
        {
            return (RidFromToken(token) == 0);
        }

        public static int RidFromToken(UInt32 token)
        {
            return (int)(token & 0x00ffffff);
        }

        public static CorTokenType TypeFromToken(UInt32 token)
        {
            return (CorTokenType)(token & 0xff000000);
        }
    }
}