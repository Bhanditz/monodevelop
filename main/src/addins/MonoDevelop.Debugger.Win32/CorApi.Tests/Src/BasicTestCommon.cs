﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using CorApi.ComInterop;
using CorApi.Infra;

using NUnit.Framework;

using Should;

namespace CorApi.Tests
{
    public unsafe class BasicTestCommon
    {
        public static void TestProcess(ICorDebugProcess process)
        {
            var say = Tracepoints.New("DoChecksOnProcess");

            say();

            {
                say(1000);
                ICorDebugAppDomainEnum @enum;
                process.EnumerateAppDomains(out @enum).AssertSucceeded("Cannot enumerate appdomains.");
                say();
                List<ICorDebugAppDomain> listAppdomains1 = @enum.AsEnumerable().ToList();
                say();
                List<ICorDebugAppDomain> listAppdomains2 = @enum.AsEnumerable().ToList();
                say();

                listAppdomains1.Count.ShouldEqual(1);
                listAppdomains2.Count.ShouldEqual(1);
                ReferenceEquals(listAppdomains1.Single(), listAppdomains2.Single()).ShouldBeTrue("Different RCWs.");
                say();
            }

            {
                say(2000);
                IList<ICorDebugAppDomain> listAppdomains1 = process.GetAppDomains();
                say();
                IList<ICorDebugAppDomain> listAppdomains2 = process.GetAppDomains();
                say();

                listAppdomains1.Count.ShouldEqual(1);
                listAppdomains2.Count.ShouldEqual(1);
                ReferenceEquals(listAppdomains1.Single(), listAppdomains2.Single()).ShouldBeTrue("Different RCWs.");
                say();
                TestAppdomain(listAppdomains1.Single());
                say();
                
            }

            IList<ICorDebugThread> threads = process.GetThreads();
            threads.Count.ShouldBeGreaterThan(0);
            foreach(ICorDebugThread thread in threads)
            {
                TestThread(thread);
            }

            say(10000);
            Console.Error.WriteLine("EAD -1");
        }

        private static void TestThread(ICorDebugThread thread)
        {
            uint tid;
            thread.GetID(&tid).AssertSucceeded("Could not get the Thread ID.");
            Console.Error.WriteLine("Got thread {0:N0}.", tid);
        }

        private static void TestAppdomain(ICorDebugAppDomain appdomain)
        {
            Console.Error.WriteLine($"Got appdomain {appdomain.GetName()} with {appdomain.GetThreads().Count:N0} threads and {appdomain.GetAssemblies().Count:N0} assemblies.");
        }

        public static void CheckBaseCorDbg(ICorDebug cordbg, Func<ICorDebug, ICorDebugProcess> onCreateProcess)
        {
            Console.Error.WriteLine(2);
            cordbg.Initialize().AssertSucceeded("Could not init the debugger.");
            Console.Error.WriteLine(3);
            var callback = new CorDbgCallback();
            Console.Error.WriteLine(4);
            cordbg.SetManagedHandler(callback).AssertSucceeded("Could not set the managed handler.");
            //cordbg.SetUnmanagedHandler(callback);
            Console.Error.WriteLine(5);

            bool isDone = false;
            callback.OnProcess += ppc =>
            {
                try
                {
                    TestProcess(ppc);

//                    MessageBox.Show("RUnning.");
                    Console.Error.WriteLine("CB 0");
                    // Not running yet
                    int run;
                    ppc.IsRunning(&run).AssertSucceeded("IsRunning");
                    run.ShouldEqual(0);
                    Console.Error.WriteLine("CB 1");

                    ICorDebugAppDomainEnum ads;
                    ppc.EnumerateAppDomains(out ads);
                    Console.Error.WriteLine("CB 2");
                    uint num = 0;
                    ads.GetCount(&num).AssertSucceeded("Can't get count.");
                    Console.Error.WriteLine("CB 3");

                    // Resume
                    Console.Error.WriteLine("CB 4");
                    ppc.Continue(0).AssertSucceeded("Could not continue.");

                    Console.Error.WriteLine("CB 5");
                    // Should be running
                    //                        ppc.IsRunning(&run);
                    //                        run.ShouldNotEqual(0);
                }
                finally
                {
                    Console.Error.WriteLine("CB 6");
                    Volatile.Write(ref isDone, true);
                    Console.Error.WriteLine("CB 7");
                }
            };

            Console.Error.WriteLine(6);
            ICorDebugProcess process = onCreateProcess(cordbg);

            Console.Error.WriteLine(7);

            if(process != null)
            {
                // Commented out. The process cannot continue because it is not stopped here, and with debug builds of coreclr this triggers an ASSERT
/*
                int hrCont = process.Continue(0);
                Console.Error.WriteLine(7.5);
                try
                {
                    hrCont.AssertSucceeded("Continue.");
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine("Expected exception: {0}.", ex.Message);
                    //                        throw;
                }
*/
                Console.Error.WriteLine(8);

                {
                    ICorDebugAppDomainEnum ads;
                    process.EnumerateAppDomains(out ads);
                    Console.Error.WriteLine(9);
                    uint num = 0;
                    ads.GetCount(&num).AssertSucceeded("Could not get the appdomains count.");
                    Console.Error.WriteLine(10);
                }
                Console.Error.WriteLine("Start waiting.");
                while(!Volatile.Read(ref isDone))
                {
                    Console.Error.WriteLine("Waiting…");
                    Thread.Sleep(0x100);
                }
                Console.Error.WriteLine("Done waiting.");

                TestProcess(process);

                process.Stop(0).AssertSucceeded("process.Stop(0)");
                process.Terminate(42).AssertSucceeded("process.Terminate(42)");
                //                cordbg.Terminate();
                /*
                          session = new CorDebuggerSession(new char[] { });
                          var sessionStartGuard = new ManualResetEvent(false);
                          session.TargetInterrupted += (s1, a1) =>
                          {
                            var corDebugSession = s1 as CorDebuggerSession;
                            corDebugSession.ShouldNotBeNull();
                            corDebugSession.IsConnected.ShouldBeTrue();
                            corDebugSession.IsRunning.ShouldBeFalse();
                            sessionStartGuard.Set();
                          };

                          session.Run(app.GetStartInfoForDebuggerBreak(), new DebuggerSessionOptions());
                          sessionStartGuard.WaitOne(TimeSpan.FromSeconds(10)).ShouldBeTrue("Session wasn't start");
                          session.Stop();
                */
            }
            if(!callback.Exceptions.IsEmpty)
                throw new AggregateException(callback.Exceptions);
        }

        [ComVisible(true)]
        [Guid("2C08770A-A39C-4767-8309-F03FB160BB98")]
        public class CorDbgCallback : ICorDebugManagedCallback, ICorDebugManagedCallback2
        {
            public readonly ConcurrentBag<Exception> Exceptions = new ConcurrentBag<Exception>();

            /// <inheritdoc />
            int ICorDebugManagedCallback.Break(ICorDebugAppDomain pAppDomain, ICorDebugThread thread)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.Breakpoint(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugBreakpoint pBreakpoint)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.BreakpointSetError(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugBreakpoint pBreakpoint, uint dwError)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.ChangeConnection(ICorDebugProcess pProcess, uint dwConnectionId)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.ControlCTrap(ICorDebugProcess pProcess)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.CreateAppDomain(ICorDebugProcess pProcess, ICorDebugAppDomain pAppDomain)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.CreateConnection(ICorDebugProcess pProcess, uint dwConnectionId, ushort* pConnName)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.CreateProcess(ICorDebugProcess pProcess)
            {
                try
                {
                    Console.Error.WriteLine("CP!");
                    OnProcess(pProcess);
                }
                catch(Exception ex)
                {
                    Exceptions.Add(ex);
                }
                return (int)HResults.S_OK;
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.CreateThread(ICorDebugAppDomain pAppDomain, ICorDebugThread thread)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.DebuggerError(ICorDebugProcess pProcess, int errorHR, uint errorCode)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.DestroyConnection(ICorDebugProcess pProcess, uint dwConnectionId)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.EditAndContinueRemap(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pFunction, int fAccurate)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.EvalComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval pEval)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.EvalException(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval pEval)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.Exception(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int unhandled)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.Exception(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFrame pFrame, uint nOffset, CorDebugExceptionCallbackType dwEventType, uint dwFlags)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.ExceptionUnwind(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, CorDebugExceptionUnwindCallbackType dwEventType, uint dwFlags)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.ExitAppDomain(ICorDebugProcess pProcess, ICorDebugAppDomain pAppDomain)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.ExitProcess(ICorDebugProcess pProcess)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.ExitThread(ICorDebugAppDomain pAppDomain, ICorDebugThread thread)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.FunctionRemapComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pFunction)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.FunctionRemapOpportunity(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pOldFunction, ICorDebugFunction pNewFunction, uint oldILOffset)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.LoadAssembly(ICorDebugAppDomain pAppDomain, ICorDebugAssembly pAssembly)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.LoadClass(ICorDebugAppDomain pAppDomain, ICorDebugClass c)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.LoadModule(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.LogMessage(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int lLevel, ushort* pLogSwitchName, ushort* pMessage)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.LogSwitch(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int lLevel, uint ulReason, ushort* pLogSwitchName, ushort* pParentName)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback2.MDANotification(ICorDebugController pController, ICorDebugThread pThread, ICorDebugMDA pMDA)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.NameChange(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread)
            {
                return 0; // TODO_IMPL();
            }

            public event Action<ICorDebugProcess> OnProcess = o => { };

            /// <inheritdoc />
            int ICorDebugManagedCallback.StepComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugStepper pStepper, CorDebugStepReason reason)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.UnloadAssembly(ICorDebugAppDomain pAppDomain, ICorDebugAssembly pAssembly)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.UnloadClass(ICorDebugAppDomain pAppDomain, ICorDebugClass c)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.UnloadModule(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule)
            {
                return 0; // TODO_IMPL();
            }

            /// <inheritdoc />
            int ICorDebugManagedCallback.UpdateModuleSymbols(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule, IStream pSymbolStream)
            {
                return 0; // TODO_IMPL();
            }
        }
    }
}