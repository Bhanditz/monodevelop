using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorUpdateModuleSymbolsEventArgs : CorModuleEventArgs
    {
        IStream m_stream;

        [CLSCompliant(false)]
        public CorUpdateModuleSymbolsEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugModule managedModule,
            IStream stream)
            : base(appDomain, managedModule)
        {
            m_stream = stream;
        }

        [CLSCompliant(false)]
        public CorUpdateModuleSymbolsEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugModule managedModule,
            IStream stream,
            ManagedCallbackType callbackType)
            : base(appDomain, managedModule, callbackType)
        {
            m_stream = stream;
        }

        [CLSCompliant(false)]
        public IStream Stream
        {
            get
            {
                return m_stream;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnUpdateModuleSymbols)
            {
                return "Module Symbols Updated";
            }
            return base.ToString();
        }
    }
}