using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorBreakpointEventArgs : CorThreadEventArgs
    {
        private ICorDebugBreakpoint m_break;

        public CorBreakpointEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint managedBreakpoint)
            : base(appDomain, thread)
        {
            m_break = managedBreakpoint;
        }

        public CorBreakpointEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint managedBreakpoint,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_break = managedBreakpoint;
        }

        /** The breakpoint involved. */
        public ICorDebugBreakpoint Breakpoint
        {
            get
            {
                return m_break;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnBreakpoint)
            {
                return "Breakpoint Hit";
            }
            return base.ToString();
        }
    }
}