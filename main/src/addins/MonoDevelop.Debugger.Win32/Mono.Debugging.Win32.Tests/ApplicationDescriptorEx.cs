using CorApi.Tests.Infra;
using System;
using Mono.Debugging.Client;

namespace Mono.Debugging.Win32.Tests
{
    internal static class ApplicationDescriptorEx
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

        public static string GetDbgShimPath(this ApplicationDescriptor app)
        {
            return @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.0.1\dbgshim.dll";
        }
    }
}