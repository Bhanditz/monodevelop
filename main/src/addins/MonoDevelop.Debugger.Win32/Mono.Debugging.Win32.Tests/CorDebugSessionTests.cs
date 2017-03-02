using System;
using System.IO;
using System.Threading;

using CorApi.Tests.Infra;

using Mono.Debugging.Client;
using Should;
using Xunit;

namespace Mono.Debugging.Win32.Tests
{
    public class CorDebugSessionTests
    {
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
                session.Stop();
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
        public void ShouldCorrectlyStopOnCutomBreakPoint()
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
                        corDebugSession.Breakpoints.Add(docFile, 14);
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
                breakPointHittedGuard.WaitOne(TimeSpan.FromSeconds(10)).ShouldBeTrue("Breakpoint wasn't hit");
                session.Stop();
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