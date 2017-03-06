using System;
using System.Collections.Generic;

namespace CorApi2.Metadata
{
    public unsafe class Instantiation
    {
        public static readonly Instantiation Empty = new Instantiation (new List<Type> ());

        public static Instantiation Create (IList<Type> typeArgs)
        {
            return new Instantiation (typeArgs);
        }

        public IList<Type> TypeArgs { get; private set; }

        Instantiation (IList<Type> typeArgs)
        {
            TypeArgs = typeArgs;
        }
    }
}