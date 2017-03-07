using System;
using System.Collections.Generic;

using CorApi.ComInterop;
using CorApi.Pinvoke;
using CorApi.Tests.Infra;

using NUnit.Framework;

using Should;

namespace CorApi.Tests
{
    [Category("Core")]
    public class CoreClrBasicTest : BasicTestCommon
    {
        [Test]
        public void BasicTest()
        {
            ApplicationDescriptor app = Constants.NetCoreApp10ConsoleAppPdb;

            CoreClrShimUtil.CorDebugAndPid started = CoreClrShimUtil.CreateICorDebugForCommand(new DbgShimInterop(app.GetDbgShimPath()), app.GetCommandlineForSleep(TimeSpan.FromSeconds(10)), app.WorkingDirectory, new Dictionary<string, string>(), TimeSpan.FromSeconds(10));
            started.Pid.ShouldBeGreaterThan(0u);
            started.ICorDebug.ShouldNotBeNull();

            CheckBaseCorDbg(started.ICorDebug, cdbg =>
            {
                ICorDebugProcess process;
                cdbg.DebugActiveProcess(started.Pid, 0, out process);
                return process;
            });
        }
    }
}