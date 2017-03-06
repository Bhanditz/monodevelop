using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorExceptionUnwind2EventArgs : CorThreadEventArgs
    {

        [CLSCompliant(false)]
        public CorExceptionUnwind2EventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            CorDebugExceptionUnwindCallbackType eventType,
            int flags)
            : base(appDomain, thread)
        {
            m_eventType = eventType;
            m_flags = flags;
        }

        [CLSCompliant(false)]
        public CorExceptionUnwind2EventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            CorDebugExceptionUnwindCallbackType eventType,
            int flags,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_eventType = eventType;
            m_flags = flags;
        }

        [CLSCompliant(false)]
        public CorDebugExceptionUnwindCallbackType EventType
        {
            get
            {
                return m_eventType;
            }
        }

        public int Flags
        {
            get
            {
                return m_flags;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnExceptionUnwind2)
            {
                return "Exception unwind\n" +
                    "EventType: " + m_eventType;
            }
            return base.ToString();
        }

        CorDebugExceptionUnwindCallbackType m_eventType;
        int m_flags;
    }
}