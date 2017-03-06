//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System;
using System.Text;

namespace CorApi2.SymStore 
{
    // Interface does not need to be marked with the serializable attribute

    internal class SymConstant : ISymbolConstant
    {
        ISymUnmanagedConstant m_target;
        
        public SymConstant(ISymUnmanagedConstant target)
        {
            m_target = target;
        }
        
        public String GetName()
        {
            int count;
            m_target.GetName(0, out count, null);
            StringBuilder name = new StringBuilder(count);
            m_target.GetName(count, out count, name);
            return name.ToString();
        }
        
        public Object GetValue()
        {
            Object value = null;
            m_target.GetValue(out value);
            return value;
        }
         
        public byte[] GetSignature()
        {
            int count = 0;
            m_target.GetSignature(0, out count, null);
            byte[] sig = new byte[count];
            m_target.GetSignature(count, out count, sig);
            return sig;
        }
    }
}
