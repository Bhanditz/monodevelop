using System;

namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorUpdateModuleSymbolsEventArgs : CorModuleEventArgs
    {
        [CLSCompliant(false)]
        public CorUpdateModuleSymbolsEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugModule managedModule,
            IStream stream)
            : base(appDomain, managedModule)
        {
            Stream = stream;
        }

        [CLSCompliant(false)]
        public CorUpdateModuleSymbolsEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugModule managedModule,
            IStream stream,
            ManagedCallbackType callbackType)
            : base(appDomain, managedModule, callbackType)
        {
            Stream = stream;
        }

        [CLSCompliant(false)]
        public IStream Stream { get; }

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