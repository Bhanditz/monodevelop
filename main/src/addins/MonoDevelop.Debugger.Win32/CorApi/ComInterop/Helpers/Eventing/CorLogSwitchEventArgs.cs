namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorLogSwitchEventArgs : CorThreadEventArgs
    {
        public CorLogSwitchEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, int reason, string logSwitchName, string parentName)
            : base(appDomain, thread)
        {
            Level = level;
            Reason = reason;
            LogSwitchName = logSwitchName;
            ParentName = parentName;
        }

        public CorLogSwitchEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, int reason, string logSwitchName, string parentName,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Level = level;
            Reason = reason;
            LogSwitchName = logSwitchName;
            ParentName = parentName;
        }

        public int Level { get; }

        public int Reason { get; }

        public string LogSwitchName { get; }

        public string ParentName { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnLogSwitch)
            {
                return "Log Switch" + "\n" +
                    "Level: " + Level + "\n" +
                    "Log Switch Name: " + LogSwitchName;
            }
            return base.ToString();
        }
    }
}