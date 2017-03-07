//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System;
using System.Diagnostics.SymbolStore;

namespace CorApi2.SymStore 
{
    // Interface does not need to be marked with the serializable attribute

    internal class SymDocumentWriter: ISymbolDocumentWriter
    {
        readonly ISymUnmanagedDocumentWriter m_unmanagedDocumentWriter;
        
        public SymDocumentWriter(ISymUnmanagedDocumentWriter unmanagedDocumentWriter)
        {
            m_unmanagedDocumentWriter = unmanagedDocumentWriter;
        }
        
        public void SetSource(byte[] source)
        {
            m_unmanagedDocumentWriter.SetSource(source.Length, source);
        }

        public void SetCheckSum(Guid algorithmId, byte[] checkSum)
        {
            m_unmanagedDocumentWriter.SetCheckSum(algorithmId, checkSum.Length, checkSum);
        }

        // Public API
        internal ISymUnmanagedDocumentWriter InternalDocumentWriter
        {
            get
            {
                return m_unmanagedDocumentWriter;
            }
        }
                                      
 
    }
}
