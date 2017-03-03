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
                session.Run(app.GetStartInfoForSleep(TimeSpan.FromSeconds(5)), new DebuggerSessionOptions());

                // Wait when until Thread.Sleep started
                Thread.Sleep(5000);

                session.StopAndWait(TimeSpan.FromSeconds(5));

                var appDomains = session.GetAppDomains().ToList();
                var modules = session.GetAllModules().ToList();
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
                session.StopAndWait(TimeSpan.FromSeconds(10));
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
        public void ShouldCorrectlyStopOnCustomBreakpoint()
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
                        const string breakpointLineMessage = "//ShouldCorrectlyStopOnCustomBreakpoint: insert breakpoint here";
                        var breakpointLine = -1;
                        var currentLineNumber = 0;
                        foreach (var line in File.ReadLines(docFile)) {
                            currentLineNumber++;
                            if (line.Contains (breakpointLineMessage)) {
                                breakpointLine = currentLineNumber;
                                break;
                            }
                        }
                        breakpointLine.ShouldBeGreaterThan (0);
                        corDebugSession.ToggleBreakpointAndWaitForBind(docFile, breakpointLine, TimeSpan.FromSeconds(10));
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
                session.StopAndWait(TimeSpan.FromSeconds(10));
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