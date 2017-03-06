using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore
{
                [
                                ComVisible(false)
                ]
                public interface ISymbolWriter2 : ISymbolWriter
                {
                                void Initialize(Object emitter,
                                                String fileName,
                                                Boolean fullBuild);

                                void Initialize(Object emitter,
                                                String fileName,
                                                IStream stream,
                                                Boolean fullBuild);

                                void Initialize(Object emitter,
                                                String temporaryFileName,
                                                IStream stream,
                                                Boolean fullBuild,
                                                String finalFileName);

                                byte[] GetDebugInfo(out ImageDebugDirectory imageDebugDirectory);
                             
                                void RemapToken(SymbolToken oldToken,
                                                SymbolToken newToken);
                             
                                void DefineConstant(String name,
                                                Object value,
                                                byte[] signature);
    
                                void Abort();   

                                void DefineLocalVariable(String name,
                                                int attributes,
                                                SymbolToken sigToken,
                                                int addressKind,
                                                int addr1,
                                                int addr2,
                                                int addr3,
                                                int startOffset,
                                                int endOffset);
    
                                void DefineGlobalVariable(String name,
                                                int attributes,
                                                SymbolToken sigToken,
                                                int addressKind,
                                                int addr1,
                                                int addr2,
                                                int addr3);
        
        
                                void DefineConstant(String name,
                                                Object value,
                                                SymbolToken sigToken);
                }
}