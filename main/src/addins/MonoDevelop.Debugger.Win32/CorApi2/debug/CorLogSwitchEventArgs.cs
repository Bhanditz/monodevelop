using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorLogSwitchEventArgs : CorThreadEventArgs
    {
        int m_level;

        int m_reason;

        string m_logSwitchName;

        string m_parentName;

        public CorLogSwitchEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, int reason, string logSwitchName, string parentName)
            : base(appDomain, thread)
        {
            m_level = level;
            m_reason = reason;
            m_logSwitchName = logSwitchName;
            m_parentName = parentName;
        }

        public CorLogSwitchEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, int reason, string logSwitchName, string parentName,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_level = level;
            m_reason = reason;
            m_logSwitchName = logSwitchName;
            m_parentName = parentName;
        }

        public int Level
        {
            get
            {
                return m_level;
            }
        }

        public int Reason
        {
            get
            {
                return m_reason;
            }
        }

        public string LogSwitchName
        {
            get
            {
                return m_logSwitchName;
            }
        }

        public string ParentName
        {
            get
            {
                return m_parentName;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnLogSwitch)
            {
                return "Log Switch" + "\n" +
                    "Level: " + m_level + "\n" +
                    "Log Switch Name: " + m_logSwitchName;
            }
            return base.ToString();
        }
    }
}