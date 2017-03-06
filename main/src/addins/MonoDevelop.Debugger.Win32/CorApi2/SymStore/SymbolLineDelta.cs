//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------


// These interfaces serve as an extension to the BCL's SymbolStore interfaces.

using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace CorApi2.SymStore 
{
    // Interface does not need to be marked with the serializable attribute

    /// <include file='doc\ISymENCUpdate.uex' path='docs/doc[@for="SymbolLineDelta"]/*' />
    [StructLayout(LayoutKind.Sequential)]
    public struct SymbolLineDelta
    {
        SymbolToken mdMethod;
        int delta;
    };
}
