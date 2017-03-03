using System;
using System.IO;

using CorApi.Tests.Infra;

using PinvokeKit;

namespace CorApi.Tests
{
    internal static class CorClrApplicationDescriptorEx
    {
        private static readonly string DotNetCoreRootPath = PlatformUtil.IsRunningUnderWindows ? Path.Combine(Constants.TestDataFolder, "Tools", "NetCore", /*"v1.0.3_win"*/"v1.0.5_win_mfdebug")+"\\": @"/usr/share/dotnet/";
        private static readonly string PlatformVersion = "1.0.5";

        public static string GetCommandlineForDebuggerBreak(this ApplicationDescriptor app)
        {
            return $"\"{DotNetCoreRootPath}{DotNetCliExeName}\" \"{app.BinaryPath}\" BREAK";
        }

        public static string GetCommandlineForSleep(this ApplicationDescriptor app, TimeSpan sleepTime)
        {
            return $"\"{DotNetCoreRootPath}{DotNetCliExeName}\" \"{app.BinaryPath}\" SLEEP {sleepTime.TotalMilliseconds}";
        }

        private static readonly string DotNetCliExeName = PlatformUtil.IsRunningUnderWindows ? /*"dotnet.exe"*/$"shared/Microsoft.NETCore.App/{PlatformVersion}/CoreRun.exe" : "dotnet";

        public static string GetDbgShimPath(this ApplicationDescriptor app)
        {
            return $@"{DotNetCoreRootPath}shared/Microsoft.NETCore.App/{PlatformVersion}/{(PlatformUtil.IsRunningUnderWindows ? "dbgshim.dll" : "libdbgshim.dll")}";
        }
    }
}