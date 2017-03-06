using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorAppDomainEventArgs : CorProcessEventArgs
    {
        private ICorDebugAppDomain m_ad;

        public CorAppDomainEventArgs(CorProcess process, ICorDebugAppDomain ad)
            : base(process)
        {
            m_ad = ad;
        }

        public CorAppDomainEventArgs(CorProcess process, ICorDebugAppDomain ad,
            ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
            m_ad = ad;
        }

        /** The AppDomain that generated the event. */
        public ICorDebugAppDomain AppDomain
        {
            get
            {
                return m_ad;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnCreateAppDomain:
                return "AppDomain Created: " + m_ad.Name;
            case ManagedCallbackType.OnAppDomainExit:
                return "AppDomain Exited: " + m_ad.Name;
            }
            return base.ToString();
        }
    }
}