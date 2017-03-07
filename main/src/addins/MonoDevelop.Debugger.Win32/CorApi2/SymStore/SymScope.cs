//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System.Diagnostics.SymbolStore;

namespace CorApi2.SymStore 
{
    // Interface does not need to be marked with the serializable attribute

    internal class SymScope : ISymbolScope, ISymbolScope2
    {
        readonly ISymUnmanagedScope m_target;

        internal SymScope(ISymUnmanagedScope target)
        {
            m_target = target;
        }
        
        public ISymbolMethod Method 
        { 
            get
            {
                ISymUnmanagedMethod uMethod = null;
                m_target.GetMethod(out uMethod);
                return new SymMethod(uMethod);
            }
        }

        public ISymbolScope Parent
        { 
            get
            {
                ISymUnmanagedScope uScope = null;
                m_target.GetParent(out uScope);
                return new SymScope(uScope);
            }
        }

        public ISymbolScope[] GetChildren()
        {
            int count;
            m_target.GetChildren(0, out count, null);
            ISymUnmanagedScope[] uScopes = new ISymUnmanagedScope[count];
            m_target.GetChildren(count, out count, uScopes);
            
            int i;
            ISymbolScope[] scopes = new ISymbolScope[count];
            for (i = 0; i < count; i++)
            {
                scopes[i] = new SymScope(uScopes[i]);
            }
            return scopes;
        }

        public int StartOffset
        { 
            get
            {
                int offset;
                m_target.GetStartOffset(out offset);
                return offset;
            }
        }
        

        public int EndOffset
        { 
            get
            {
                int offset;
                m_target.GetEndOffset(out offset);
                return offset;
            }
        }

        public ISymbolVariable[] GetLocals()
        {
            int count;
            m_target.GetLocals(0, out count, null);
            ISymUnmanagedVariable[] uVariables = new ISymUnmanagedVariable[count];
            m_target.GetLocals(count, out count, uVariables);
            
            int i;
            ISymbolVariable[] variables = new ISymbolVariable[count];
            for (i = 0; i < count; i++)
            {
                variables[i] = new SymVariable(uVariables[i]);
            }
            return variables;
        }

        public ISymbolNamespace[] GetNamespaces()
        {
            int count;
            m_target.GetNamespaces(0, out count, null);
            ISymUnmanagedNamespace[] uNamespaces = new ISymUnmanagedNamespace[count];
            m_target.GetNamespaces(count, out count, uNamespaces);
            
            int i;
            ISymbolNamespace[] namespaces = new ISymbolNamespace[count];
            for (i = 0; i < count; i++)
            {
                namespaces[i] = new SymNamespace(uNamespaces[i]);
            }
            return namespaces;
        }

        public int LocalCount
        {
            get
            {
                int count;
                m_target.GetLocalCount(out count);
                return count;
            }
        }
        
        public int ConstantCount
        {
            get
            {
                int count;
                ((ISymUnmanagedScope2)m_target).GetConstantCount(out count);
                return count;
            }
        }
        
        public ISymbolConstant[] GetConstants()
        {
            int count;
            ((ISymUnmanagedScope2)m_target).GetConstants(0, out count, null);
            ISymUnmanagedConstant[] uConstants = new ISymUnmanagedConstant[count];
            ((ISymUnmanagedScope2)m_target).GetConstants(count, out count, uConstants);
            
            int i;
            ISymbolConstant[] Constants = new ISymbolConstant[count];
            for (i = 0; i < count; i++)
            {
                Constants[i] = new SymConstant(uConstants[i]);
            }
            return Constants;
        }
    

    }
}
