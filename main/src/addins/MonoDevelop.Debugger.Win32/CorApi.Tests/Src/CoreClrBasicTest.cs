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

            uint pid;
            ICorDebug cordbg = CoreClrShimUtil.CreateICorDebugForCommand(new DbgShimInterop(app.GetDbgShimPath()), app.GetCommandlineForSleep(TimeSpan.FromSeconds(10)), app.WorkingDirectory, new Dictionary<string, string>(), TimeSpan.FromSeconds(10), out pid);
            pid.ShouldBeGreaterThan(0u);
            cordbg.ShouldNotBeNull();

            CheckBaseCorDbg(cordbg, cdbg =>
            {
                ICorDebugProcess process;
                cdbg.DebugActiveProcess(pid, 0, out process);
                return process;
            });
        }
    }
}