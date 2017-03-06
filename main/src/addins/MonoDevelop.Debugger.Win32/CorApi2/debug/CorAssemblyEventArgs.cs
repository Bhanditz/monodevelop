using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorAssemblyEventArgs : CorAppDomainBaseEventArgs
    {
        private ICorDebugAssembly m_assembly;
        public CorAssemblyEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugAssembly assembly)
            : base(appDomain)
        {
            m_assembly = assembly;
        }

        public CorAssemblyEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugAssembly assembly, ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            m_assembly = assembly;
        }

        /** The Assembly of interest. */
        public ICorDebugAssembly Assembly
        {
            get
            {
                return m_assembly;
            }
        }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnAssemblyLoad:
                return "Assembly loaded: " + m_assembly.Name;
            case ManagedCallbackType.OnAssemblyUnload:
                return "Assembly unloaded: " + m_assembly.Name;
            }
            return base.ToString();
        }
    }
}