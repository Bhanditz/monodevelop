namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorDebuggerErrorEventArgs : CorProcessEventArgs
    {
        public CorDebuggerErrorEventArgs(ICorDebugProcess process, int hresult,
            int errorCode)
            : base(process)
        {
            HResult = hresult;
            ErrorCode = errorCode;
        }

        public CorDebuggerErrorEventArgs(ICorDebugProcess process, int hresult,
            int errorCode, ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
            HResult = hresult;
            ErrorCode = errorCode;
        }

        public int HResult { get; }

        public int ErrorCode { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnDebuggerError)
            {
                return "Debugger Error";
            }
            return base.ToString();
        }
    }
}