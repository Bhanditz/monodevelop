namespace CorApi2.debug
{
    public unsafe class CorDebuggerErrorEventArgs : CorProcessEventArgs
    {
        int m_hresult;
        int m_errorCode;

        public CorDebuggerErrorEventArgs(CorProcess process, int hresult,
            int errorCode)
            : base(process)
        {
            m_hresult = hresult;
            m_errorCode = errorCode;
        }

        public CorDebuggerErrorEventArgs(CorProcess process, int hresult,
            int errorCode, ManagedCallbackType callbackType)
            : base(process, callbackType)
        {
            m_hresult = hresult;
            m_errorCode = errorCode;
        }

        public int HResult
        {
            get
            {
                return m_hresult;
            }
        }

        public int ErrorCode
        {
            get
            {
                return m_errorCode;
            }
        }

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