using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Samples.Debugging.CorDebug;
using Mono.Debugging.Client;
using Should;
using Xunit;

namespace Mono.Debugging.Win32.Tests
{
    public class CorDebugSessionTests
    {
        [Fact]
        public void ShouldCorrectlyEnemrateAppDoimains()
        {
            CorDebuggerSession session = null;
            var app = Constants.Net45ConsoleApp;
            try
            {
                session = new CorDebuggerSession(new char[] { });
                var sessionStartGuard = new ManualResetEvent(false);
                session.Run(app.GetStartInfoForSleep(TimeSpan.FromSeconds(20)), new DebuggerSessionOptions());

                Thread.Sleep(5000);

                List<CorAppDomain> appDomains = null;
                List<CorModule> modules = null;

                session.TargetStopped += (sender, args) =>
                {
                    var ss = sender as CorDebuggerSession;
                    ss.ShouldNotBeNull();
                    appDomains = ss.GetAppDomains().ToList();
                    modules = ss.GetAllModules().ToList();
                    sessionStartGuard.Set();
                };

                session.Stop();

                sessionStartGuard.WaitOne(TimeSpan.FromSeconds(60)).ShouldBeTrue("Session wasn't start");
                appDomains.Count.ShouldEqual(1);
                modules.Count.ShouldEqual(2);
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                    session = null;
                }
            }
        }

        [Fact]
        public void ShouldCorrectlyStopOnDebuggerBreak()
        {
            CorDebuggerSession session = null;
            var app = Constants.Net45ConsoleApp;
            try
            {
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
                session.StopAndWait();
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                    session = null;
                }
            }
        }

        [Fact]
        public void ShouldCorrectlyStopOnCutomBreakpoint()
        {
            CorDebuggerSession session = null;
            var app = Constants.Net45ConsoleApp;
            try
            {
                session = new CorDebuggerSession(new char[] { });
                var breakPointHittedGuard = new ManualResetEvent(false);
                session.TargetInterrupted += (s1, a1) =>
                {
                    var corDebugSession = s1 as CorDebuggerSession;
                    corDebugSession.ShouldNotBeNull();

                    var docFiles = corDebugSession.GetAllDocumentPaths();
                    foreach (var docFile in docFiles)
                    {
                        if (Path.GetFileName(docFile) != "Program.cs")
                            continue;
                        // TODO: Change it if you change tester sources
                        const int breakPointLine = 44;
                        corDebugSession.ToggleBreakpointAndWaitForBind(docFile, breakPointLine);
                        break;
                    }
                    corDebugSession.IsConnected.ShouldBeTrue();
                    corDebugSession.IsRunning.ShouldBeFalse();

                    corDebugSession.TargetHitBreakpoint += (s2, a2) =>
                    {
                        breakPointHittedGuard.Set();
                    };
                    corDebugSession.Continue();
                };

                session.Run(app.GetStartInfoForDebuggerBreak(), new DebuggerSessionOptions());
                breakPointHittedGuard.WaitOne(TimeSpan.FromSeconds(20)).ShouldBeTrue("Breakpoint wasn't hit");
                session.StopAndWait();
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                }
            }
        }
    }
}