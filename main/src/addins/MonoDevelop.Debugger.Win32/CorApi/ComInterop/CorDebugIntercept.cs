using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <cordebug.idl>
        typedef enum CorDebugIntercept
        {
            INTERCEPT_NONE                = 0x0 ,
            INTERCEPT_CLASS_INIT          = 0x01,
            INTERCEPT_EXCEPTION_FILTER    = 0x02,
            INTERCEPT_SECURITY            = 0x04,
            INTERCEPT_CONTEXT_POLICY      = 0x08,
            INTERCEPT_INTERCEPTION        = 0x10,
            INTERCEPT_ALL                 = 0xffff
        } CorDebugIntercept;
    */
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorDebugIntercept
    {
        INTERCEPT_NONE = 0,
        INTERCEPT_CLASS_INIT = 1,
        INTERCEPT_EXCEPTION_FILTER = 2,
        INTERCEPT_SECURITY = 4,
        INTERCEPT_CONTEXT_POLICY = 8,
        INTERCEPT_INTERCEPTION = 16,
        INTERCEPT_ALL = 65535,
    }
}