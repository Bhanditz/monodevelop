namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorAppDomainEventArgs : CorProcessEventArgs
    {
        public CorAppDomainEventArgs(ICorDebugProcess process, ICorDebugAppDomain ad)
            : base(process)
        {
            AppDomain = ad;
        }

        public CorAppDomainEventArgs(ICorDebugProcess process, ICorDebugAppDomain ad,
            ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
            AppDomain = ad;
        }

        /** The AppDomain that generated the event. */
        public ICorDebugAppDomain AppDomain { get; }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnCreateAppDomain:
                return "AppDomain Created: " + LpwstrHelper.GetString(AppDomain.GetName, "Could not get appdomain name.");
            case ManagedCallbackType.OnAppDomainExit:
                return "AppDomain Exited: " + LpwstrHelper.GetString(AppDomain.GetName, "Could not get appdomain name.");
            }
            return base.ToString();
        }
    }
}