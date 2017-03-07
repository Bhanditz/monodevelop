using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorMDAEventArgs : CorProcessEventArgs
    {
        // Thread may be null.
        public CorMDAEventArgs(ICorDebugMDA mda, ICorDebugThread thread, ICorDebugProcess proc)
            : base(proc)
        {
            m_mda = mda;
            Thread = thread;
            //m_proc = proc;
        }

        public CorMDAEventArgs(ICorDebugMDA mda, ICorDebugThread thread, ICorDebugProcess proc,
            ManagedCallbackType callbackType)
            : base(proc, callbackType)
        {
            m_mda = mda;
            Thread = thread;
            //m_proc = proc;
        }

        readonly ICorDebugMDA m_mda;
        public ICorDebugMDA MDA { get { return m_mda; } }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnMDANotification)
            {
                return "MDANotification" + "\n" +
                    "Name=" + LpcwstrHelper.GetString(m_mda.GetName, "MDA Name.") + "\n" +
                    "XML=" + LpcwstrHelper.GetString(m_mda.GetXML, "MDA XML");
            }
            return base.ToString();
        }

        //ICorDebugProcess m_proc;
        //ICorDebugProcess Process { get { return m_proc; } }
    }
}