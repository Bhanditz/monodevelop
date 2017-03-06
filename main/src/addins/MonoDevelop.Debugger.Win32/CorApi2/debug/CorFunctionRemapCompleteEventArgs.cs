using CorApi.ComInterop;

namespace CorApi2.debug
{
    public sealed class CorFunctionRemapCompleteEventArgs : CorThreadEventArgs
    {
        public CorFunctionRemapCompleteEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction
        )
            : base(appDomain, thread)
        {
            m_managedFunction = managedFunction;
        }

        public CorFunctionRemapCompleteEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFunction managedFunction,
            ManagedCallbackType callbackType
        )
            : base(appDomain, thread, callbackType)
        {
            m_managedFunction = managedFunction;
        }

        public ICorDebugFunction Function
        {
            get
            {
                return m_managedFunction;
            }
        }

        private ICorDebugFunction m_managedFunction;
    }
}