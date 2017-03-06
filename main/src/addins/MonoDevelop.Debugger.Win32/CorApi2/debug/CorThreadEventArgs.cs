using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorThreadEventArgs : CorAppDomainBaseEventArgs
    {
        public CorThreadEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread)
            : base(appDomain != null ? appDomain : thread.AppDomain)
        {
            Thread = thread;
        }

        public CorThreadEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ManagedCallbackType callbackType)
            : base(appDomain != null ? appDomain : thread.AppDomain, callbackType)
        {
            Thread = thread;
        }

        public override string ToString()
        {
            switch (CallbackType)
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
    }
}