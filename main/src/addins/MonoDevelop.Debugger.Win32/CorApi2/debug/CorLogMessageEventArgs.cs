using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorLogMessageEventArgs : CorThreadEventArgs
    {
        int m_level;
        string m_logSwitchName;
        string m_message;

        public CorLogMessageEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, string logSwitchName, string message)
            : base(appDomain, thread)
        {
            m_level = level;
            m_logSwitchName = logSwitchName;
            m_message = message;
        }

        public CorLogMessageEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, string logSwitchName, string message,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_level = level;
            m_logSwitchName = logSwitchName;
            m_message = message;
        }

        public int Level
        {
            get
            {
                return m_level;
            }
        }

        public string LogSwitchName
        {
            get
            {
                return m_logSwitchName;
            }
        }

        public string Message
        {
            get
            {
                return m_message;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnLogMessage)
            {
                return "Log message(" + m_logSwitchName + ")";
            }
            return base.ToString();
        }
    }
}