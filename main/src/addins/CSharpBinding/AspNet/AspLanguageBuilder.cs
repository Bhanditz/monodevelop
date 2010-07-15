// 
// AspDocumentBuilder.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using MonoDevelop.AspNet.Parser.Dom;
using MonoDevelop.AspNet.Gui;
using System.Text;
using MonoDevelop.Projects.Dom;
using MonoDevelop.Projects.Dom.Parser;
using System.Collections.Generic;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui;
using Mono.TextEditor;
using ICSharpCode.NRefactory;

namespace MonoDevelop.CSharp.Completion
{
	public class AspLanguageBuilder : Visitor, ILanguageCompletionBuilder
	{
		public bool SupportsLanguage (string language)
		{
			return language == "C#";
		}
		
		ParsedDocument Parse (string fileName, string text)
		{
			return new MonoDevelop.CSharp.Parser.NRefactoryParser ().Parse (null, fileName, text);
		}
		
		static void WriteUsings (IEnumerable<string> usings, StringBuilder builder)
		{
			foreach (var u in usings) {
				builder.Append ("using ");
				builder.Append (u);
				builder.AppendLine (";");
			}
		}
		
		static void WriteClassDeclaration (DocumentInfo info, StringBuilder builder)
		{
			builder.Append ("partial class ");
			builder.Append (info.ClassName);
			builder.Append (" : ");
			builder.AppendLine (info.BaseType);
		}
		
		public LocalDocumentInfo BuildLocalDocument (DocumentInfo info, TextEditorData data,
		                                             string expressionText, string textAfterCaret, bool isExpression)
		{
			var sb = new StringBuilder ();
			
			WriteUsings (info.Imports, sb);
			WriteClassDeclaration (info, sb);
			sb.AppendLine ("{");
			var result = new LocalDocumentInfo ();
			if (isExpression) {
				sb.AppendLine ("void Generated ()");
				sb.AppendLine ("{");
				//Console.WriteLine ("start:" + location.BeginLine  +"/" +location.BeginColumn);
				foreach (var expression in info.Expressions) {
					if (expression.Location.BeginLine > data.Caret.Line || expression.Location.BeginLine == data.Caret.Line && expression.Location.BeginColumn > data.Caret.Column - 5) 
						continue;
					//Console.WriteLine ("take xprt:" + expressions.Key.BeginLine  +"/" +expressions.Key.BeginColumn);
					if (expression.IsExpression)
						sb.Append ("WriteLine (");
					string expr = expression.Expression.Trim ('=');
					result.AddTextPosition (data.Document.LocationToOffset (expression.Location.BeginLine, expression.Location.EndLine), sb.Length, expr.Length);
					sb.Append (expr);
					if (expression.IsExpression)
						sb.Append (");");
				}
			}
			sb.Append (expressionText);
			int caretPosition = sb.Length;
			sb.Append (textAfterCaret);
			
			sb.AppendLine ();
			sb.AppendLine ("}");
			sb.AppendLine ("}");
			
			result.LocalDocument = sb.ToString ();
			result.CaretPosition = caretPosition;
			result.OriginalCaretPosition = data.Caret.Offset;
			result.ParsedLocalDocument = Parse (info.AspNetDocument.FileName, sb.ToString ());
			
			return result;
		}
		
		public ICompletionDataList HandlePopupCompletion (MonoDevelop.Ide.Gui.Document realDocument, DocumentInfo info, LocalDocumentInfo localInfo)
		{
			CodeCompletionContext codeCompletionContext;
			using (var completion = CreateCompletion (realDocument, info, localInfo, out codeCompletionContext)) {
				return completion.CodeCompletionCommand (codeCompletionContext);
			}
		}
		
		public ICompletionDataList HandleCompletion (MonoDevelop.Ide.Gui.Document realDocument, CodeCompletionContext completionContext, DocumentInfo info, LocalDocumentInfo localInfo, char currentChar, ref int triggerWordLength)
		{
			CodeCompletionContext ccc;
			using (var completion = CreateCompletion (realDocument, info, localInfo, out ccc)) {
				return completion.HandleCodeCompletion (completionContext, currentChar, ref triggerWordLength);
			}
		}
		
		public IParameterDataProvider HandleParameterCompletion (MonoDevelop.Ide.Gui.Document realDocument, CodeCompletionContext completionContext, DocumentInfo info, LocalDocumentInfo localInfo, char completionChar)
		{
			CodeCompletionContext ccc;
			using (var completion = CreateCompletion (realDocument, info, localInfo, out ccc)) {
				return completion.HandleParameterCompletion (completionContext, completionChar);
			}
		}
		
		public bool GetParameterCompletionCommandOffset (MonoDevelop.Ide.Gui.Document realDocument, DocumentInfo info, LocalDocumentInfo localInfo, out int cpos)
		{
			CodeCompletionContext codeCompletionContext;
			using (var completion = CreateCompletion (realDocument, info, localInfo, out codeCompletionContext)) {
				
				return completion.GetParameterCompletionCommandOffset (out cpos);
			}
		}

		public ICompletionWidget CreateCompletionWidget (MonoDevelop.Ide.Gui.Document realDocument, LocalDocumentInfo localInfo)
		{
			return new AspCompletionWidget (realDocument, localInfo);
		}

		CSharpTextEditorCompletion CreateCompletion (MonoDevelop.Ide.Gui.Document realDocument, DocumentInfo info, LocalDocumentInfo localInfo, out CodeCompletionContext codeCompletionContext)
		{
			var doc = new Mono.TextEditor.Document () {
				Text = localInfo.LocalDocument,
			};
			var documentLocation = doc.OffsetToLocation (localInfo.CaretPosition);
			
			codeCompletionContext = new CodeCompletionContext () {
				TriggerOffset = localInfo.CaretPosition,
				TriggerLine = documentLocation.Line + 1,
				TriggerLineOffset = documentLocation.Column + 1,
			};
			
			var r = new System.IO.StringReader (localInfo.LocalDocument);
			using (var parser = ICSharpCode.NRefactory.ParserFactory.CreateParser (SupportedLanguage.CSharp, r)) {
				parser.Parse ();
				return new CSharpTextEditorCompletion (localInfo.HiddenDocument) {
					ParsedUnit = parser.CompilationUnit,
					CompletionWidget = CreateCompletionWidget (realDocument, localInfo),
					Dom = localInfo.HiddenDocument.Dom
				};
			}
		}
		
		class AspCompletionWidget : ICompletionWidget
		{
			MonoDevelop.Ide.Gui.Document realDocument;
			LocalDocumentInfo localInfo;
			
			public AspCompletionWidget (MonoDevelop.Ide.Gui.Document realDocument, LocalDocumentInfo localInfo)
			{
				this.realDocument = realDocument;
				this.localInfo = localInfo;
			}

			#region ICompletionWidget implementation
			public CodeCompletionContext CurrentCodeCompletionContext {
				get {
					return CreateCodeCompletionContext (localInfo.CaretPosition);
				}
			}

			public event EventHandler CompletionContextChanged;

			public string GetText (int startOffset, int endOffset)
			{
				return localInfo.HiddenDocument.TextEditorData.Document.GetTextBetween (startOffset, endOffset);
			}

			public char GetChar (int offset)
			{
				return localInfo.HiddenDocument.TextEditorData.Document.GetCharAt (offset);
			}

			public void Replace (int offset, int count, string text)
			{
				throw new NotImplementedException ();
			}

			public CodeCompletionContext CreateCodeCompletionContext (int triggerOffset)
			{
				var savedCtx = realDocument.GetContent<ICompletionWidget> ().CreateCodeCompletionContext (realDocument.TextEditorData.Caret.Offset + triggerOffset - localInfo.CaretPosition);
				CodeCompletionContext result = new CodeCompletionContext ();
				result.TriggerOffset = triggerOffset;
				DocumentLocation loc = localInfo.HiddenDocument.TextEditorData.Document.OffsetToLocation (triggerOffset);
				result.TriggerLine   = loc.Line + 1;
				result.TriggerLineOffset = loc.Column + 1;
				
				result.TriggerXCoord = savedCtx.TriggerXCoord;
				result.TriggerYCoord = savedCtx.TriggerYCoord;
				result.TriggerTextHeight = savedCtx.TriggerTextHeight;
				return result;
			}

			public string GetCompletionText (CodeCompletionContext ctx)
			{
				if (ctx == null)
					return null;
				int min = Math.Min (ctx.TriggerOffset, localInfo.HiddenDocument.TextEditorData.Caret.Offset);
				int max = Math.Max (ctx.TriggerOffset, localInfo.HiddenDocument.TextEditorData.Caret.Offset);
				return localInfo.HiddenDocument.TextEditorData.Document.GetTextBetween (min, max);
			}

			public void SetCompletionText (CodeCompletionContext ctx, string partial_word, string complete_word)
			{
				CodeCompletionContext translatedCtx = new CodeCompletionContext ();
				/*int offset = ctx.TriggerOffset;
				var info = localInfo.OffsetInfos.FirstOrDefault (x => x.FromOffset <= offset && offset < x.FromOffset + x.Length);
				if (info != null)
					offset = offset - info.FromOffset + info.ToOffset;
				*/
				int offset = localInfo.OriginalCaretPosition + ctx.TriggerOffset - localInfo.CaretPosition;
				translatedCtx.TriggerOffset = offset;
				DocumentLocation loc = localInfo.HiddenDocument.TextEditorData.Document.OffsetToLocation (offset);
				translatedCtx.TriggerLine   = loc.Line + 1;
				translatedCtx.TriggerLineOffset = loc.Column + 1;
				translatedCtx.TriggerWordLength = ctx.TriggerWordLength;
				realDocument.GetContent <ICompletionWidget> ().SetCompletionText (translatedCtx, partial_word, complete_word);
			}

			public int TextLength {
				get {
					return localInfo.HiddenDocument.TextEditorData.Document.Length;
				}
			}

			public int SelectedLength {
				get {
					return 0;
				}
			}

			public Gtk.Style GtkStyle {
				get {
					return Gtk.Widget.DefaultStyle;
				}
			}
			#endregion
		}
		
		public ParsedDocument BuildDocument (DocumentInfo info, TextEditorData data)
		{
			var document = new StringBuilder ();
			
			WriteUsings (info.Imports, document);
			WriteClassDeclaration (info, document);
			
			foreach (var node in info.ScriptBlocks) {
				int start = data.Document.LocationToOffset (node.Location.EndLine - 1,  node.Location.EndColumn);
				int end = data.Document.LocationToOffset (node.EndLocation.BeginLine - 1, node.EndLocation.BeginColumn);
				document.AppendLine (data.Document.GetTextBetween (start, end));
			}
			
			var docStr = document.ToString ();
			document.Length = 0;
			return Parse (info.AspNetDocument.FileName, docStr);
		}
	}
}
