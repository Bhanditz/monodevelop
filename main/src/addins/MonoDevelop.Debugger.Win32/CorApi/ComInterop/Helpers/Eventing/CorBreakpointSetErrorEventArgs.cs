namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorBreakpointSetErrorEventArgs : CorThreadEventArgs
    {
        public CorBreakpointSetErrorEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint breakpoint,
            int errorCode)
            : base(appDomain, thread)
        {
            Breakpoint = breakpoint;
            ErrorCode = errorCode;
        }

        public CorBreakpointSetErrorEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugBreakpoint breakpoint,
            int errorCode,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Breakpoint = breakpoint;
            ErrorCode = errorCode;
        }

        public ICorDebugBreakpoint Breakpoint { get; }

        public int ErrorCode { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnBreakpointSetError)
            {
                return "Error Setting Breakpoint";
            }
            return base.ToString();
        }
    }
}