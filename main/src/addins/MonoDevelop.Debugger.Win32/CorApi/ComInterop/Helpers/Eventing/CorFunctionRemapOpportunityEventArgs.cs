namespace CorApi.ComInterop.Eventing
{
    public sealed class CorFunctionRemapOpportunityEventArgs : CorThreadEventArgs
    {
        public CorFunctionRemapOpportunityEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction oldFunction,
            ICorDebugFunction newFunction,
            int oldILoffset
        )
            : base(appDomain, thread)
        {
            OldFunction = oldFunction;
            NewFunction = newFunction;
            OldILOffset = oldILoffset;
        }

        public CorFunctionRemapOpportunityEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction oldFunction,
            ICorDebugFunction newFunction,
            int oldILoffset,
            ManagedCallbackType callbackType
        )
            : base(appDomain, thread, callbackType)
        {
            OldFunction = oldFunction;
            NewFunction = newFunction;
            OldILOffset = oldILoffset;
        }

        public ICorDebugFunction OldFunction { get; }

        public ICorDebugFunction NewFunction { get; }

        public int OldILOffset { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnFunctionRemapOpportunity)
            {
                return "Function Remap Opportunity";
            }
            return base.ToString();
        }
    }
}