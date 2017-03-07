namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorEvalEventArgs : CorThreadEventArgs
    {
        public CorEvalEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugEval eval)
            : base(appDomain, thread)
        {
            Eval = eval;
        }

        public CorEvalEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugEval eval, ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Eval = eval;
        }

        /** The object being evaluated. */
        public ICorDebugEval Eval { get; }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnEvalComplete:
                return "Eval Complete";
            case ManagedCallbackType.OnEvalException:
                return "Eval Exception";
            }
            return base.ToString();
        }
    }
}