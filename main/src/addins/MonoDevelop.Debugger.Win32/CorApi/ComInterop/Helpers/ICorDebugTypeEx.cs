using System;
using System.Linq;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugTypeEx
    {
        public static CorElementType Type([NotNull] this ICorDebugType cortype)
        {
            if(cortype == null)
                throw new ArgumentNullException(nameof(cortype));
            CorElementType ty;
            cortype.GetType(out ty).AssertSucceeded("Could not get the Element Type of a Type.");
            return ty;
        }

        public static ICorDebugType[] TypeParameters([NotNull] this ICorDebugType cortype)
        {
            if(cortype == null)
                throw new ArgumentNullException(nameof(cortype));
            ICorDebugTypeEnum enumTypeParams;
            cortype.EnumerateTypeParameters(out enumTypeParams).AssertSucceeded("Could not enumerate the Type Parameters of a Type.");
            return enumTypeParams.ToList<ICorDebugTypeEnum, ICorDebugType>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched)).ToArray();
        }
    }
}