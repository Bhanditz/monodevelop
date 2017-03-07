namespace CorApi.ComInterop.Eventing
{
    public sealed class CorFunctionRemapCompleteEventArgs : CorThreadEventArgs
    {
        public CorFunctionRemapCompleteEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction
        )
            : base(appDomain, thread)
        {
            Function = managedFunction;
        }

        public CorFunctionRemapCompleteEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction,
            ManagedCallbackType callbackType
        )
            : base(appDomain, thread, callbackType)
        {
            Function = managedFunction;
        }

        public ICorDebugFunction Function { get; }
    }
}