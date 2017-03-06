using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorEvalEventArgs : CorThreadEventArgs
    {
        ICorDebugEval m_eval;

        public CorEvalEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugEval eval)
            : base(appDomain, thread)
        {
            m_eval = eval;
        }

        public CorEvalEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugEval eval, ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_eval = eval;
        }

        /** The object being evaluated. */
        public ICorDebugEval Eval
        {
            get
            {
                return m_eval;
            }
        }

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