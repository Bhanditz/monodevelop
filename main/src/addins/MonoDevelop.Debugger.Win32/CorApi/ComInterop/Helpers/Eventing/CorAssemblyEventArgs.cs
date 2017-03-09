namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorAssemblyEventArgs : CorAppDomainBaseEventArgs
    {
        public CorAssemblyEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugAssembly assembly)
            : base(appDomain)
        {
            Assembly = assembly;
        }

        public CorAssemblyEventArgs(ICorDebugAppDomain appDomain,
            ICorDebugAssembly assembly, ManagedCallbackType callbackType)
            : base(appDomain, callbackType)
        {
            Assembly = assembly;
        }

        /// <summary>
        /// The Assembly of interest.
        /// </summary>
        public ICorDebugAssembly Assembly { get; }

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnAssemblyLoad:
                return "Assembly loaded: " + LpwstrHelper.GetString(Assembly.GetName, "Could not get the assembly name.");
            case ManagedCallbackType.OnAssemblyUnload:
                return "Assembly unloaded: " + LpwstrHelper.GetString(Assembly.GetName, "Could not get the assembly name.");
            }
            return base.ToString();
        }
    }
}