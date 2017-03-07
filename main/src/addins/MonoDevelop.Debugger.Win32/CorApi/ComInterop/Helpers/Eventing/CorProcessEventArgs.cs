namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorProcessEventArgs : CorEventArgs
    {
        public CorProcessEventArgs(ICorDebugProcess process)
            : base(process)
        {
        }

        public CorProcessEventArgs(ICorDebugProcess process, ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
        }

        /** The process that generated the event. */
        public ICorDebugProcess Process => (ICorDebugProcess)Controller;

        public override string ToString()
        {
            switch (CallbackType)
            {
            case ManagedCallbackType.OnCreateProcess:
                return "Process Created";
            case ManagedCallbackType.OnProcessExit:
                return "Process Exited";
            case ManagedCallbackType.OnControlCTrap:
                break;
            }
            return base.ToString();
        }
    }
}