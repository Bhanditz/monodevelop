using System;

using CorApi.ComInterop;

using Microsoft.Samples.Debugging.CorDebug;
using Mono.Debugging.Evaluation;
using DC = Mono.Debugging.Client;

namespace Mono.Debugging.Win32
{
	public class CorEvaluationContext: EvaluationContext
	{
		ICorDebugEval corEval;
		ICorDebugFrame frame;
		ICorDebugChain activeChain;
		int frameIndex;
		int evalTimestamp;
		readonly CorBacktrace backtrace;
		ICorDebugThread thread;
		int threadId;

		public CorDebuggerSession Session { get; set; }

		internal CorEvaluationContext (CorDebuggerSession session, CorBacktrace backtrace, int index, DC.EvaluationOptions ops): base (ops)
		{
			Session = session;
			base.Adapter = session.ObjectAdapter;
			frameIndex = index;
			this.backtrace = backtrace;
			evalTimestamp = CorDebuggerSession.EvaluationTimestamp;
			Evaluator = session.GetEvaluator (CorBacktrace.CreateFrame (session, Frame));
		}

		public new CorObjectAdaptor Adapter {
			get { return (CorObjectAdaptor)base.Adapter; }
		}

		void CheckTimestamp ( )
		{
			if (evalTimestamp != CorDebuggerSession.EvaluationTimestamp) {
				thread = null;
				frame = null;
				corEval = null;
				activeChain = null;
			}
		}

		public ICorDebugThread Thread {
			get {
				CheckTimestamp ();
				if (thread == null)
					thread = Session.GetThread (threadId);
				return thread;
			}
			set {
				thread = value;
				threadId = thread.Id;
			}
		}

		public ICorDebugChain ActiveChain {
			get {
				CheckTimestamp ();
				if (activeChain == null) {
					activeChain = Thread.ActiveChain;
				}
				return activeChain;
			}
		}

		public ICorDebugFrame Frame {
			get {
				CheckTimestamp ();
				if (frame == null) {
					frame = backtrace.FrameList [frameIndex];
				}
				return frame;
			}
		}

		public ICorDebugEval Eval {
			get {
				CheckTimestamp ();
				if (corEval == null)
					corEval = Thread.CreateEval ();
				return corEval;
			}
		}

		public override void CopyFrom (EvaluationContext ctx)
		{
			base.CopyFrom (ctx);
			frame = ((CorEvaluationContext) ctx).frame;
			frameIndex = ((CorEvaluationContext) ctx).frameIndex;
			evalTimestamp = ((CorEvaluationContext) ctx).evalTimestamp;
			Thread = ((CorEvaluationContext) ctx).Thread;
			Session = ((CorEvaluationContext) ctx).Session;
		}

		public override void WriteDebuggerError (Exception ex)
		{
			Session.Frontend.NotifyDebuggerOutput (true, ex.Message);
		}

		public override void WriteDebuggerOutput (string message, params object[] values)
		{
			Session.Frontend.NotifyDebuggerOutput (false, string.Format (message, values));
		}

		public ICorDebugValue RuntimeInvoke (ICorDebugFunction function, ICorDebugType[] typeArgs, ICorDebugValue thisObj, ICorDebugValue[] arguments)
		{
			return Session.RuntimeInvoke (this, function, typeArgs, thisObj, arguments);
		}
	}
}
