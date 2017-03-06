using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public sealed class CorExceptionInCallbackEventArgs : CorEventArgs
    {
        public CorExceptionInCallbackEventArgs(ICorDebugController controller, Exception exceptionThrown)
            : base(controller)
        {
            m_exceptionThrown = exceptionThrown;
        }

        public CorExceptionInCallbackEventArgs(ICorDebugController controller, Exception exceptionThrown,
            ManagedCallbackType callbackType)
            : base(controller, callbackType)
        {
            m_exceptionThrown = exceptionThrown;
        }

        public Exception ExceptionThrown
        {
            get
            {
                return m_exceptionThrown;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnExceptionInCallback)
            {
                return "Callback Exception: " + m_exceptionThrown.Message;
            }
            return base.ToString();
        }

        private Exception m_exceptionThrown;
    }
}