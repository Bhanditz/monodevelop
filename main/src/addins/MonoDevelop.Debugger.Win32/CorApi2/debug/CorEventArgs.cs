using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public unsafe class CorEventArgs : EventArgs
    {
        private readonly ICorDebugController m_controller;

        private bool m_continue;


        private readonly ManagedCallbackType m_callbackType;

        private ICorDebugThread m_thread;

        public CorEventArgs(ICorDebugController controller)
        {
            m_controller = controller;
            m_continue = true;
        }

        public CorEventArgs(ICorDebugController controller, ManagedCallbackType callbackType)
        {
            m_controller = controller;
            m_continue = true;
            m_callbackType = callbackType;
        }

        /** The Controller of the current event. */
        public ICorDebugController Controller
        {
            get
            {
                return m_controller;
            }
        }

        /** 
         * The default behavior after an event is to Continue processing
         * after the event has been handled.  This can be changed by
         * setting this property to false.
         */
        public virtual bool Continue
        {
            get
            {
                return m_continue;
            }
            set
            {
                m_continue = value;
            }
        }

        /// <summary>
        /// The type of callback that returned this CorEventArgs object.
        /// </summary>
        public ManagedCallbackType CallbackType
        {
            get
            {
                return m_callbackType;
            }
        }

        /// <summary>
        /// The CorThread associated with the callback event that returned
        /// this CorEventArgs object. If here is no such thread, Thread is null.
        /// </summary>
        public ICorDebugThread Thread
        {
            get
            {
                return m_thread;
            }
            protected set
            {
                m_thread = value;
            }
        }

    }
}