using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorAssemblyEventArgs : CorAppDomainBaseEventArgs
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
                return "Assembly loaded: " + LpcwstrHelper.GetString((UInt32 cchBuffer, UInt16*pBuffer, UInt32*pcchActual) => m_assembly.GetName(cchBuffer, pcchActual, pBuffer), "Could not get the assembly name.");
            case ManagedCallbackType.OnAssemblyUnload:
                return "Assembly unloaded: " + LpcwstrHelper.GetString((UInt32 cchBuffer, UInt16*pBuffer, UInt32*pcchActual) => m_assembly.GetName(cchBuffer, pcchActual, pBuffer), "Could not get the assembly name.");
            }
            return base.ToString();
        }
    }
}