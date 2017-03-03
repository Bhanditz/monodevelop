using System;
using System.Collections.Generic;
using System.Threading;

using CorApi.ComInterop;
using CorApi.Pinvoke;
using CorApi.Tests.Infra;

using NUnit.Framework;

using Should;

namespace CorApi.Tests
{
    [Category("Core")]
    public unsafe class CoreClrBasicTest : BasicTestCommon
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

            uint numproc = 0;
            for(int a = 0; (a < 0x1000) && (numproc == 0); a++)
            {
                ICorDebugProcessEnum @enum;
                cordbg.EnumerateProcesses(out @enum);
                @enum.GetCount(&numproc);
                Thread.Sleep(0x10);
            }

            Console.Error.WriteLine("Got {0} processes.", numproc);
        }
    }
}