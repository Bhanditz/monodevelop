using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Reflection;

using CorApi.ComInterop;

using Mono.Debugging.Client;
using Mono.Debugging.Evaluation;
using System.Linq;
using System.Runtime.InteropServices;

using CorApi2.Metadata;

using StackFrame = Mono.Debugging.Client.StackFrame;

namespace Mono.Debugging.Win32
{
	unsafe class CorBacktrace: BaseBacktrace
	{
		ICorDebugThread thread;
		readonly uint threadId;
		readonly CorDebuggerSession session;
		List<ICorDebugFrame> frames;
		int evalTimestamp;

		public CorBacktrace (ICorDebugThread thread, CorDebuggerSession session): base (session.ObjectAdapter)
		{
			this.session = session;
			this.thread = thread;
			uint dwThreadId = 0;
			thread.GetID(&dwThreadId).AssertSucceeded("thread.GetID(&dwThreadId)");
			threadId = dwThreadId;
			frames = new List<ICorDebugFrame> (GetFrames (thread));
			evalTimestamp = CorDebuggerSession.EvaluationTimestamp;
		}

		internal static IEnumerable<ICorDebugFrame> GetFrames(ICorDebugThread thread)
		{
			var corFrames = new List<ICorDebugFrame>();
			try
			{
				foreach(ICorDebugChain chain in thread.GetChains())
				{
					int isManaged = 0;
					chain.IsManaged(&isManaged).AssertSucceeded("chain.IsManaged(&isManaged)");
					if(!(isManaged != 0))
						continue;
					try
					{
						IList<ICorDebugFrame> chainFrames = chain.GetFrames();

						foreach(ICorDebugFrame frame in chainFrames)
							corFrames.Add(frame);
					}
					catch(COMException e)
					{
						DebuggerLoggingService.LogMessage("Failed to enumerate frames of chain: {0}", e.Message);
					}
				}
			}
			catch(COMException e)
			{
				DebuggerLoggingService.LogMessage("Failed to enumerate chains of thread: {0}", e.Message);
			}
			return corFrames;
		}

		internal List<ICorDebugFrame> FrameList
		{
			get
			{
				if(evalTimestamp != CorDebuggerSession.EvaluationTimestamp)
				{
					thread = session.GetThread(((int)threadId));
					frames = new List<ICorDebugFrame>(GetFrames(thread));
					evalTimestamp = CorDebuggerSession.EvaluationTimestamp;
				}
				return frames;
			}
		}

		protected override EvaluationContext GetEvaluationContext (int frameIndex, EvaluationOptions options)
		{
			CorEvaluationContext ctx = new CorEvaluationContext (session, this, frameIndex, options);
			ctx.Thread = thread;
			return ctx;
		}
	
		#region IBacktrace Members

		public override AssemblyLine[] Disassemble (int frameIndex, int firstLine, int count)
		{
			return new AssemblyLine[0];
		}

		public override int FrameCount
		{
			get { return FrameList.Count; }
		}

		public override StackFrame[] GetStackFrames (int firstIndex, int lastIndex)
		{
			if (lastIndex >= FrameList.Count)
				lastIndex = FrameList.Count - 1;
			StackFrame[] array = new StackFrame[lastIndex - firstIndex + 1];
			for (int n = 0; n < array.Length; n++)
				array[n] = CreateFrame (session, FrameList[n + firstIndex]);
			return array;
		}

		private const int SpecialSequencePoint = 0xfeefee;

		public static SequencePoint GetSequencePoint(CorDebuggerSession session, ICorDebugFrame frame)
		{
			ICorDebugFunction framefunction;
			frame.GetFunction(out framefunction).AssertSucceeded("Could not get the Function of a Frame.");
			ICorDebugModule module;
			framefunction.GetModule(out module).AssertSucceeded("framefunction.GetModule(out module)");;
			ISymbolReader reader = session.GetReaderForModule (module);
			if (reader == null)
				return null;

			uint mdMethodDef = 0;
			framefunction.GetToken(&mdMethodDef).AssertSucceeded("framefunction.GetToken(&mdMethodDef)");
			ISymbolMethod met = reader.GetMethod (new SymbolToken (((int)mdMethodDef)));
			if (met == null)
				return null;

			int SequenceCount = met.SequencePointCount;
			if (SequenceCount <= 0)
				return null;

			CorDebugMappingResult mappingResult = 0;
			uint ip = 0;
			Com.QueryInteface<ICorDebugILFrame>(frame).GetIP(&ip, &mappingResult).AssertSucceeded("Com.QueryInteface<ICorDebugILFrame>(frame).GetIP(&ip, &mappingResult)");
			if (mappingResult == CorDebugMappingResult.MAPPING_NO_INFO || mappingResult == CorDebugMappingResult.MAPPING_UNMAPPED_ADDRESS)
				return null;

			int[] offsets = new int[SequenceCount];
			int[] lines = new int[SequenceCount];
			int[] endLines = new int[SequenceCount];
			int[] columns = new int[SequenceCount];
			int[] endColumns = new int[SequenceCount];
			ISymbolDocument[] docs = new ISymbolDocument[SequenceCount];
			met.GetSequencePoints (offsets, docs, lines, columns, endLines, endColumns);

			if ((SequenceCount > 0) && (offsets [0] <= ip)) {
				int i;
				for (i = 0; i < SequenceCount; ++i) {
					if (offsets [i] >= ip) {
						break;
					}
				}

				if ((i == SequenceCount) || (offsets [i] != ip)) {
					--i;
				}

				if (lines [i] == SpecialSequencePoint) {
					int j = i;
					// let's try to find a sequence point that is not special somewhere earlier in the code
					// stream.
					while (j > 0) {
						--j;
						if (lines [j] != SpecialSequencePoint) {
							return new SequencePoint () {
								IsSpecial = true,
								Offset = offsets [j],
								StartLine = lines [j],
								EndLine = endLines [j],
								StartColumn = columns [j],
								EndColumn = endColumns [j],
								Document = docs [j]
							};
						}
					}
					// we didn't find any non-special seqeunce point before current one, let's try to search
					// after.
					j = i;
					while (++j < SequenceCount) {
						if (lines [j] != SpecialSequencePoint) {
							return new SequencePoint () {
								IsSpecial = true,
								Offset = offsets [j],
								StartLine = lines [j],
								EndLine = endLines [j],
								StartColumn = columns [j],
								EndColumn = endColumns [j],
								Document = docs [j]
							};
						}
					}

					// Even if sp is null at this point, it's a valid scenario to have only special sequence 
					// point in a function.  For example, we can have a compiler-generated default ctor which
					// doesn't have any source.
					return null;
				} else {
					return new SequencePoint () {
						IsSpecial = false,
						Offset = offsets [i],
						StartLine = lines [i],
						EndLine = endLines [i],
						StartColumn = columns [i],
						EndColumn = endColumns [i],
						Document = docs [i]
					};
				}
			}
			return null;
		}

		internal static StackFrame CreateFrame(CorDebuggerSession session, ICorDebugFrame frame)
		{
			uint address = 0;
			string addressSpace = "";
			string file = "";
			int line = 0;
			int endLine = 0;
			int column = 0;
			int endColumn = 0;
			string method = "[Unknown]";
			string lang = "";
			string module = "";
			string type = "";
			bool hasDebugInfo = false;
			bool hidden = false;
			bool external;

			ICorDebugFrameEx.CorFrameType frametype = frame.GetFrameType();
			if(frametype == ICorDebugFrameEx.CorFrameType.ILFrame)
			{
				ICorDebugFunction framefunction;
				frame.GetFunction(out framefunction).AssertSucceeded("Could not get the Function of a Frame.");
				if(framefunction != null)
				{
					ICorDebugModule functionmodule;
					framefunction.GetModule(out functionmodule).AssertSucceeded("framefunction.GetModule(out functionmodule)");
					module = LpwstrHelper.GetString(functionmodule.GetName, "Could not get the Frame Function Module Name.");
					var importer = new CorMetadataImport(functionmodule);
					uint mdMethodDef;
					framefunction.GetToken(&mdMethodDef).AssertSucceeded("framefunction.GetToken(&mdMethodDef)");
					MethodInfo mi = importer.GetMethodInfo(mdMethodDef);
					Type declaringType = mi.DeclaringType;
					if(declaringType != null)
					{
						method = declaringType.FullName + "." + mi.Name;
						type = declaringType.FullName;
					}
					else
						method = mi.Name;

					addressSpace = mi.Name;

					SequencePoint sp = GetSequencePoint(session, frame);
					if(sp != null)
					{
						line = sp.StartLine;
						column = sp.StartColumn;
						endLine = sp.EndLine;
						endColumn = sp.EndColumn;
						file = sp.Document.URL;
						address = (uint)sp.Offset;
					}

					if(session.IsExternalCode(file))
						external = true;
					else
					{
						if(session.Options.ProjectAssembliesOnly)
							external = mi.GetCustomAttributes(true).Any(v => v is DebuggerHiddenAttribute || v is DebuggerNonUserCodeAttribute);
						else
							external = mi.GetCustomAttributes(true).Any(v => v is DebuggerHiddenAttribute);
					}
					hidden = mi.GetCustomAttributes(true).Any(v => v is DebuggerHiddenAttribute);
				}
				else
					external = true;
				lang = "Managed";
				hasDebugInfo = true;
			}
			else if(frametype == ICorDebugFrameEx.CorFrameType.NativeFrame)
			{
				external = true;
				Com.QueryInteface<ICorDebugNativeFrame>(frame).GetIP(&address).AssertSucceeded("Com.QueryInteface<ICorDebugNativeFrame>(frame).GetIP(&address)");
				method = "[Native frame]";
				lang = "Native";
			}
			else if(frametype == ICorDebugFrameEx.CorFrameType.InternalFrame)
			{
				external = true;
				CorDebugInternalFrameType intframetype = 0;
				Com.QueryInteface<ICorDebugInternalFrame>(frame).GetFrameType(&intframetype).AssertSucceeded("Com.QueryInteface<ICorDebugInternalFrame>(frame).GetFrameType(&intframetype)");
				switch(intframetype)
				{
				case CorDebugInternalFrameType.STUBFRAME_M2U:
					method = "[Managed to Native Transition]";
					break;
				case CorDebugInternalFrameType.STUBFRAME_U2M:
					method = "[Native to Managed Transition]";
					break;
				case CorDebugInternalFrameType.STUBFRAME_LIGHTWEIGHT_FUNCTION:
					method = "[Lightweight Method Call]";
					break;
				case CorDebugInternalFrameType.STUBFRAME_APPDOMAIN_TRANSITION:
					method = "[Application Domain Transition]";
					break;
				case CorDebugInternalFrameType.STUBFRAME_FUNC_EVAL:
					method = "[Function Evaluation]";
					break;
				}
			}
			else
				external = true;

			var loc = new SourceLocation(method, file, line, column, endLine, endColumn);
			return new StackFrame(address, addressSpace, loc, lang, external, hasDebugInfo, hidden, module, type);
		}

		#endregion
	}
}
