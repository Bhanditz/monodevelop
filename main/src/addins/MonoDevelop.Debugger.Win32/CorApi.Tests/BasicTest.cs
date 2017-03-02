using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

using CorApi.ComInterop;
using CorApi.Tests.Infra;

using NUnit.Framework;

using Should;

namespace CorApi.Tests
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe class BasicTest
    {
        private const string ShimLibraryName = "mscoree.dll";

        [DllImport(ShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern Int32 CreateDebuggingInterfaceFromVersion(Int32 iDebuggerVersion, UInt16* szDebuggeeVersion, void**ppCordb);

        [Test]
        public void Run()
        {
            Console.Error.WriteLine(0.0);
            //            MessageBox.Show("Stop!");
            ApplicationDescriptor app = Constants.Net45ConsoleApp;
            try
            {
                Console.Error.WriteLine(1.0);
                void* pCorDb;
                ICorDebug cordbg;
                using(Com.UsingReference(&pCorDb))
                {
                    Console.Error.WriteLine(1.1);
                    fixed(char* pchVer = "v4.0.30319")
                        CreateDebuggingInterfaceFromVersion(4, (ushort*)pchVer, &pCorDb).AssertSucceeded("Could not CreateDebuggingInterfaceFromVersion.");
                    Console.Error.WriteLine(1.2);
                    cordbg = Com.QueryInteface<ICorDebug>(pCorDb);
                    Console.Error.WriteLine(1.3);
                }
                Console.Error.WriteLine(2);
                cordbg.Initialize();
                Console.Error.WriteLine(3);
                var callback = new CorDbgCallback();
                Console.Error.WriteLine(4);
                cordbg.SetManagedHandler(callback);
                //cordbg.SetUnmanagedHandler(callback);
                Console.Error.WriteLine(5);
                ICorDebugProcess process;

                var si = new STARTUPINFOW();
                si.cb = (uint)Marshal.SizeOf(si);

                var pi = new PROCESS_INFORMATION();

                bool isDone = false;
                callback.OnProcess += ppc =>
                {
                    try
                    {
                        Console.Error.WriteLine("CB 0");
                        // Not running yet
                        int run;
                        ppc.IsRunning(&run);
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
                        ppc.Continue(0);

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
                fixed(char* pchPath = app.BinaryPath)
                fixed(char* pchCmdl = $"\"{app.BinaryPath}\" NO_BREAK")
                fixed(char* pchWorkdir = app.WorkingDirectory)
                    cordbg.CreateProcess((ushort*)pchPath, (ushort*)pchCmdl, null, null, 0, 0, null, (ushort*)pchWorkdir, &si, &pi, CorDebugCreateProcessFlags.DEBUG_NO_SPECIAL_OPTIONS, out process);

                Console.Error.WriteLine(7);

                int hrCont = process.Continue(0);
                Console.Error.WriteLine(7.5);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    try
                    {
                        hrCont.AssertSucceeded("Continue.");

                    }
                    catch(Exception ex)
                    {
                        Console.Error.WriteLine("Expected exception: {0}.", ex.Message);
                        throw;
                    }
                });
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
                    Thread.Sleep(0x10);
                }
                Console.Error.WriteLine("Done waiting.");

                process.Stop(0);
                process.Terminate(42);
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

                if(!callback.Exceptions.IsEmpty)
                    throw new AggregateException(callback.Exceptions);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                /*
                          if (session != null)
                          {
                            session.Dispose();
                            session = null;
                          }
                */
            }
        }

        [ComVisible(true)]
        [Guid("2C08770A-A39C-4767-8309-F03FB160BB98")]
        public class CorDbgCallback : ICorDebugManagedCallback, ICorDebugManagedCallback2
        {
            public readonly ConcurrentBag<Exception> Exceptions = new ConcurrentBag<Exception>();

            /// <inheritdoc />
            void ICorDebugManagedCallback.Break(ICorDebugAppDomain pAppDomain, ICorDebugThread thread)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.Breakpoint(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugBreakpoint pBreakpoint)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.BreakpointSetError(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugBreakpoint pBreakpoint, uint dwError)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.ChangeConnection(ICorDebugProcess pProcess, uint dwConnectionId)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.ControlCTrap(ICorDebugProcess pProcess)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.CreateAppDomain(ICorDebugProcess pProcess, ICorDebugAppDomain pAppDomain)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.CreateConnection(ICorDebugProcess pProcess, uint dwConnectionId, ushort* pConnName)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.CreateProcess(ICorDebugProcess pProcess)
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
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.CreateThread(ICorDebugAppDomain pAppDomain, ICorDebugThread thread)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.DebuggerError(ICorDebugProcess pProcess, int errorHR, uint errorCode)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.DestroyConnection(ICorDebugProcess pProcess, uint dwConnectionId)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.EditAndContinueRemap(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pFunction, int fAccurate)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.EvalComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval pEval)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.EvalException(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval pEval)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.Exception(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int unhandled)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.Exception(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFrame pFrame, uint nOffset, CorDebugExceptionCallbackType dwEventType, uint dwFlags)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.ExceptionUnwind(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, CorDebugExceptionUnwindCallbackType dwEventType, uint dwFlags)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.ExitAppDomain(ICorDebugProcess pProcess, ICorDebugAppDomain pAppDomain)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.ExitProcess(ICorDebugProcess pProcess)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.ExitThread(ICorDebugAppDomain pAppDomain, ICorDebugThread thread)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.FunctionRemapComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pFunction)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.FunctionRemapOpportunity(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pOldFunction, ICorDebugFunction pNewFunction, uint oldILOffset)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.LoadAssembly(ICorDebugAppDomain pAppDomain, ICorDebugAssembly pAssembly)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.LoadClass(ICorDebugAppDomain pAppDomain, ICorDebugClass c)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.LoadModule(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.LogMessage(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int lLevel, ushort* pLogSwitchName, ushort* pMessage)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.LogSwitch(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int lLevel, uint ulReason, ushort* pLogSwitchName, ushort* pParentName)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback2.MDANotification(ICorDebugController pController, ICorDebugThread pThread, ICorDebugMDA pMDA)
            {
                // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.NameChange(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread)
            {
                // // TODO_IMPLEMENT_ME();
            }

            public event Action<ICorDebugProcess> OnProcess = o => { };

            /// <inheritdoc />
            void ICorDebugManagedCallback.StepComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugStepper pStepper, CorDebugStepReason reason)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.UnloadAssembly(ICorDebugAppDomain pAppDomain, ICorDebugAssembly pAssembly)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.UnloadClass(ICorDebugAppDomain pAppDomain, ICorDebugClass c)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.UnloadModule(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule)
            {
                // // TODO_IMPLEMENT_ME();
            }

            /// <inheritdoc />
            void ICorDebugManagedCallback.UpdateModuleSymbols(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule, IStream pSymbolStream)
            {
                // // TODO_IMPLEMENT_ME();
            }
        }
    }
}