namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorLogMessageEventArgs : CorThreadEventArgs
    {
        public CorLogMessageEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, string logSwitchName, string message)
            : base(appDomain, thread)
        {
            Level = level;
            LogSwitchName = logSwitchName;
            Message = message;
        }

        public CorLogMessageEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            int level, string logSwitchName, string message,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Level = level;
            LogSwitchName = logSwitchName;
            Message = message;
        }

        public int Level { get; }

        public string LogSwitchName { get; }

        public string Message { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnLogMessage)
            {
                return "Log message(" + LogSwitchName + ")";
            }
            return base.ToString();
        }
    }
}