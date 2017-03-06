namespace CorApi2.Metadata
{
    public enum CorCallingConvention
    {
        Default       = 0x0,

        VarArg        = 0x5,
        Field         = 0x6,
        LocalSig     = 0x7,
        Property      = 0x8,
        Unmanaged         = 0x9,
        GenericInst   = 0xa,  // generic method instantiation
        NativeVarArg  = 0xb,  // used ONLY for 64bit vararg PInvoke calls

        // The high bits of the calling convention convey additional info
        Mask      = 0x0f,  // Calling convention is bottom 4 bits
        HasThis   = 0x20,  // Top bit indicates a 'this' parameter
        ExplicitThis = 0x40,  // This parameter is explicitly in the signature
        Generic   = 0x10,  // Generic method sig with explicit number of type arguments (precedes ordinary parameter count)
    };
}