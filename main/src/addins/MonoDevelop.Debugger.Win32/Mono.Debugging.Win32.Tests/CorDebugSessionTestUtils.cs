using System;
using System.Threading;
using Mono.Debugging.Client;
using Should;

namespace Mono.Debugging.Win32.Tests
{
    public static class CorDebugSessionTestUtils
    {
        private static EventHandler<TargetEventArgs> sessionOnTargetStopped;

        public static void StopAndWait (this CorDebuggerSession session, int timeoutSeconds = 10)
        {
            var stopHelper = new ManualResetEvent (false);
            sessionOnTargetStopped = (sender, args) => {
                stopHelper.Set ();
            };
            session.TargetStopped += sessionOnTargetStopped;
            try {
                session.Stop ();
                stopHelper.WaitOne (TimeSpan.FromSeconds (timeoutSeconds)).ShouldBeTrue ("Session wasn't stopped");
            }
            finally {
                session.TargetStopped -= sessionOnTargetStopped;
            }
        }
    }
}