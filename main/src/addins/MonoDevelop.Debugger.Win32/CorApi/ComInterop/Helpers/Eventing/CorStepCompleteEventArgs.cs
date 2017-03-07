using System;

namespace CorApi.ComInterop.Eventing
{
    public unsafe class CorStepCompleteEventArgs : CorThreadEventArgs
    {
        [CLSCompliant(false)]
        public CorStepCompleteEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugStepper stepper, CorDebugStepReason stepReason)
            : base(appDomain, thread)
        {
            Stepper = stepper;
            StepReason = stepReason;
        }

        [CLSCompliant(false)]
        public CorStepCompleteEventArgs(ICorDebugAppDomain appDomain, ICorDebugThread thread,
            ICorDebugStepper stepper, CorDebugStepReason stepReason,
            ManagedCallbackType callbackType)
            : base(appDomain, thread, callbackType)
        {
            Stepper = stepper;
            StepReason = stepReason;
        }

        public ICorDebugStepper Stepper { get; }

        [CLSCompliant(false)]
        public CorDebugStepReason StepReason { get; }

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