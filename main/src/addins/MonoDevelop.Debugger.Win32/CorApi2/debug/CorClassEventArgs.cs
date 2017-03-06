using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorClassEventArgs : CorAppDomainBaseEventArgs
    {
        ICorDebugClass m_class;

        public CorClassEventArgs(ICorDebugAppDomain appDomain, ICorDebugClass managedClass)
            : base(appDomain)
        {
            m_class = managedClass;
        }

        public CorClassEventArgs(ICorDebugAppDomain appDomain, ICorDebugClass managedClass,
            ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            m_class = managedClass;
        }

        public ICorDebugClass Class
        {
            get
            {
                return m_class;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnClassLoad:
                return "Class loaded: " + m_class;
            case ManagedCallbackType.OnClassUnload:
                return "Class unloaded: " + m_class;
            }
            return base.ToString();
        }
    }
}