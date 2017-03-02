using CorApi.Tests.Infra;
using System;
using Mono.Debugging.Client;

namespace Mono.Debugging.Win32.Tests
{
    internal static class CorDebugApplicationDescriptorEx
    {
        public static DebuggerStartInfo GetStartInfoForDebuggerBreak(this ApplicationDescriptor app)
        {
            return new DebuggerStartInfo
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
        }

        public static DebuggerStartInfo GetStartInfoForSleep(this ApplicationDescriptor app, TimeSpan sleepTime)
        {
            return new DebuggerStartInfo
            {
                Arguments = string.Format("SLEEP {0}", sleepTime.TotalMilliseconds),
                CloseExternalConsoleOnExit = true,
                Command = app.BinaryPath,
                RequiresManualStart = false,
                RuntimeArguments = "",
                UseExternalConsole = true,
                WorkingDirectory = app
                    .WorkingDirectory
            };
        }
    }
}