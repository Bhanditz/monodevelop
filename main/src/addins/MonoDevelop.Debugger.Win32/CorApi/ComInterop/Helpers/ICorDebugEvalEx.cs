namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugEvalEx
    {
        public static void CallParameterizedFunction(this ICorDebugEval corEval, ICorDebugFunction managedFunction, ICorDebugType[] argumentTypes, ICorDebugValue[] arguments)
        {
            var types = new void*[argumentTypes?.Length ?? 0];
            var args = new void*[arguments?.Length ?? 0];

            try
            {
                if(argumentTypes != null)
                {
                    for(int a = 0; a < argumentTypes.Length; a++)
                        types[a] = Com.UnknownAddRef(argumentTypes[a]);
                }
                if(arguments != null)
                {
                    for(int a = 0; a < arguments.Length; a++)
                        args[a] = Com.UnknownAddRef(arguments[a]);
                }
                fixed(void** ppTypes = types)
                fixed(void** ppArgs = args)
                    Com.QueryInteface<ICorDebugEval2>(corEval).CallParameterizedFunction(managedFunction, ((uint)types.Length), ppTypes, (uint)args.Length, ppArgs).AssertSucceeded($"Could not make a parameterized call to a managed function with {types.Length} actual type parameters for classes and function, and {args.Length} actual parameters.");
            }
            finally
            {
                foreach(void* pType in types)
                    Com.UnknownRelease(pType);
                foreach(void* pArg in args)
                    Com.UnknownRelease(pArg);
            }
        }

        public static void NewParameterizedObject(this ICorDebugEval corEval, ICorDebugFunction managedFunction, ICorDebugType[] argumentTypes, ICorDebugValue[] arguments)
        {
            var types = new void*[argumentTypes?.Length ?? 0];
            var args = new void*[arguments?.Length ?? 0];

            try
            {
                if(argumentTypes != null)
                {
                    for(int a = 0; a < argumentTypes.Length; a++)
                        types[a] = Com.UnknownAddRef(argumentTypes[a]);
                }
                if(arguments != null)
                {
                    for(int a = 0; a < arguments.Length; a++)
                        args[a] = Com.UnknownAddRef(arguments[a]);
                }
                fixed(void** ppTypes = types)
                fixed(void** ppArgs = args)
                    Com.QueryInteface<ICorDebugEval2>(corEval).NewParameterizedObject(managedFunction, ((uint)types.Length), ppTypes, (uint)args.Length, ppArgs).AssertSucceeded($"Could not make a parameterized managed object creation call with {types.Length} actual type parameters and {args.Length} actual parameters.");
            }
            finally
            {
                foreach(void* pType in types)
                    Com.UnknownRelease(pType);
                foreach(void* pArg in args)
                    Com.UnknownRelease(pArg);
            }
        }
    }
} /* namespace */