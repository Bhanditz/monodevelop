namespace CorApi2.debug
{
    public class CorProcessEventArgs : CorEventArgs
    {
        public CorProcessEventArgs(CorProcess process)
            : base(process)
        {
        }

        public CorProcessEventArgs(CorProcess process, ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
        }

        /** The process that generated the event. */
        public CorProcess Process
        {
            get
            {
                return (CorProcess)Controller;
            }
        }

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