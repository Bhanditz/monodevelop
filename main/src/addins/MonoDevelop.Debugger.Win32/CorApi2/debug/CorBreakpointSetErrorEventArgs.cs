using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorBreakpointSetErrorEventArgs : CorThreadEventArgs
    {
        public CorBreakpointSetErrorEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint breakpoint,
            int errorCode)
            : base(appDomain, thread)
        {
            m_breakpoint = breakpoint;
            m_errorCode = errorCode;
        }

        public CorBreakpointSetErrorEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint breakpoint,
            int errorCode,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_breakpoint = breakpoint;
            m_errorCode = errorCode;
        }

        public ICorDebugBreakpoint Breakpoint
        {
            get
            {
                return m_breakpoint;
            }
        }

        public int ErrorCode
        {
            get
            {
                return m_errorCode;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnBreakpointSetError)
            {
                return "Error Setting Breakpoint";
            }
            return base.ToString();
        }

        private readonly ICorDebugBreakpoint m_breakpoint;
        private readonly int m_errorCode;
    }
}