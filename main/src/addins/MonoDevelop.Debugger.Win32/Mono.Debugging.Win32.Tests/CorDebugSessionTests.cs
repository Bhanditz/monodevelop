using System;
using System.IO;
using Should;
using Xunit;

namespace Mono.Debugging.Win32.Tests
{
    public class CorDebugSessionTests
    {
        [Fact]
        public void ShouldCorrectlyStopOnDebuggerBreak()
        {
            Constants.Net45ConsoleApp.ExecuteWhenDebuggerBreakHitted(session =>
            {
                session.IsConnected.ShouldBeTrue();
                session.IsRunning.ShouldBeFalse();
            }, TimeSpan.FromSeconds(120));
        }

        [Fact]
        public void ShouldCorrectlyStopOnCutomBreakPoint()
        {
            Constants.Net45ConsoleApp.ExecuteWhenDebuggerBreakHitted(session =>
            {
                var corDebugSession = session as CorDebuggerSession;

                corDebugSession.ShouldNotBeNull();
                var docFiles = corDebugSession.GetAllDocumentPaths();
                foreach (var docFile in docFiles)
                {
                    if (Path.GetFileName(docFile) != "Program.cs")
                        continue;
                    session.Breakpoints.Add(docFile, 14);
                    break;
                }
                session.Continue();

            }, TimeSpan.FromSeconds(120));
        }

    }
}