namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorBreakpointEventArgs : CorThreadEventArgs
    {
        public CorBreakpointEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint managedBreakpoint)
            : base(appDomain, thread)
        {
            Breakpoint = managedBreakpoint;
        }

        public CorBreakpointEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint managedBreakpoint,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Breakpoint = managedBreakpoint;
        }

        /** The breakpoint involved. */
        public ICorDebugBreakpoint Breakpoint { get; }

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