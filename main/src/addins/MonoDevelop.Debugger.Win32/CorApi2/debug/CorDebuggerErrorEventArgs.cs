using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorDebuggerErrorEventArgs : CorProcessEventArgs
    {
        readonly int m_hresult;

        readonly int m_errorCode;

        public CorDebuggerErrorEventArgs(ICorDebugProcess process, int hresult,
            int errorCode)
            : base(process)
        {
            m_hresult = hresult;
            m_errorCode = errorCode;
        }

        public CorDebuggerErrorEventArgs(ICorDebugProcess process, int hresult,
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