using System;

namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorEventArgs : EventArgs
    {
        private bool m_continue;

        public CorEventArgs(ICorDebugController controller)
        {
            Controller = controller;
            m_continue = true;
        }

        public CorEventArgs(ICorDebugController controller, ManagedCallbackType callbackType)
        {
            Controller = controller;
            m_continue = true;
            CallbackType = callbackType;
        }

        /// <summary>
        /// The Controller of the current event.
        /// </summary>
        public ICorDebugController Controller { get; }

        /// <summary>
        /// The default behavior after an event is to Continue processing
        /// after the event has been handled.  This can be changed by
        /// setting this property to false.
        /// </summary>
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
        public ManagedCallbackType CallbackType { get; }

        /// <summary>
        /// The CorThread associated with the callback event that returned
        /// this CorEventArgs object. If here is no such thread, Thread is null.
        /// </summary>
        public ICorDebugThread Thread { get; protected set; }
    }
}