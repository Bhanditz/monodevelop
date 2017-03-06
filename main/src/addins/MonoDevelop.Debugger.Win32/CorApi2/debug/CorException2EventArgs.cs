using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorException2EventArgs : CorThreadEventArgs
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
            m_frame = frame;
            m_offset = offset;
            m_eventType = eventType;
            m_flags = flags;
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
            m_frame = frame;
            m_offset = offset;
            m_eventType = eventType;
            m_flags = flags;
        }

        public ICorDebugFrame Frame
        {
            get
            {
                return m_frame;
            }
        }

        public int Offset
        {
            get
            {
                return m_offset;
            }
        }

        [CLSCompliant(false)]
        public CorDebugExceptionCallbackType EventType
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
            if (CallbackType == ManagedCallbackType.OnException2)
            {
                return "Exception Thrown";
            }
            return base.ToString();
        }

        ICorDebugFrame m_frame;
        int m_offset;
        CorDebugExceptionCallbackType m_eventType;
        int m_flags;
    }
}