using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorMDAEventArgs : CorProcessEventArgs
    {
        // Thread may be null.
        public CorMDAEventArgs(ICorDebugMDA mda, ICorDebugThread thread, CorProcess proc)
            : base(proc)
        {
            m_mda = mda;
            Thread = thread;
            //m_proc = proc;
        }

        public CorMDAEventArgs(ICorDebugMDA mda, ICorDebugThread thread, CorProcess proc,
            ManagedCallbackType callbackType)
            : base(proc, callbackType)
        {
            m_mda = mda;
            Thread = thread;
            //m_proc = proc;
        }

        ICorDebugMDA m_mda;
        public ICorDebugMDA MDA { get { return m_mda; } }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnMDANotification)
            {
                return "MDANotification" + "\n" +
                    "Name=" + m_mda.Name + "\n" +
                    "XML=" + m_mda.XML;
            }
            return base.ToString();
        }

        //CorProcess m_proc;
        //CorProcess Process { get { return m_proc; } }
    }
}