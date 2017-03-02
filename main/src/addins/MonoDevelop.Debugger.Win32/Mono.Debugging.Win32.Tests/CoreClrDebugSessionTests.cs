using System;
using System.IO;
using System.Threading;
using Mono.Debugging.Client;
using Should;
using Xunit;

namespace Mono.Debugging.Win32.Tests
{
    public class CoreClrDebugSessionTests
    {
        [Fact]
        public void ShouldCorrectlyEnemrateAppDoimains()
        {
            /*CorDebuggerSession session = null;
            var app = Constants.;
            try
            {
                session = new CoreClrDebuggerSession(new char[] { }, app.GetDbgShimPath());
                session.Run(app.GetStartInfoForSleep(TimeSpan.FromSeconds(5)), new DebuggerSessionOptions());

                // Wait when until Thread.Sleep started
                Thread.Sleep(5000);

                session.StopAndWait(TimeSpan.FromSeconds(5));

                var appDomains = session.GetAppDomains().ToList();
                var modules = session.GetAllModules().ToList();
                appDomains.Count.ShouldEqual(1);
                modules.Count.ShouldEqual(2);
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                    session = null;
                }
            }*/
        }
    }
}