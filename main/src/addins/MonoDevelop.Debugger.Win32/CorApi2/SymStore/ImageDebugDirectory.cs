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
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDebugDirectory {
        int     Characteristics;
        int     TimeDateStamp;
        short   MajorVersion;
        short   MinorVersion;
        int     Type;
        int     SizeOfData;
        int     AddressOfRawData;
        int     PointerToRawData;

        public override string ToString()
        {
            return String.Format( @"Characteristics: {0}
TimeDateStamp: {1}
MajorVersion: {2}
MinorVersion: {3}
Type: {4}
SizeOfData: {5}
AddressOfRawData: {6}
PointerToRawData: {7}
", 
                      Characteristics, 
                      TimeDateStamp, 
                      MajorVersion, 
                      MinorVersion, 
                      Type, 
                      SizeOfData, 
                      AddressOfRawData, 
                      PointerToRawData);
        }
    };
}
