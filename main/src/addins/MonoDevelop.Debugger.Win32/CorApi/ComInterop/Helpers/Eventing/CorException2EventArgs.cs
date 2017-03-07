using System;

namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorException2EventArgs : CorThreadEventArgs
    {

        [CLSCompliant(false)]
        public CorException2EventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFrame frame,
            int offset,
            CorDebugExceptionCallbackType eventType,
            int flags)
            : base(appDomain, thread)
        {
            Frame = frame;
            Offset = offset;
            EventType = eventType;
            Flags = flags;
        }

        [CLSCompliant(false)]
        public CorException2EventArgs(ICorDebugAppDomain appDomain,
            ICorDebugThread thread,
            ICorDebugFrame frame,
            int offset,
            CorDebugExceptionCallbackType eventType,
            int flags,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Frame = frame;
            Offset = offset;
            EventType = eventType;
            Flags = flags;
        }

        public ICorDebugFrame Frame { get; }

        public int Offset { get; }

        [CLSCompliant(false)]
        public CorDebugExceptionCallbackType EventType { get; }

        public int Flags { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnException2)
            {
                return "Exception Thrown";
            }
            return base.ToString();
        }
    }
}