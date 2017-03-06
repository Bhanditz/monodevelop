//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System;
using System.Diagnostics.SymbolStore;
using System.Text;

namespace CorApi2.SymStore 
{
    // Interface does not need to be marked with the serializable attribute

    internal class SymNamespace : ISymbolNamespace
    {
        ISymUnmanagedNamespace m_unmanagedNamespace;
        
        internal SymNamespace(ISymUnmanagedNamespace nameSpace)
        {
            m_unmanagedNamespace = nameSpace;
        }
        
        public String Name 
        { 
            get
            {
                StringBuilder Name;
                int cchName = 0;
                m_unmanagedNamespace.GetName(0, out cchName, null);
                Name = new StringBuilder(cchName);
                m_unmanagedNamespace.GetName(cchName, out cchName, Name);
                return Name.ToString();
            }
        }

        public ISymbolNamespace[] GetNamespaces()
        {
            uint i;
            int cNamespaces = 0;
            m_unmanagedNamespace.GetNamespaces(0, out cNamespaces, null);
            ISymUnmanagedNamespace[] unmamagedNamespaces = new ISymUnmanagedNamespace[cNamespaces];
            m_unmanagedNamespace.GetNamespaces(cNamespaces, out cNamespaces, unmamagedNamespaces);
            
            ISymbolNamespace[] Namespaces = new ISymbolNamespace[cNamespaces];
            for (i = 0; i < cNamespaces; i++)
            {
                Namespaces[i] = new SymNamespace(unmamagedNamespaces[i]);
            }
            return Namespaces;
        }

        public ISymbolVariable[] GetVariables()
        {
            int cVars = 0;
            uint i;
            m_unmanagedNamespace.GetVariables(0, out cVars, null);
            ISymUnmanagedVariable[] unmanagedVariables = new ISymUnmanagedVariable[cVars];
            m_unmanagedNamespace.GetVariables(cVars, out cVars, unmanagedVariables);

            ISymbolVariable[] Variables = new ISymbolVariable[cVars];
            for (i = 0; i < cVars; i++)
            {
                Variables[i] = new SymVariable(unmanagedVariables[i]);
            }
            return Variables;
        }
    }
}
