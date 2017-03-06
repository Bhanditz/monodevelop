using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorAppDomainBaseEventArgs : CorEventArgs
    {
        public CorAppDomainBaseEventArgs(ICorDebugAppDomain ad)
            : base(ad)
        {
        }

        public CorAppDomainBaseEventArgs(ICorDebugAppDomain ad, ManagedCallbackType callbackType)
            : base(ad, callbackType)
        {
        }

        public ICorDebugAppDomain AppDomain
        {
            get
            {
                return (ICorDebugAppDomain)Controller;
            }
        }
    }
}