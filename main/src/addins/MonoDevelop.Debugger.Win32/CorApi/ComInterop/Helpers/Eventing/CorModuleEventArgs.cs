namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorModuleEventArgs : CorAppDomainBaseEventArgs
    {
        public CorModuleEventArgs(ICorDebugAppDomain appDomain, ICorDebugModule managedModule)
            : base(appDomain)
        {
            Module = managedModule;
        }

        public CorModuleEventArgs(ICorDebugAppDomain appDomain, ICorDebugModule managedModule,
            ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            Module = managedModule;
        }

        public ICorDebugModule Module { get; }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnModuleLoad:
                return "Module loaded: " + LpwstrHelper.GetString(Module.GetName, "Could not get the managed module name.");
            case ManagedCallbackType.OnModuleUnload:
                return "Module unloaded: " + LpwstrHelper.GetString(Module.GetName, "Could not get the managed module name.");
            }
            return base.ToString();
        }
    }
}