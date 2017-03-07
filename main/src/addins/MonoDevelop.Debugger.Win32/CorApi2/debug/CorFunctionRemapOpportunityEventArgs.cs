using CorApi.ComInterop;

namespace CorApi2.debug
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
            m_oldFunction = oldFunction;
            m_newFunction = newFunction;
            m_oldILoffset = oldILoffset;
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
            m_oldFunction = oldFunction;
            m_newFunction = newFunction;
            m_oldILoffset = oldILoffset;
        }

        public ICorDebugFunction OldFunction
        {
            get
            {
                return m_oldFunction;
            }
        }

        public ICorDebugFunction NewFunction
        {
            get
            {
                return m_newFunction;
            }
        }

        public int OldILOffset
        {
            get
            {
                return m_oldILoffset;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnFunctionRemapOpportunity)
            {
                return "Function Remap Opportunity";
            }
            return base.ToString();
        }

        private readonly ICorDebugFunction m_oldFunction;

        private readonly ICorDebugFunction m_newFunction;

        private readonly int m_oldILoffset;
    }
}