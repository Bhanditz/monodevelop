using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mono.Debugging.Client;
using Should;
using Xunit;

namespace Mono.Debugging.Win32.Tests
{
    public class EvaluationTests: IDisposable
    {
        private readonly CorDebuggerSession _session;

        public EvaluationTests ()
        {
            var app = Constants.Net45ConsoleApp;
            _session = new CorDebuggerSession (new char[] { });
            var initializationFinishedGuard = new ManualResetEvent (false);
            _session.TargetInterrupted += (sender, a1) => {
                initializationFinishedGuard.Set ();
            };

            _session.Run (app.GetStartInfoForDebuggerBreak (), new DebuggerSessionOptions ());
            initializationFinishedGuard.WaitOne (TimeSpan.FromSeconds (20))
                .ShouldBeTrue ("Initialization wasn't finished");
            var breakpointInfo = _session.GetBreakpointInfo ("Program.cs", "//EvaluationTests: insert breakpoint here");
            breakpointInfo.ShouldNotBeNull ();
            _session.ToggleBreakpointAndWaitForBind (breakpointInfo.Filename, breakpointInfo.Line, TimeSpan.FromSeconds (10));
            _session.ContinueAndWaitForBreakpointHit (breakpointInfo.Filename, breakpointInfo.Line, TimeSpan.FromSeconds (10));
        }

        private void CheckValue (ObjectValue value, string knownValue)
        {
//            EventHandler onValueChanged = (sender, args) => { //TODO:
//                knownValue.ShouldEqual (value.Value);
//            };
//
//            value.ValueChanged += onValueChanged;
//
//            try {
//                if (value.IsEvaluating) {
//                    knownValue.ShouldEqual (value.Value);
//                }
//            }
//            finally {
//                value.ValueChanged -= onValueChanged;
//            }
        }
        public void CheckEvaluation (String[] toEvaluate, String[] checkValues)
        {
            _session.ShouldNotBeNull ();
            if (_session.IsRunning)
                _session.StopAndWait (TimeSpan.FromSeconds (10));
            var threadInfo = _session.ActiveThread;
            int frameCount = threadInfo.Backtrace.FrameCount;
            var frames = new List<StackFrame>(frameCount);

            threadInfo.Backtrace.GetFrame(frameCount);
            for (int i = 0; i < frameCount; i++)
            {
                var stackFrame = threadInfo.Backtrace.GetFrame(i);
                if (stackFrame == null)
                    break;
                frames.Add(stackFrame);
            }

            frames.Count.ShouldEqual (1);
            var frame = frames.First ();

            var values = frame.GetExpressionValues (toEvaluate, new EvaluationOptions () {
                AllowMethodEvaluation = true
            });
            for (var i = 0; i < values.Length; i++) {
                CheckValue (values[i], checkValues[i]);
            }
        }

        [Fact]
        public void TestEvaluationOfString ()
        {
            //CheckEvaluation (new[] {"str"}, new[] {"Test string"});
        }

        public void Dispose ()
        {
            if (_session != null)
                _session.Dispose ();
        }
    }
}