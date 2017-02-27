using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugInternalFrameType
        {
            // This is a 'null' value for GetFrameType and is included for completeness sake.
            // ICorDebugInternalFrame::GetFrameType() should never actually return this.
            STUBFRAME_NONE = 0x00000000,

            // This frame is a M2U stub-frame. This could include both PInvoke
            // and COM-interop calls.
            STUBFRAME_M2U = 0x0000001,

            // This is a U2M stub frame.
            STUBFRAME_U2M = 0x0000002,

            // AppDomain transition.
            STUBFRAME_APPDOMAIN_TRANSITION = 0x00000003,

            // LightWeight method calls.
            STUBFRAME_LIGHTWEIGHT_FUNCTION = 0x00000004,

            // Start of Func-eval. This is included for CHF callbacks.
            // Funcevals also have a chain CHAIN_FUNC_EVAL (legacy from v1.0)
            STUBFRAME_FUNC_EVAL = 0x00000005,

            // Start of an internal call into the CLR.
            STUBFRAME_INTERNALCALL = 0x00000006,

            // start of a class initialization; corresponds to CHAIN_CLASS_INIT
            STUBFRAME_CLASS_INIT = 0x00000007,

            // an exception is thrown; corresponds to CHAIN_EXCEPTION_FILTER
            STUBFRAME_EXCEPTION = 0x00000008,

            // a frame used for code-access security purposes; corresponds to CHAIN_SECURITY
            STUBFRAME_SECURITY = 0x00000009,

            // a frame used to mark that the runtime is jitting a managed method
            STUBFRAME_JIT_COMPILATION = 0x0000000a,
        } CorDebugInternalFrameType;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugInternalFrameType
    {
        /// <summary>
        /// This is a 'null' value for GetFrameType and is included for completeness sake.
        /// ICorDebugInternalFrame::GetFrameType() should never actually return this.
        /// </summary>
        STUBFRAME_NONE,
        /// <summary>
        /// This frame is a M2U stub-frame. This could include both PInvoke and COM-interop calls.
        /// </summary>
        STUBFRAME_M2U,
        /// <summary>
        /// This is a U2M stub frame.
        /// </summary>
        STUBFRAME_U2M,
        /// <summary>
        /// AppDomain transition.
        /// </summary>
        STUBFRAME_APPDOMAIN_TRANSITION,
        /// <summary>
        /// LightWeight method calls.
        /// </summary>
        STUBFRAME_LIGHTWEIGHT_FUNCTION,
        /// <summary>
        /// Start of Func-eval. This is included for CHF callbacks.
        /// Funcevals also have a chain CHAIN_FUNC_EVAL (legacy from v1.0)
        /// </summary>
        STUBFRAME_FUNC_EVAL,
        /// <summary>
        /// Start of an internal call into the CLR.
        /// </summary>
        STUBFRAME_INTERNALCALL,
        /// <summary>
        /// start of a class initialization; corresponds to CHAIN_CLASS_INIT
        /// </summary>
        STUBFRAME_CLASS_INIT,
        /// <summary>
        /// an exception is thrown; corresponds to CHAIN_EXCEPTION_FILTER
        /// </summary>
        STUBFRAME_EXCEPTION,
        /// <summary>
        /// a frame used for code-access security purposes; corresponds to CHAIN_SECURITY
        /// </summary>
        STUBFRAME_SECURITY,
        /// <summary>
        /// a frame used to mark that the runtime is jitting a managed method
        /// </summary>
        STUBFRAME_JIT_COMPILATION,
    }
}