using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorEditAndContinueRemapEventArgs : CorThreadEventArgs
    {
        public CorEditAndContinueRemapEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction,
            int accurate)
            : base(appDomain, thread)
        {
            m_managedFunction = managedFunction;
            m_accurate = accurate;
        }

        public CorEditAndContinueRemapEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction,
            int accurate,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_managedFunction = managedFunction;
            m_accurate = accurate;
        }

        public ICorDebugFunction Function
        {
            get
            {
                return m_managedFunction;
            }
        }

        public bool IsAccurate
        {
            get
            {
                return m_accurate != 0;
            }
        }

        private ICorDebugFunction m_managedFunction;
        private int m_accurate;
    }
}