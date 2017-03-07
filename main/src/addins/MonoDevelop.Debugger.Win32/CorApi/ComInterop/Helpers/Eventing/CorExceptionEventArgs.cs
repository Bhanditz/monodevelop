namespace CorApi.ComInterop.Eventing
{
    public class CorExceptionEventArgs : CorThreadEventArgs
    {
        public CorExceptionEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            bool unhandled)
            : base(appDomain, thread)
        {
            Unhandled = unhandled;
        }

        public CorExceptionEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            bool unhandled,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Unhandled = unhandled;
        }

        /** Has the exception been handled yet? */
        public bool Unhandled { get; }
    }
}