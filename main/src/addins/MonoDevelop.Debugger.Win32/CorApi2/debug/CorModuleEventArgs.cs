using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorModuleEventArgs : CorAppDomainBaseEventArgs
    {
        ICorDebugModule m_managedModule;

        public CorModuleEventArgs(ICorDebugAppDomain appDomain, ICorDebugModule managedModule)
            : base(appDomain)
        {
            m_managedModule = managedModule;
        }

        public CorModuleEventArgs(ICorDebugAppDomain appDomain, ICorDebugModule managedModule,
            ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            m_managedModule = managedModule;
        }

        public ICorDebugModule Module
        {
            get
            {
                return m_managedModule;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnModuleLoad:
                return "Module loaded: " + m_managedModule.Name;
            case ManagedCallbackType.OnModuleUnload:
                return "Module unloaded: " + m_managedModule.Name;
            }
            return base.ToString();
        }
    }
}