using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorThreadEventArgs : CorAppDomainBaseEventArgs
    {
        public CorThreadEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread)
            : base(appDomain ?? GetThreadAppDomain(thread))
        {
            Thread = thread;
        }

        public CorThreadEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread, ManagedCallbackType callbackType)
            : base(appDomain ?? GetThreadAppDomain(thread), callbackType)
        {
            Thread = thread;
        }

        public override string ToString()
        {
            switch(CallbackType)
            {
            case ManagedCallbackType.OnBreak:
                return "Break";
            case ManagedCallbackType.OnCreateThread:
                return "Thread Created";
            case ManagedCallbackType.OnThreadExit:
                return "Thread Exited";
            case ManagedCallbackType.OnNameChange:
                return "Name Changed";
            }
            return base.ToString();
        }

        private static ICorDebugAppDomain GetThreadAppDomain(ICorDebugThread thread)
        {
            ICorDebugAppDomain appdomain;
            thread.GetAppDomain(out appdomain).AssertSucceeded("thread.GetAppDomain(out appdomain)");
            return appdomain;
        }
    }
}