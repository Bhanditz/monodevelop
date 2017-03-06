using System;
using System.Diagnostics;

using CorApi.ComInterop;

namespace CorApi2.debug
{
    abstract unsafe public unsafe class ManagedCallbackBase : ICorDebugManagedCallback, ICorDebugManagedCallback2
    {
        // Derived class overrides this methdos 
        protected abstract void HandleEvent(ManagedCallbackType eventId, CorEventArgs args);

        int ICorDebugManagedCallback.Breakpoint(CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugBreakpoint breakpoint)
        {
            HandleEvent(ManagedCallbackType.OnBreakpoint,
                new CorBreakpointEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    breakpoint == null ? null : new ICorDebugFunctionBreakpoint((CorApi.ComInterop.ICorDebugFunctionBreakpoint)breakpoint),
                    ManagedCallbackType.OnBreakpoint
                ));
        }

        void ICorDebugManagedCallback.StepComplete(CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugStepper stepper,
            CorDebugStepReason stepReason)
        {
            HandleEvent(ManagedCallbackType.OnStepComplete,
                new CorStepCompleteEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    stepper,
                    stepReason,
                    ManagedCallbackType.OnStepComplete));
        }

        void ICorDebugManagedCallback.Break(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnBreak,
                new CorThreadEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    ManagedCallbackType.OnBreak));
        }

        void ICorDebugManagedCallback.Exception(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            int unhandled)
        {
            HandleEvent(ManagedCallbackType.OnException,
                new CorExceptionEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    !(unhandled == 0),
                    ManagedCallbackType.OnException));
        }
        /* pass false if ``unhandled'' is 0 -- mapping TRUE to true, etc. */

        void ICorDebugManagedCallback.EvalComplete(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugEval eval)
        {
            HandleEvent(ManagedCallbackType.OnEvalComplete,
                new CorEvalEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    eval == null ? null : new ICorDebugEval(eval),
                    ManagedCallbackType.OnEvalComplete));
        }

        void ICorDebugManagedCallback.EvalException(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugEval eval)
        {
            HandleEvent(ManagedCallbackType.OnEvalException,
                new CorEvalEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    eval == null ? null : new ICorDebugEval(eval),
                    ManagedCallbackType.OnEvalException));
        }

        void ICorDebugManagedCallback.CreateProcess(
            ICorDebugProcess process)
        {
            HandleEvent(ManagedCallbackType.OnCreateProcess,
                new CorProcessEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                    ManagedCallbackType.OnCreateProcess));
        }

        void ICorDebugManagedCallback.ExitProcess(
            ICorDebugProcess process)
        {
            HandleEvent(ManagedCallbackType.OnProcessExit,
                new CorProcessEventArgs(process == null ? null : CorProcess.GetCorProcess(process),
                    ManagedCallbackType.OnProcessExit) { Continue = false });
        }

        void ICorDebugManagedCallback.CreateThread(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnCreateThread,
                new CorThreadEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    ManagedCallbackType.OnCreateThread));
        }

        void ICorDebugManagedCallback.ExitThread(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnThreadExit,
                new CorThreadEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    ManagedCallbackType.OnThreadExit));
        }

        void ICorDebugManagedCallback.LoadModule(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugModule managedModule)
        {
            HandleEvent(ManagedCallbackType.OnModuleLoad,
                new CorModuleEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    managedModule == null ? null : new ICorDebugModule(managedModule),
                    ManagedCallbackType.OnModuleLoad));
        }

        void ICorDebugManagedCallback.UnloadModule(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugModule managedModule)
        {
            HandleEvent(ManagedCallbackType.OnModuleUnload,
                new CorModuleEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    managedModule == null ? null : new ICorDebugModule(managedModule),
                    ManagedCallbackType.OnModuleUnload));
        }

        void ICorDebugManagedCallback.LoadClass(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugClass c)
        {
            HandleEvent(ManagedCallbackType.OnClassLoad,
                new CorClassEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    c == null ? null : new ICorDebugClass(c),
                    ManagedCallbackType.OnClassLoad));
        }

        void ICorDebugManagedCallback.UnloadClass(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugClass c)
        {
            HandleEvent(ManagedCallbackType.OnClassUnload,
                new CorClassEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    c == null ? null : new ICorDebugClass(c),
                    ManagedCallbackType.OnClassUnload));
        }

        void ICorDebugManagedCallback.DebuggerError(
            ICorDebugProcess process,
            int errorHR,
            uint errorCode)
        {
            HandleEvent(ManagedCallbackType.OnDebuggerError,
                new CorDebuggerErrorEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                    errorHR,
                    (int)errorCode,
                    ManagedCallbackType.OnDebuggerError));
        }

        void ICorDebugManagedCallback.LogMessage(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            int level,
            UInt16* pLogSwitchName,
            UInt16* pMessage)
        {
            HandleEvent(ManagedCallbackType.OnLogMessage,
                new CorLogMessageEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    level,
                    pLogSwitchName == null ? null : new string((char*) pLogSwitchName),
                    pMessage == null ? null : new string ((char*) pMessage),
                    ManagedCallbackType.OnLogMessage));
        }

        void ICorDebugManagedCallback.LogSwitch(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            int level,
            uint reason,
            UInt16* pLogSwitchName,
            UInt16* pParentName)
        {
            HandleEvent(ManagedCallbackType.OnLogSwitch,
                new CorLogSwitchEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    level, (int)reason,
                    pLogSwitchName == null ? null : new string ((char*) pLogSwitchName),
                    pParentName == null ? null : new string ((char*) pParentName),
                    ManagedCallbackType.OnLogSwitch));
        }

        void ICorDebugManagedCallback.CreateAppDomain(
            ICorDebugProcess process,
            CorApi.ComInterop.ICorDebugAppDomain appDomain)
        {
            HandleEvent(ManagedCallbackType.OnCreateAppDomain,
                new CorAppDomainEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                    appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    ManagedCallbackType.OnCreateAppDomain));
        }

        void ICorDebugManagedCallback.ExitAppDomain(
            ICorDebugProcess process,
            CorApi.ComInterop.ICorDebugAppDomain appDomain)
        {
            HandleEvent(ManagedCallbackType.OnAppDomainExit,
                new CorAppDomainEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                    appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    ManagedCallbackType.OnAppDomainExit));
        }

        void ICorDebugManagedCallback.LoadAssembly(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugAssembly assembly)
        {
            HandleEvent(ManagedCallbackType.OnAssemblyLoad,
                new CorAssemblyEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    assembly == null ? null : new ICorDebugAssembly(assembly),
                    ManagedCallbackType.OnAssemblyLoad));
        }

        void ICorDebugManagedCallback.UnloadAssembly(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugAssembly assembly)
        {
            HandleEvent(ManagedCallbackType.OnAssemblyUnload,
                new CorAssemblyEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    assembly == null ? null : new ICorDebugAssembly(assembly),
                    ManagedCallbackType.OnAssemblyUnload));
        }

        void ICorDebugManagedCallback.ControlCTrap(ICorDebugProcess process)
        {
            HandleEvent(ManagedCallbackType.OnControlCTrap,
                new CorProcessEventArgs( process == null ? null : CorProcess.GetCorProcess(process),
                    ManagedCallbackType.OnControlCTrap));
        }

        void ICorDebugManagedCallback.NameChange(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread)
        {
            HandleEvent(ManagedCallbackType.OnNameChange,
                new CorThreadEventArgs( appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    ManagedCallbackType.OnNameChange));
        }

        
        void ICorDebugManagedCallback.UpdateModuleSymbols(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugModule managedModule,
            IStream stream)
        {
            HandleEvent(ManagedCallbackType.OnUpdateModuleSymbols,
                new CorUpdateModuleSymbolsEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    managedModule == null ? null : new ICorDebugModule(managedModule),
                    stream,
                    ManagedCallbackType.OnUpdateModuleSymbols));
        }

        void ICorDebugManagedCallback.EditAndContinueRemap(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugFunction managedFunction,
            int isAccurate)
        {
            Debug.Assert(false); //OBSOLETE callback
        }


        void ICorDebugManagedCallback.BreakpointSetError(
            CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugBreakpoint breakpoint,
            UInt32 errorCode)
        {
            HandleEvent(ManagedCallbackType.OnBreakpointSetError,
                new CorBreakpointSetErrorEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    null, 
                    (int)errorCode,
                    ManagedCallbackType.OnBreakpointSetError));
        }

        void ICorDebugManagedCallback2.FunctionRemapOpportunity(CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugFunction oldFunction,
            CorApi.ComInterop.ICorDebugFunction newFunction,
            uint oldILoffset)
        {
            HandleEvent(ManagedCallbackType.OnFunctionRemapOpportunity,
                new CorFunctionRemapOpportunityEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    oldFunction == null ? null : new ICorDebugFunction(oldFunction),
                    newFunction == null ? null : new ICorDebugFunction(newFunction),
                    (int)oldILoffset,
                    ManagedCallbackType.OnFunctionRemapOpportunity));
        }

        void ICorDebugManagedCallback2.FunctionRemapComplete(CorApi.ComInterop.ICorDebugAppDomain appDomain,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugFunction managedFunction)
        {
            HandleEvent(ManagedCallbackType.OnFunctionRemapComplete,
                new CorFunctionRemapCompleteEventArgs(appDomain == null ? null : new ICorDebugAppDomain(appDomain),
                    thread == null ? null : new ICorDebugThread(thread),
                    managedFunction == null ? null : new ICorDebugFunction(managedFunction),
                    ManagedCallbackType.OnFunctionRemapComplete));
        }

        void ICorDebugManagedCallback2.CreateConnection(ICorDebugProcess process, uint connectionId, ushort* pConnName)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        void ICorDebugManagedCallback2.ChangeConnection(ICorDebugProcess process, uint connectionId)
        {
            //  Not Implemented
            Debug.Assert(false);
        }

        void ICorDebugManagedCallback2.DestroyConnection(ICorDebugProcess process, uint connectionId)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        void ICorDebugManagedCallback2.Exception(CorApi.ComInterop.ICorDebugAppDomain ad, CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugFrame frame, uint offset,
            CorDebugExceptionCallbackType eventType, uint flags) 
        {
            HandleEvent(ManagedCallbackType.OnException2,
                new CorException2EventArgs(ad == null ? null : new ICorDebugAppDomain(ad),
                    thread == null ? null : new ICorDebugThread(thread),
                    frame == null ? null : new ICorDebugFrame(frame),
                    (int)offset,
                    eventType,
                    (int)flags,
                    ManagedCallbackType.OnException2));
        }

        void ICorDebugManagedCallback2.ExceptionUnwind(CorApi.ComInterop.ICorDebugAppDomain ad, CorApi.ComInterop.ICorDebugThread thread,
            CorDebugExceptionUnwindCallbackType eventType, uint flags)
        {
            HandleEvent(ManagedCallbackType.OnExceptionUnwind2,
                new CorExceptionUnwind2EventArgs(ad == null ? null : new ICorDebugAppDomain(ad),
                    thread == null ? null : new ICorDebugThread(thread),
                    eventType,
                    (int)flags,
                    ManagedCallbackType.OnExceptionUnwind2));
        }

        // Get process from controller 
        static private CorProcess GetProcessFromController(CorApi.ComInterop.ICorDebugController pController)
        {
            CorProcess p;
            ICorDebugProcess p2 = pController as ICorDebugProcess;
            if (p2 != null)
            {
                p = CorProcess.GetCorProcess(p2);
            }
            else
            {
                CorApi.ComInterop.ICorDebugAppDomain a2 = (CorApi.ComInterop.ICorDebugAppDomain)pController;
                p = new ICorDebugAppDomain(a2).Process;
            }
            return p;
        }

        void ICorDebugManagedCallback2.MDANotification(CorApi.ComInterop.ICorDebugController pController,
            CorApi.ComInterop.ICorDebugThread thread,
            CorApi.ComInterop.ICorDebugMDA pMDA)
        {
            ICorDebugMDA c = new ICorDebugMDA(pMDA);
            string szName = c.Name;
            CorDebugMDAFlags f = c.Flags;
            CorProcess p = GetProcessFromController(pController);


            HandleEvent(ManagedCallbackType.OnMDANotification,
                new CorMDAEventArgs(c,
                    thread == null ? null : new ICorDebugThread(thread),
                    p, ManagedCallbackType.OnMDANotification));
        }


    }
}