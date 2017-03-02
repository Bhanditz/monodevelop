using CorApi.Tests.Infra;

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
    }
}