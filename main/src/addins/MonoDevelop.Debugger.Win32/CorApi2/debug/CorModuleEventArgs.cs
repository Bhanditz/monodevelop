using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorModuleEventArgs : CorAppDomainBaseEventArgs
    {
        readonly ICorDebugModule m_managedModule;

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
                return "Module loaded: " + LpcwstrHelper.GetString(m_managedModule.GetName, "Could not get the managed module name.");
            case ManagedCallbackType.OnModuleUnload:
                return "Module unloaded: " + LpcwstrHelper.GetString(m_managedModule.GetName, "Could not get the managed module name.");
            }
            return base.ToString();
        }
    }
}