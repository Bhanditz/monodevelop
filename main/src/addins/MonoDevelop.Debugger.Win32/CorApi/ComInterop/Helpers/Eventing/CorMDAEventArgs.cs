namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorMDAEventArgs : CorProcessEventArgs
    {
        // Thread may be null.
        public CorMDAEventArgs(ICorDebugMDA mda, ICorDebugThread thread, ICorDebugProcess proc)
            : base(proc)
        {
            MDA = mda;
            Thread = thread;
            //m_proc = proc;
        }

        public CorMDAEventArgs(ICorDebugMDA mda, ICorDebugThread thread, ICorDebugProcess proc,
            ManagedCallbackType callbackType)
            : base(proc, callbackType)
        {
            MDA = mda;
            Thread = thread;
            //m_proc = proc;
        }

        public ICorDebugMDA MDA { get; }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnMDANotification)
            {
                return "MDANotification" + "\n" +
                    "Name=" + LpcwstrHelper.GetString(MDA.GetName, "MDA Name.") + "\n" +
                    "XML=" + LpcwstrHelper.GetString(MDA.GetXML, "MDA XML");
            }
            return base.ToString();
        }

        //ICorDebugProcess m_proc;
        //ICorDebugProcess Process { get { return m_proc; } }
    }
}