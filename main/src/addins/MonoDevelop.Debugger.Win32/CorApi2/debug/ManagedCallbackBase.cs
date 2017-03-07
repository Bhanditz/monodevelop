using System.Diagnostics;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    public abstract unsafe class ManagedCallbackBase : ICorDebugManagedCallback, ICorDebugManagedCallback2
    {
        int ICorDebugManagedCallback.Break(ICorDebugAppDomain appDomain, ICorDebugThread thread)
        {
            return HandleEvent(ManagedCallbackType.OnBreak, new CorThreadEventArgs(appDomain, thread, ManagedCallbackType.OnBreak));
        }

        int ICorDebugManagedCallback.Breakpoint(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugBreakpoint breakpoint)
        {
            return HandleEvent(ManagedCallbackType.OnBreakpoint, new CorBreakpointEventArgs(appDomain, thread, breakpoint, ManagedCallbackType.OnBreakpoint));
        }

        int ICorDebugManagedCallback.BreakpointSetError(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugBreakpoint breakpoint, uint errorCode)
        {
            return HandleEvent(ManagedCallbackType.OnBreakpointSetError, new CorBreakpointSetErrorEventArgs(appDomain, thread, null, (int)errorCode, ManagedCallbackType.OnBreakpointSetError));
        }

        int ICorDebugManagedCallback2.ChangeConnection(ICorDebugProcess process, uint connectionId)
        {
            //  Not Implemented
            Debug.Assert(false);
        }

        int ICorDebugManagedCallback.ControlCTrap(ICorDebugProcess process)
        {
            return HandleEvent(ManagedCallbackType.OnControlCTrap, new CorProcessEventArgs(process, ManagedCallbackType.OnControlCTrap));
        }

        int ICorDebugManagedCallback.CreateAppDomain(ICorDebugProcess process, ICorDebugAppDomain appDomain)
        {
            return HandleEvent(ManagedCallbackType.OnCreateAppDomain, new CorAppDomainEventArgs(process, appDomain, ManagedCallbackType.OnCreateAppDomain));
        }

        int ICorDebugManagedCallback2.CreateConnection(ICorDebugProcess process, uint connectionId, ushort* pConnName)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        int ICorDebugManagedCallback.CreateProcess(ICorDebugProcess process)
        {
            return HandleEvent(ManagedCallbackType.OnCreateProcess, new CorProcessEventArgs(process, ManagedCallbackType.OnCreateProcess));
        }

        int ICorDebugManagedCallback.CreateThread(ICorDebugAppDomain appDomain, ICorDebugThread thread)
        {
            return HandleEvent(ManagedCallbackType.OnCreateThread, new CorThreadEventArgs(appDomain, thread, ManagedCallbackType.OnCreateThread));
        }

        int ICorDebugManagedCallback.DebuggerError(ICorDebugProcess process, int errorHR, uint errorCode)
        {
            return HandleEvent(ManagedCallbackType.OnDebuggerError, new CorDebuggerErrorEventArgs(process, errorHR, (int)errorCode, ManagedCallbackType.OnDebuggerError));
        }

        int ICorDebugManagedCallback2.DestroyConnection(ICorDebugProcess process, uint connectionId)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        int ICorDebugManagedCallback.EditAndContinueRemap(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugFunction managedFunction, int isAccurate)
        {
            Debug.Assert(false); //OBSOLETE callback
        }
        /* pass false if ``unhandled'' is 0 -- mapping TRUE to true, etc. */

        int ICorDebugManagedCallback.EvalComplete(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugEval eval)
        {
            return HandleEvent(ManagedCallbackType.OnEvalComplete, new CorEvalEventArgs(appDomain, thread, eval, ManagedCallbackType.OnEvalComplete));
        }

        int ICorDebugManagedCallback.EvalException(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugEval eval)
        {
            return HandleEvent(ManagedCallbackType.OnEvalException, new CorEvalEventArgs(appDomain, thread, eval, ManagedCallbackType.OnEvalException));
        }

        int ICorDebugManagedCallback.Exception(ICorDebugAppDomain appDomain, ICorDebugThread thread, int unhandled)
        {
            return HandleEvent(ManagedCallbackType.OnException, new CorExceptionEventArgs(appDomain, thread, !(unhandled == 0), ManagedCallbackType.OnException));
        }

        int ICorDebugManagedCallback2.Exception(ICorDebugAppDomain ad, ICorDebugThread thread, ICorDebugFrame frame, uint offset, CorDebugExceptionCallbackType eventType, uint flags)
        {
            return HandleEvent(ManagedCallbackType.OnException2, new CorException2EventArgs(ad, thread, frame, (int)offset, eventType, (int)flags, ManagedCallbackType.OnException2));
        }

        int ICorDebugManagedCallback2.ExceptionUnwind(ICorDebugAppDomain ad, ICorDebugThread thread, CorDebugExceptionUnwindCallbackType eventType, uint flags)
        {
            return HandleEvent(ManagedCallbackType.OnExceptionUnwind2, new CorExceptionUnwind2EventArgs(ad, thread, eventType, (int)flags, ManagedCallbackType.OnExceptionUnwind2));
        }

        int ICorDebugManagedCallback.ExitAppDomain(ICorDebugProcess process, ICorDebugAppDomain appDomain)
        {
            return HandleEvent(ManagedCallbackType.OnAppDomainExit, new CorAppDomainEventArgs(process, appDomain, ManagedCallbackType.OnAppDomainExit));
        }

        int ICorDebugManagedCallback.ExitProcess(ICorDebugProcess process)
        {
            return HandleEvent(ManagedCallbackType.OnProcessExit, new CorProcessEventArgs(process, ManagedCallbackType.OnProcessExit) {Continue = false});
        }

        int ICorDebugManagedCallback.ExitThread(ICorDebugAppDomain appDomain, ICorDebugThread thread)
        {
            return HandleEvent(ManagedCallbackType.OnThreadExit, new CorThreadEventArgs(appDomain, thread, ManagedCallbackType.OnThreadExit));
        }

        int ICorDebugManagedCallback2.FunctionRemapComplete(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugFunction managedFunction)
        {
            return HandleEvent(ManagedCallbackType.OnFunctionRemapComplete, new CorFunctionRemapCompleteEventArgs(appDomain, thread, managedFunction, ManagedCallbackType.OnFunctionRemapComplete));
        }

        int ICorDebugManagedCallback2.FunctionRemapOpportunity(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugFunction oldFunction, ICorDebugFunction newFunction, uint oldILoffset)
        {
            return HandleEvent(ManagedCallbackType.OnFunctionRemapOpportunity, new CorFunctionRemapOpportunityEventArgs(appDomain, thread, oldFunction, newFunction, (int)oldILoffset, ManagedCallbackType.OnFunctionRemapOpportunity));
        }

        // Get process from controller 
        private static CorProcess GetProcessFromController(ICorDebugController pController)
        {
            CorProcess p;
            var p2 = pController as ICorDebugProcess;
            if(p2 != null)
                p = CorProcess.GetCorProcess(p2);
            else
            {
                var a2 = (ICorDebugAppDomain)pController;
                p = new ICorDebugAppDomain(a2).Process;
            }
            return p;
        }

        // Derived class overrides this methdos 
        protected abstract int HandleEvent(ManagedCallbackType eventId, CorEventArgs args);

        int ICorDebugManagedCallback.LoadAssembly(ICorDebugAppDomain appDomain, ICorDebugAssembly assembly)
        {
            return HandleEvent(ManagedCallbackType.OnAssemblyLoad, new CorAssemblyEventArgs(appDomain, assembly, ManagedCallbackType.OnAssemblyLoad));
        }

        int ICorDebugManagedCallback.LoadClass(ICorDebugAppDomain appDomain, ICorDebugClass c)
        {
            return HandleEvent(ManagedCallbackType.OnClassLoad, new CorClassEventArgs(appDomain, c, ManagedCallbackType.OnClassLoad));
        }

        int ICorDebugManagedCallback.LoadModule(ICorDebugAppDomain appDomain, ICorDebugModule managedModule)
        {
            return HandleEvent(ManagedCallbackType.OnModuleLoad, new CorModuleEventArgs(appDomain, managedModule, ManagedCallbackType.OnModuleLoad));
        }

        int ICorDebugManagedCallback.LogMessage(ICorDebugAppDomain appDomain, ICorDebugThread thread, int level, ushort* pLogSwitchName, ushort* pMessage)
        {
            return HandleEvent(ManagedCallbackType.OnLogMessage, new CorLogMessageEventArgs(appDomain, thread, level, pLogSwitchName == null ? null : new string((char*)pLogSwitchName), pMessage == null ? null : new string((char*)pMessage), ManagedCallbackType.OnLogMessage));
        }

        int ICorDebugManagedCallback.LogSwitch(ICorDebugAppDomain appDomain, ICorDebugThread thread, int level, uint reason, ushort* pLogSwitchName, ushort* pParentName)
        {
            return HandleEvent(ManagedCallbackType.OnLogSwitch, new CorLogSwitchEventArgs(appDomain, thread, level, (int)reason, pLogSwitchName == null ? null : new string((char*)pLogSwitchName), pParentName == null ? null : new string((char*)pParentName), ManagedCallbackType.OnLogSwitch));
        }

        int ICorDebugManagedCallback2.MDANotification(ICorDebugController pController, ICorDebugThread thread, ICorDebugMDA pMDA)
        {
            CorProcess p = GetProcessFromController(pController);

            return HandleEvent(ManagedCallbackType.OnMDANotification, new CorMDAEventArgs(pMDA, thread, p, ManagedCallbackType.OnMDANotification));
        }

        int ICorDebugManagedCallback.NameChange(ICorDebugAppDomain appDomain, ICorDebugThread thread)
        {
            return HandleEvent(ManagedCallbackType.OnNameChange, new CorThreadEventArgs(appDomain, thread, ManagedCallbackType.OnNameChange));
        }

        int ICorDebugManagedCallback.StepComplete(ICorDebugAppDomain appDomain, ICorDebugThread thread, ICorDebugStepper stepper, CorDebugStepReason stepReason)
        {
            return HandleEvent(ManagedCallbackType.OnStepComplete, new CorStepCompleteEventArgs(appDomain, thread, stepper, stepReason, ManagedCallbackType.OnStepComplete));
        }

        int ICorDebugManagedCallback.UnloadAssembly(ICorDebugAppDomain appDomain, ICorDebugAssembly assembly)
        {
            return HandleEvent(ManagedCallbackType.OnAssemblyUnload, new CorAssemblyEventArgs(appDomain, assembly, ManagedCallbackType.OnAssemblyUnload));
        }

        int ICorDebugManagedCallback.UnloadClass(ICorDebugAppDomain appDomain, ICorDebugClass c)
        {
            return HandleEvent(ManagedCallbackType.OnClassUnload, new CorClassEventArgs(appDomain, c, ManagedCallbackType.OnClassUnload));
        }

        int ICorDebugManagedCallback.UnloadModule(ICorDebugAppDomain appDomain, ICorDebugModule managedModule)
        {
            return HandleEvent(ManagedCallbackType.OnModuleUnload, new CorModuleEventArgs(appDomain, managedModule, ManagedCallbackType.OnModuleUnload));
        }

        int ICorDebugManagedCallback.UpdateModuleSymbols(ICorDebugAppDomain appDomain, ICorDebugModule managedModule, IStream stream)
        {
            return HandleEvent(ManagedCallbackType.OnUpdateModuleSymbols, new CorUpdateModuleSymbolsEventArgs(appDomain, managedModule, stream, ManagedCallbackType.OnUpdateModuleSymbols));
        }
    }
}