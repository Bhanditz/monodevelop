using System;

namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorExceptionUnwind2EventArgs : CorThreadEventArgs
    {

        [CLSCompliant(false)]
        public CorExceptionUnwind2EventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            CorDebugExceptionUnwindCallbackType eventType,
            int flags)
            : base(appDomain, thread)
        {
            EventType = eventType;
            Flags = flags;
        }

        [CLSCompliant(false)]
        public CorExceptionUnwind2EventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            CorDebugExceptionUnwindCallbackType eventType,
            int flags,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            EventType = eventType;
            Flags = flags;
        }

        [CLSCompliant(false)]
        public CorDebugExceptionUnwindCallbackType EventType { get; }

        public int Flags { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnExceptionUnwind2)
            {
                return "Exception unwind\n" +
                    "EventType: " + EventType;
            }
            return base.ToString();
        }
    }
}