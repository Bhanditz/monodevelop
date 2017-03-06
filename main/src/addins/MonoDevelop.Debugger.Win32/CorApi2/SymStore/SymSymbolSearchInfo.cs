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

    internal class SymSymbolSearchInfo : ISymbolSearchInfo
    {
        ISymUnmanagedSymbolSearchInfo m_target;

        public SymSymbolSearchInfo(ISymUnmanagedSymbolSearchInfo target)
        {
            m_target = target;
        }
        
        public int SearchPathLength
        {
            get 
            {
                int length;
                m_target.GetSearchPathLength(out length);
                return length;
            }
        }

        public String SearchPath
        {
            get 
            {
                int length;
                m_target.GetSearchPath(0, out length, null);
                StringBuilder path = new StringBuilder(length);
                m_target.GetSearchPath(length, out length, path);
                return path.ToString();
            }
        }

        public int HResult
        {
            get 
            {
                int hr;
                m_target.GetHRESULT(out hr);
                return hr;
            }
         }
      }

}
