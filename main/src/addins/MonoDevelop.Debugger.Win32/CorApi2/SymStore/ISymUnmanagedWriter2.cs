using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
                             [
                                                          ComImport,
                                                          Guid("0B97726E-9E6D-4f05-9A26-424022093CAA"),
                                                          InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
                                                          ComVisible(false)
                             ]
                             internal interface ISymUnmanagedWriter2 : ISymUnmanagedWriter
                             {
                                                          // ISymUnmanagedWriter interfaces (need to define the base interface methods also, per COM interop requirements)
                                                          new void DefineDocument([MarshalAs(UnmanagedType.LPWStr)] String url,
                                                                                       ref Guid language,
                                                                                       ref Guid languageVendor,
                                                                                       ref Guid documentType,
                                                                                       [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocumentWriter RetVal);

                                                          new void SetUserEntryPoint(SymbolToken entryMethod);

                                                          new void OpenMethod(SymbolToken method);

                                                          new void CloseMethod();

                                                          new void OpenScope(int startOffset,
                                                                                       out int pRetVal);

                                                          new void CloseScope(int endOffset);

                                                          new void SetScopeRange(int scopeID,
                                                                                       int startOffset,
                                                                                       int endOffset);

                                                          new void DefineLocalVariable([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int attributes,
                                                                                       int cSig,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] signature,
                                                                                       int addressKind,
                                                                                       int addr1,
                                                                                       int addr2,
                                                                                       int addr3,
                                                                                       int startOffset,
                                                                                       int endOffset);

                                                          new void DefineParameter([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int attributes,
                                                                                       int sequence,
                                                                                       int addressKind,
                                                                                       int addr1,
                                                                                       int addr2,
                                                                                       int addr3);

                                                          new void DefineField(SymbolToken parent,
                                                                                       [MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int attributes,
                                                                                       int cSig,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] signature,
                                                                                       int addressKind,
                                                                                       int addr1,
                                                                                       int addr2,
                                                                                       int addr3);

                                                          new void DefineGlobalVariable([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int attributes,
                                                                                       int cSig,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] signature,
                                                                                       int addressKind,
                                                                                       int addr1,
                                                                                       int addr2,
                                                                                       int addr3);

                                                          new void Close();

                                                          new void SetSymAttribute(SymbolToken parent,
                                                                                       [MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int cData,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data);

                                                          new void OpenNamespace([MarshalAs(UnmanagedType.LPWStr)] String name);

                                                          new void CloseNamespace();

                                                          new void UsingNamespace([MarshalAs(UnmanagedType.LPWStr)] String fullName);

                                                          new void SetMethodSourceRange(ISymUnmanagedDocumentWriter startDoc,
                                                                                       int startLine,
                                                                                       int startColumn,
                                                                                       ISymUnmanagedDocumentWriter endDoc,
                                                                                       int endLine,
                                                                                       int endColumn);

                                                          new void Initialize(IntPtr emitter,
                                                                                       [MarshalAs(UnmanagedType.LPWStr)] String filename,
                                                                                       IStream stream,
                                                                                       Boolean fullBuild);

                                                          new void GetDebugInfo(out ImageDebugDirectory iDD,
                                                                                       int cData,
                                                                                       out int pcData,
                                                                                       [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

                                                          new void DefineSequencePoints(ISymUnmanagedDocumentWriter document,
                                                                                       int spCount,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] offsets,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] lines,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] columns,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] endLines,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] endColumns);

                                                          new void RemapToken(SymbolToken oldToken,
                                                                                       SymbolToken newToken);

                                                          new void Initialize2(IntPtr emitter,
                                                                                       [MarshalAs(UnmanagedType.LPWStr)] String tempfilename,
                                                                                       IStream stream,
                                                                                       Boolean fullBuild,
                                                                                       [MarshalAs(UnmanagedType.LPWStr)] String finalfilename);

                                                          new void DefineConstant([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       Object value,
                                                                                       int cSig,
                                                                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] signature);

                                                          new void Abort();

                                                          // ISymUnmanagedWriter2 interfaces
                                                          void DefineLocalVariable2([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int attributes,
                                                                                       SymbolToken sigToken,
                                                                                       int addressKind,
                                                                                       int addr1,
                                                                                       int addr2,
                                                                                       int addr3,
                                                                                       int startOffset,
                                                                                       int endOffset);
          
                                                          void DefineGlobalVariable2([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       int attributes,
                                                                                       SymbolToken sigToken,
                                                                                       int addressKind,
                                                                                       int addr1,
                                                                                       int addr2,
                                                                                       int addr3);
          
          
                                                          void DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                       Object value,
                                                                                       SymbolToken sigToken);
                             };
}