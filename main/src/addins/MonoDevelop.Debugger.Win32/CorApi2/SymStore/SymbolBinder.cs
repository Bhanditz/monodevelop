//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CorApi2.SymStore 
{
    /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder"]/*' />

    public unsafe class SymbolBinder: ISymbolBinder1, ISymbolBinder2
    {
        ISymUnmanagedBinder m_binder;

        /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder.SymbolBinder"]/*' />
        public SymbolBinder()
        {
            Guid CLSID_CorSymBinder = new Guid("0A29FF9E-7F9C-4437-8B11-F424491E3931");
            m_binder = (ISymUnmanagedBinder3)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_CorSymBinder));
        }
        
        /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder.GetReader"]/*' />
        public ISymbolReader GetReader(IntPtr importer, String filename,
                                          String searchPath)
        {
            ISymUnmanagedReader reader = null;
            int hr = m_binder.GetReaderForFile(importer, filename, searchPath, out reader);
            if (IsFailingResultNormal(hr))
            {
                return null;
            }
            Marshal.ThrowExceptionForHR(hr);
            return new SymReader(reader);
        }

        /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder.GetReaderForFile"]/*' />
        public ISymbolReader GetReaderForFile(Object importer, String filename,
                                           String searchPath)
        {
            ISymUnmanagedReader reader = null;
            IntPtr uImporter = IntPtr.Zero;
            try
            {
                uImporter = Marshal.GetIUnknownForObject(importer);
                int hr = m_binder.GetReaderForFile(uImporter, filename, searchPath, out reader);
                if (IsFailingResultNormal(hr))
                {
                    return null;
                }
                Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                if (uImporter != IntPtr.Zero)
                    Marshal.Release(uImporter);
            }
            return new SymReader(reader);
        }
        
        /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder.GetReaderForFile1"]/*' />
        public ISymbolReader GetReaderForFile(Object importer, String fileName,
                                           String searchPath, SymSearchPolicies searchPolicy)
        {
            ISymUnmanagedReader symReader = null;
            IntPtr uImporter = IntPtr.Zero;
            try
            {
                uImporter = Marshal.GetIUnknownForObject(importer);
                int hr = ((ISymUnmanagedBinder2)m_binder).GetReaderForFile2(uImporter, fileName, searchPath, (int)searchPolicy, out symReader);
                if (IsFailingResultNormal(hr))
                {
                    return null;
                }
                Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                if (uImporter != IntPtr.Zero)
                    Marshal.Release(uImporter);
            }
            return new SymReader(symReader);
        }
        
        /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder.GetReaderForFile2"]/*' />
        public ISymbolReader GetReaderForFile(Object importer, String fileName,
                                           String searchPath, SymSearchPolicies searchPolicy,
                                           IntPtr callback)
        {
            ISymUnmanagedReader reader = null;
            IntPtr uImporter = IntPtr.Zero;
            try
            {
                uImporter = Marshal.GetIUnknownForObject(importer);
                int hr = ((ISymUnmanagedBinder3)m_binder).GetReaderFromCallback(uImporter, fileName, searchPath, (int)searchPolicy, callback, out reader);
                if (IsFailingResultNormal(hr))
                {
                    return null;
                }
                Marshal.ThrowExceptionForHR(hr);
            }
            finally {
                if (uImporter != IntPtr.Zero)
                    Marshal.Release(uImporter);
            }
            return new SymReader(reader);
        }
        
        /// <include file='doc\symbinder.uex' path='docs/doc[@for="SymbolBinder.GetReaderFromStream"]/*' />
        public ISymbolReader GetReaderFromStream(Object importer, IStream stream)
        {
            ISymUnmanagedReader reader = null;
            IntPtr uImporter = IntPtr.Zero;
            try
            {
                uImporter = Marshal.GetIUnknownForObject(importer);
                int hr = ((ISymUnmanagedBinder2)m_binder).GetReaderFromStream(uImporter, stream, out reader);
                if (IsFailingResultNormal(hr))
                {
                    return null;
                }
                Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                if (uImporter != IntPtr.Zero)
                    Marshal.Release(uImporter);
            }
            return new SymReader(reader);
        }

        private static bool IsFailingResultNormal(int hr)
        {
            // If a pdb is not found, that's a pretty common thing.
            if (hr == unchecked((int)0x806D0005))   // E_PDB_NOT_FOUND
            {
                return true;
            }
            // Other fairly common things may happen here, but we don't want to hide
            // this from the programmer.
            // You may get 0x806D0014 if the pdb is there, but just old (mismatched)
            // Or if you ask for the symbol information on something that's not an assembly.
            // If that may happen for your application, wrap calls to GetReaderForFile in 
            // try-catch(COMException) blocks and use the error code in the COMException to report error.
            return false;
        }
    }
}
