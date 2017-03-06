using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
                        [
                                                ComImport,
                                                Guid("ED14AA72-78E2-4884-84E2-334293AE5214"),
                                                InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
                                                ComVisible(false)
                        ]
                        internal interface ISymUnmanagedWriter 
                        {
                                                void DefineDocument([MarshalAs(UnmanagedType.LPWStr)] String url,
                                                                        ref Guid language,
                                                                        ref Guid languageVendor,
                                                                        ref Guid documentType,
                                                                        [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocumentWriter RetVal);
    
                                                void SetUserEntryPoint(SymbolToken entryMethod);
    
                                                void OpenMethod(SymbolToken method);
    
                                                void CloseMethod();
    
                                                void OpenScope(int startOffset,
                                                                        out int pRetVal);
    
                                                void CloseScope(int endOffset);
    
                                                void SetScopeRange(int scopeID,
                                                                        int startOffset,
                                                                        int endOffset);
    
                                                void DefineLocalVariable([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                        int attributes,
                                                                        int cSig,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] signature,
                                                                        int addressKind,
                                                                        int addr1,
                                                                        int addr2,
                                                                        int addr3,
                                                                        int startOffset,
                                                                        int endOffset);
    
                                                void DefineParameter([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                        int attributes,
                                                                        int sequence,
                                                                        int addressKind,
                                                                        int addr1,
                                                                        int addr2,
                                                                        int addr3);
    
                                                void DefineField(SymbolToken parent,
                                                                        [MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                        int attributes,
                                                                        int cSig,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] byte[] signature,
                                                                        int addressKind,
                                                                        int addr1,
                                                                        int addr2,
                                                                        int addr3);
    
                                                void DefineGlobalVariable([MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                        int attributes,
                                                                        int cSig,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] signature,
                                                                        int addressKind,
                                                                        int addr1,
                                                                        int addr2,
                                                                        int addr3);
    
                                                void Close();
    
                                                void SetSymAttribute(SymbolToken parent,
                                                                        [MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                        int cData,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] data);
    
                                                void OpenNamespace([MarshalAs(UnmanagedType.LPWStr)] String name);
    
                                                void CloseNamespace();
    
                                                void UsingNamespace([MarshalAs(UnmanagedType.LPWStr)] String fullName);
    
                                                void SetMethodSourceRange(ISymUnmanagedDocumentWriter startDoc,
                                                                        int startLine,
                                                                        int startColumn,
                                                                        ISymUnmanagedDocumentWriter endDoc,
                                                                        int endLine,
                                                                        int endColumn);
    
                                                void Initialize(IntPtr emitter,
                                                                        [MarshalAs(UnmanagedType.LPWStr)] String filename,
                                                                        IStream stream,
                                                                        Boolean fullBuild);
    
                                                void GetDebugInfo(out ImageDebugDirectory iDD,
                                                                        int cData,
                                                                        out int pcData,
                                                                        [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] data);
    
                                                void DefineSequencePoints(ISymUnmanagedDocumentWriter document,
                                                                        int spCount,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] offsets,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] lines,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] columns,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] endLines,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] endColumns);
    
                                                void RemapToken(SymbolToken oldToken,
                                                                        SymbolToken newToken);
    
                                                void Initialize2(IntPtr emitter,
                                                                        [MarshalAs(UnmanagedType.LPWStr)] String tempfilename,
                                                                        IStream stream,
                                                                        Boolean fullBuild,
                                                                        [MarshalAs(UnmanagedType.LPWStr)] String finalfilename);
    
                                                void DefineConstant( [MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                        Object value,
                                                                        int cSig,
                                                                        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] signature);
    
                                                void Abort();
    
                        }
}