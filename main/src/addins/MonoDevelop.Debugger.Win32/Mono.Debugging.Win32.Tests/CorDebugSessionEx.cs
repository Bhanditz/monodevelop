using System;
using System.IO;
using System.Threading;
using Mono.Debugging.Client;
using Should;

namespace Mono.Debugging.Win32.Tests
{
    public static class CorDebugSessionEx
    {
        private static EventHandler<TargetEventArgs> _sessionOnTargetStopped;

        public static void StopAndWait (this CorDebuggerSession session, TimeSpan timeout)
        {
            var stopHelper = new ManualResetEvent (false);
            _sessionOnTargetStopped = (sender, args) => { stopHelper.Set (); };
            session.TargetStopped += _sessionOnTargetStopped;
            try {
                session.Stop ();
                stopHelper.WaitOne (timeout).ShouldBeTrue ("Session wasn't stopped");
            }
            finally {
                session.TargetStopped -= _sessionOnTargetStopped;
            }
        }

        public static void ToggleBreakpointAndWaitForBind (this CorDebuggerSession session, string filename, int line,
            TimeSpan timeout)
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
                toggleBreakpointHelper.WaitOne (timeout)
                    .ShouldBeTrue
                    (string.Format ("Breakpoint {0}:{1} wasn't bound", filename, line));
            }
            finally {
                session.Breakpoints.BreakEventStatusChanged -= sessionOnBreakpointStatusChanged;
            }
        }

        private static int GetSpecificLineWithMarkerInside (string filename, string marker)
        {
            var currentLineNumber = 0;
            foreach (var line in File.ReadLines (filename)) {
                currentLineNumber++;
                if (line.Contains (marker)) {
                    return currentLineNumber;
                }
            }
            return -1;
        }

        public static BreakpointShortInfo GetBreakpointInfo (this CorDebuggerSession session, string fileShortName,
            string marker)
        {
            var docFiles = session.GetAllDocumentPaths ();
            foreach (var docFile in docFiles) {
                if (Path.GetFileName (docFile) != fileShortName)
                    continue;
                var breakpointLine = CorDebugSessionEx.GetSpecificLineWithMarkerInside (docFile, marker);

                if (breakpointLine > 0) {
                    return new BreakpointShortInfo {
                        Filename = docFile,
                        Line = breakpointLine
                    };
                }
            }
            return null;
        }

        public static void ContinueAndWaitForBreakpointHit (this CorDebuggerSession session, string filename, int line, TimeSpan timeout)
        {
            var breakpointHitGuard = new ManualResetEvent (false);
            EventHandler<TargetEventArgs> onBreakpointHit = (sender, args) => {
                var breakpoint = args.BreakEvent as Breakpoint;
                if (breakpoint == null)
                    return;

                if (breakpoint.FileName != filename || breakpoint.Line != line)
                    return;

                breakpointHitGuard.Set ();
            };
            session.TargetHitBreakpoint += onBreakpointHit;

            try {
                session.Continue ();
                breakpointHitGuard.WaitOne (timeout)
                    .ShouldBeTrue
                    ("Breakpoint wasn\'t hit");
            }
            finally {
                session.TargetHitBreakpoint -= onBreakpointHit;
            }
        }

        public class BreakpointShortInfo
        {
            public String Filename;
            public int Line;
        }
    }
}