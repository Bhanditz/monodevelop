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

        public static void ToggleBreakpointAndWaitForBind (this CorDebuggerSession session, string filename, int line,
            int timeoutSeconds = 10)
        {
            var toggleBreakpointHelper = new ManualResetEvent (false);
            EventHandler<BreakEventArgs> sessionOnBreakpointStatusChanged = (sender, args) => {
                if (args == null)
                    return;

                var breakpoint = args.BreakEvent as Breakpoint;
                if (breakpoint == null)
                    return;

                if (!breakpoint.FileName.Equals (filename) || line != breakpoint.Line) //TODO: normalize filename paths
                    return;

                if (breakpoint.GetStatus (session) == BreakEventStatus.Bound)
                    toggleBreakpointHelper.Set ();
            };

            session.Breakpoints.BreakEventStatusChanged += sessionOnBreakpointStatusChanged;
            try {
                session.Breakpoints.Add (filename, line);
                toggleBreakpointHelper.WaitOne (TimeSpan.FromSeconds (timeoutSeconds)).ShouldBeTrue
                    (string.Format("Breakpoint {0}:{1} wasn't bound", filename, line));
            }
            finally {
                session.Breakpoints.BreakEventStatusChanged -= sessionOnBreakpointStatusChanged;
            }
        }
    }
}