using System;
using System.Threading;
using Mono.Debugging.Client;
using Should;

namespace Mono.Debugging.Win32.Tests
{
    internal static class ApplicationDescriptorEx
    {
        public static void ExecuteWhenDebuggerBreakHitted(this ApplicationDescriptor app, Action<DebuggerSession> action, TimeSpan timeout)
        {
            CorDebuggerSession session = null;
            try
            {
                session = new CorDebuggerSession(new char[] { });

                var startInfo = new DebuggerStartInfo
                {
                    Arguments = "BREAK",
                    CloseExternalConsoleOnExit = true,
                    Command = app.BinaryPath,
                    RequiresManualStart = false,
                    RuntimeArguments = "",
                    UseExternalConsole = true,
                    WorkingDirectory = app
                        .WorkingDirectory
                };

                var timeGuard = new ManualResetEvent(false);

                session.TargetInterrupted += new EventHandler<TargetEventArgs>((sender, args) =>
                {
                    action(session);
                    timeGuard.Set();
                });

                session.Run(startInfo, new DebuggerSessionOptions());
                timeGuard.WaitOne(timeout).ShouldBeTrue("Stoped event not happend");
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