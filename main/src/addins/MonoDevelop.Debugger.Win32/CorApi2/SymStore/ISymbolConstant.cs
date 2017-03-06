//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System;
using System.Runtime.InteropServices;

namespace CorApi2.SymStore 
{
    // Interface does not need to be marked with the serializable attribute
    // Interface is returned by ISymbolScope2.GetConstants() so must be public
    [
        ComVisible(false)
    ]
    public interface ISymbolConstant
    {
        String GetName();

        Object GetValue();

        byte[] GetSignature();
    }
}
