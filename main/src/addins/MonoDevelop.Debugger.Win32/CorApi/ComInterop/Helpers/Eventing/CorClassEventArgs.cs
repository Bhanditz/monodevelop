namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorClassEventArgs : CorAppDomainBaseEventArgs
    {
        public CorClassEventArgs(ICorDebugAppDomain appDomain, ICorDebugClass managedClass)
            : base(appDomain)
        {
            Class = managedClass;
        }

        public CorClassEventArgs(ICorDebugAppDomain appDomain, ICorDebugClass managedClass,
            ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            Class = managedClass;
        }

        public ICorDebugClass Class { get; }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnClassLoad:
                return "Class loaded: " + Class;
            case ManagedCallbackType.OnClassUnload:
                return "Class unloaded: " + Class;
            }
            return base.ToString();
        }
    }
}