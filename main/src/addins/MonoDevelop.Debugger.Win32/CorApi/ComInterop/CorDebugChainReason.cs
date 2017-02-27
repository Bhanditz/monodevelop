using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugChainReason
        {
            // Note that the first five line up with CorDebugIntercept
            CHAIN_NONE              = 0x000,
            CHAIN_CLASS_INIT        = 0x001,
            CHAIN_EXCEPTION_FILTER  = 0x002,
            CHAIN_SECURITY          = 0x004,
            CHAIN_CONTEXT_POLICY    = 0x008,
            CHAIN_INTERCEPTION      = 0x010,
            CHAIN_PROCESS_START     = 0x020,
            CHAIN_THREAD_START      = 0x040,
            CHAIN_ENTER_MANAGED     = 0x080,
            CHAIN_ENTER_UNMANAGED   = 0x100,
            CHAIN_DEBUGGER_EVAL     = 0x200,
            CHAIN_CONTEXT_SWITCH    = 0x400,
            CHAIN_FUNC_EVAL         = 0x800,
         } CorDebugChainReason;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugChainReason
    {
        CHAIN_NONE = 0,
        CHAIN_CLASS_INIT = 1,
        CHAIN_EXCEPTION_FILTER = 2,
        CHAIN_SECURITY = 4,
        CHAIN_CONTEXT_POLICY = 8,
        CHAIN_INTERCEPTION = 16,
        CHAIN_PROCESS_START = 32,
        CHAIN_THREAD_START = 64,
        CHAIN_ENTER_MANAGED = 128,
        CHAIN_ENTER_UNMANAGED = 256,
        CHAIN_DEBUGGER_EVAL = 512,
        CHAIN_CONTEXT_SWITCH = 1024,
        CHAIN_FUNC_EVAL = 2048,
    }
}