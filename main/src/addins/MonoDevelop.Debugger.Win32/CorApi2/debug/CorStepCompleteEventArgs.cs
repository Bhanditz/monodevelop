using System;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public class CorStepCompleteEventArgs : CorThreadEventArgs
    {
        private ICorDebugStepper m_stepper;
        private CorDebugStepReason m_stepReason;

        [CLSCompliant(false)]
        public CorStepCompleteEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugStepper stepper, CorDebugStepReason stepReason)
            : base(appDomain, thread)
        {
            m_stepper = stepper;
            m_stepReason = stepReason;
        }

        [CLSCompliant(false)]
        public CorStepCompleteEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugStepper stepper, CorDebugStepReason stepReason,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            m_stepper = stepper;
            m_stepReason = stepReason;
        }

        public ICorDebugStepper Stepper
        {
            get
            {
                return m_stepper;
            }
        }

        [CLSCompliant(false)]
        public CorDebugStepReason StepReason
        {
            get
            {
                return m_stepReason;
            }
        }

        public override string ToString()
        {
            if (CallbackType == ManagedCallbackType.OnStepComplete)
            {
                return "Step Complete";
            }
            return base.ToString();
        }
    }
}