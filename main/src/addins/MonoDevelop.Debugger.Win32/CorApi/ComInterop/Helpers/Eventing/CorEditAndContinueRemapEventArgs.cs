namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorEditAndContinueRemapEventArgs : CorThreadEventArgs
    {
        public CorEditAndContinueRemapEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction,
            int accurate)
            : base(appDomain, thread)
        {
            Function = managedFunction;
            m_accurate = accurate;
        }

        public CorEditAndContinueRemapEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction,
            int accurate,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Function = managedFunction;
            m_accurate = accurate;
        }

        public ICorDebugFunction Function { get; }

        public bool IsAccurate
        {
            get
            {
                return m_accurate != 0;
            }
        }

        private readonly int m_accurate;
    }
}