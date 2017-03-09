using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugClassEx
    {
        public static ICorDebugType GetParameterizedType([NotNull] this ICorDebugClass corclass, CorElementType elementType, [CanBeNull] IList<ICorDebugType> typeArguments)
        {
            if(corclass == null)
                throw new ArgumentNullException(nameof(corclass));

            typeArguments = typeArguments ?? new ICorDebugType[] { };
            uint nTypeArgs = (uint)typeArguments.Count;
            var typeargs = new void*[typeArguments.Count];
            try
            {
                for(uint a = nTypeArgs; a-- > 0;)
                    typeargs[a] = Com.UnknownAddRef(typeArguments[(int)a]);

                void* pType = null;
                using(Com.UsingReference(&pType))
                {
                    fixed(void** ppArgs = typeargs)
                        Com.QueryInteface<ICorDebugClass2>(corclass).GetParameterizedType(elementType, nTypeArgs, ppArgs, &pType).AssertSucceeded("Could not get the parameterized type.");
                    return Com.QueryInteface<ICorDebugType>(pType);
                }
            }
            finally
            {
                foreach(void* pArg in typeargs)
                    Com.UnknownRelease(pArg);
            }
        }
    }
}