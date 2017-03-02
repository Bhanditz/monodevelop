using CorApi.Tests.Infra;
using System;
using PinvokeKit;

namespace Mono.Debugging.Win32.Tests
{
    internal static class CorClrApplicationDescriptorEx
    {
        private static readonly string DotNetCoreRootPath = @"C:/Program Files/dotnet/";
        private static readonly string PlatformVersion = "1.0.3";

        public static string GetCommandlineForDebuggerBreak(this ApplicationDescriptor app)
        {
            return string.Format("\"{0}{1}\" \"{2}\" BREAK", DotNetCoreRootPath, DotNetCliExeName, app.BinaryPath);
        }

        public static string GetCommandlineForSleep(this ApplicationDescriptor app, TimeSpan sleepTime)
        {
            return string.Format("\"{0}{1}\" \"{2}\" SLEEP {3}", DotNetCoreRootPath, DotNetCliExeName,
                app.BinaryPath, sleepTime.TotalMilliseconds);
        }

        private static readonly string DotNetCliExeName = PlatformUtil.IsRunningUnderWindows ? "dotnet.exe" : "dotnet";

        public static string GetDbgShimPath(this ApplicationDescriptor app)
        {
            return string.Format(@"{0}/shared/Microsoft.NETCore.App/{1}/{2}", DotNetCoreRootPath, PlatformVersion,
                PlatformUtil.IsRunningUnderWindows ? "dbgshim.dll" : "libdbgshim.dll");
        }
    }
}