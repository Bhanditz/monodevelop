using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace CorApi2.SymStore
{
                              [
                                                            ComImport,
                                                            Guid("B4CE6286-2A6B-3712-A3B7-1EE1DAD467B5"),
                                                            InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
                                                            ComVisible(false)
                              ]
                              internal interface ISymUnmanagedReader
                              {
                                                            void GetDocument([MarshalAs(UnmanagedType.LPWStr)] String url,
                                                                                          Guid language,
                                                                                          Guid languageVendor,
                                                                                          Guid documentType,
                                                                                          [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedDocument retVal);
  
                                                            void GetDocuments(int cDocs,
                                                                                          out int pcDocs,
                                                                                          [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedDocument[] pDocs);
        

                                                            // These methods will often return error HRs in common cases.
                                                            // Using PreserveSig and manually handling error cases provides a big performance win.
                                                            // Far fewer exceptions will be thrown and caught.
                                                            // Exceptions should be reserved for truely "exceptional" cases.
                                                            [PreserveSig]
                                                            int GetUserEntryPoint(out SymbolToken EntryPoint);
    
                                                            [PreserveSig]
                                                            int GetMethod(SymbolToken methodToken,
                                                                                          [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod retVal);

                                                            [PreserveSig]
                                                            int GetMethodByVersion(SymbolToken methodToken,
                                                                                          int version,
                                                                                          [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod retVal);
    
                                                            void GetVariables(SymbolToken parent,
                                                                                          int cVars,
                                                                                          out int pcVars,
                                                                                          [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] ISymUnmanagedVariable[] vars);

                                                            void GetGlobalVariables(int cVars,
                                                                                          out int pcVars,
                                                                                          [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedVariable[] vars);

         
                                                            void GetMethodFromDocumentPosition(ISymUnmanagedDocument document,
                                                                                          int line,
                                                                                          int column,
                                                                                          [MarshalAs(UnmanagedType.Interface)] out ISymUnmanagedMethod retVal);
    
                                                            void GetSymAttribute(SymbolToken parent,
                                                                                          [MarshalAs(UnmanagedType.LPWStr)] String name,
                                                                                          int sizeBuffer,
                                                                                          out int lengthBuffer,
                                                                                          [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] buffer);
    
                                                            void GetNamespaces(int cNameSpaces,
                                                                                          out int pcNameSpaces,
                                                                                          [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ISymUnmanagedNamespace[] namespaces);
    
                                                            void Initialize(IntPtr importer,
                                                                                          [MarshalAs(UnmanagedType.LPWStr)] String filename,
                                                                                          [MarshalAs(UnmanagedType.LPWStr)] String searchPath,
                                                                                          IStream stream);
    
                                                            void UpdateSymbolStore([MarshalAs(UnmanagedType.LPWStr)] String filename,
                                                                                          IStream stream);
    
                                                            void ReplaceSymbolStore([MarshalAs(UnmanagedType.LPWStr)] String filename,
                                                                                          IStream stream);
    
                                                            void GetSymbolStoreFileName(int cchName,
                                                                                          out int pcchName,
                                                                                          [MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName);
    
                                                            void GetMethodsFromDocumentPosition(ISymUnmanagedDocument document,
                                                                                          int line,
                                                                                          int column,
                                                                                          int cMethod,
                                                                                          out int pcMethod,
                                                                                          [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] ISymUnmanagedMethod[] pRetVal);
    
                                                            void GetDocumentVersion(ISymUnmanagedDocument pDoc,
                                                                                          out int version,
                                                                                          out Boolean pbCurrent);
    
                                                            void GetMethodVersion(ISymUnmanagedMethod pMethod,
                                                                                          out int version);
                              };
}