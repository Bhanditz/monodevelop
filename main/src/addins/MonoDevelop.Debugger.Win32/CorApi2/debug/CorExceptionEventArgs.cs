using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorExceptionEventArgs : CorThreadEventArgs
    {
        bool m_unhandled;

        public CorExceptionEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            bool unhandled)
            : base(appDomain, thread)
        {
            m_unhandled = unhandled;
        }

        public CorExceptionEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            bool unhandled,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_unhandled = unhandled;
        }

        /** Has the exception been handled yet? */
        public bool Unhandled
        {
            get
            {
                return m_unhandled;
            }
        }
    }
}