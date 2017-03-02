using System;
using System.Collections.Generic;
using CorApi.Pinvoke;
using CorApi.Tests.Infra;
using Mono.Debugging.Win32.Tests;
using Should;
using Xunit;

namespace CorApi.Tests
{
    public class CoreClrBasicTest
    {

        [Fact]
        public void BasicTest()
        {
            var app = Constants.NetCoreApp10ConsoleAppPdb;

            int pid;
            var corDebug = CoreClrShimUtil.CreateICorDebugForCommand(
                new DbgShimInterop(app.GetDbgShimPath()), app.GetCommandlineForSleep(TimeSpan.FromSeconds(10)),
                app.WorkingDirectory, new Dictionary<string, string>(), TimeSpan.FromSeconds(10), out pid);
            pid.ShouldBeGreaterThan(0);
            corDebug.ShouldNotBeNull();
        }
    }
}