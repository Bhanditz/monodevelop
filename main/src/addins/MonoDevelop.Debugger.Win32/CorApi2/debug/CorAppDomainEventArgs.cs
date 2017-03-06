using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorAppDomainEventArgs : CorProcessEventArgs
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
                return "AppDomain Created: " + LpcwstrHelper.GetString((a, b, c) => m_ad.GetName(a, c, b), "Could not get appdomain name.");
            case ManagedCallbackType.OnAppDomainExit:
                return "AppDomain Exited: " + LpcwstrHelper.GetString((a, b, c) => m_ad.GetName(a, c, b), "Could not get appdomain name.");
            }
            return base.ToString();
        }
    }
}