using System;
using System.Diagnostics.CodeAnalysis;

namespace CorApi.ComInterop
{
    /*  from: <corhdr.h>
        typedef enum CorElementType
        {
            ELEMENT_TYPE_END            = 0x00,
            ELEMENT_TYPE_VOID           = 0x01,
            ELEMENT_TYPE_BOOLEAN        = 0x02,
            ELEMENT_TYPE_CHAR           = 0x03,
            ELEMENT_TYPE_I1             = 0x04,
            ELEMENT_TYPE_U1             = 0x05,
            ELEMENT_TYPE_I2             = 0x06,
            ELEMENT_TYPE_U2             = 0x07,
            ELEMENT_TYPE_I4             = 0x08,
            ELEMENT_TYPE_U4             = 0x09,
            ELEMENT_TYPE_I8             = 0x0a,
            ELEMENT_TYPE_U8             = 0x0b,
            ELEMENT_TYPE_R4             = 0x0c,
            ELEMENT_TYPE_R8             = 0x0d,
            ELEMENT_TYPE_STRING         = 0x0e,

            // every type above PTR will be simple type
            ELEMENT_TYPE_PTR            = 0x0f,     // PTR &lt;type&gt;
            ELEMENT_TYPE_BYREF          = 0x10,     // BYREF &lt;type&gt;

            // Please use ELEMENT_TYPE_VALUETYPE. ELEMENT_TYPE_VALUECLASS is deprecated.
            ELEMENT_TYPE_VALUETYPE      = 0x11,     // VALUETYPE &lt;class Token&gt;
            ELEMENT_TYPE_CLASS          = 0x12,     // CLASS &lt;class Token&gt;
            ELEMENT_TYPE_VAR            = 0x13,     // a class type variable VAR &lt;number&lt;
            ELEMENT_TYPE_ARRAY          = 0x14,     // MDARRAY &lt;type&gt; &lt;rank&gt; &lt;bcount&gt; &lt;bound1&gt; ... &lt;lbcount&gt; &lt;lb1&gt; ...
            ELEMENT_TYPE_GENERICINST    = 0x15,     // GENERICINST &lt;generic type&gt; &lt;argCnt&gt; &lt;arg1&gt; ... &lt;argn&gt;
            ELEMENT_TYPE_TYPEDBYREF     = 0x16,     // TYPEDREF  (it takes no args) a typed referece to some other type

            ELEMENT_TYPE_I              = 0x18,     // native integer size
            ELEMENT_TYPE_U              = 0x19,     // native unsigned integer size
            ELEMENT_TYPE_FNPTR          = 0x1b,     // FNPTR &lt;complete sig for the function including calling convention&gt;
            ELEMENT_TYPE_OBJECT         = 0x1c,     // Shortcut for System.Object
            ELEMENT_TYPE_SZARRAY        = 0x1d,     // Shortcut for single dimension zero lower bound array
            // SZARRAY &lt;type&gt;
            ELEMENT_TYPE_MVAR           = 0x1e,     // a method type variable MVAR &lt;number&gt;

            // This is only for binding
            ELEMENT_TYPE_CMOD_REQD      = 0x1f,     // required C modifier : E_T_CMOD_REQD &lt;mdTypeRef/mdTypeDef&gt;
            ELEMENT_TYPE_CMOD_OPT       = 0x20,     // optional C modifier : E_T_CMOD_OPT &lt;mdTypeRef/mdTypeDef&gt;

            // This is for signatures generated internally (which will not be persisted in any way).
            ELEMENT_TYPE_INTERNAL       = 0x21,     // INTERNAL &lt;typehandle&gt;

            // Note that this is the max of base type excluding modifiers
            ELEMENT_TYPE_MAX            = 0x22,     // first invalid element type


            ELEMENT_TYPE_MODIFIER       = 0x40,
            ELEMENT_TYPE_SENTINEL       = 0x01 | ELEMENT_TYPE_MODIFIER, // sentinel for varargs
            ELEMENT_TYPE_PINNED         = 0x05 | ELEMENT_TYPE_MODIFIER,

        } CorElementType;
    */
    [CLSCompliant (true)]
    [Flags]
    [SuppressMessage ("ReSharper", "InconsistentNaming")]
    public enum CorElementType : uint
    {
        ELEMENT_TYPE_END            = 0x00,
        ELEMENT_TYPE_VOID           = 0x01,
        ELEMENT_TYPE_BOOLEAN        = 0x02,
        ELEMENT_TYPE_CHAR           = 0x03,
        ELEMENT_TYPE_I1             = 0x04,
        ELEMENT_TYPE_U1             = 0x05,
        ELEMENT_TYPE_I2             = 0x06,
        ELEMENT_TYPE_U2             = 0x07,
        ELEMENT_TYPE_I4             = 0x08,
        ELEMENT_TYPE_U4             = 0x09,
        ELEMENT_TYPE_I8             = 0x0a,
        ELEMENT_TYPE_U8             = 0x0b,
        ELEMENT_TYPE_R4             = 0x0c,
        ELEMENT_TYPE_R8             = 0x0d,
        ELEMENT_TYPE_STRING         = 0x0e,
        /// <summary>
        /// PTR &lt;type&gt;
        /// </summary>
        ELEMENT_TYPE_PTR            = 0x0f,
        /// <summary>
        /// BYREF &lt;type&gt;
        /// </summary>
        ELEMENT_TYPE_BYREF          = 0x10,
        /// <summary>
        /// VALUETYPE &lt;class Token&gt;
        /// </summary>
        [Obsolete("Please use ELEMENT_TYPE_VALUETYPE. ELEMENT_TYPE_VALUECLASS is deprecated.")]
        ELEMENT_TYPE_VALUETYPE      = 0x11,
        /// <summary>
        /// CLASS &lt;class Token&gt;
        /// </summary>
        [Obsolete]
        ELEMENT_TYPE_CLASS          = 0x12,
        /// <summary>
        /// a class type variable VAR &lt;number&gt;
        /// </summary>
        ELEMENT_TYPE_VAR            = 0x13,
        /// <summary>
        /// MDARRAY &lt;type&gt; &lt;rank&gt; &lt;bcount&gt; &lt;bound1&gt; ... &lt;lbcount&gt; &lt;lb1&gt; ...
        /// </summary>
        ELEMENT_TYPE_ARRAY          = 0x14,
        /// <summary>
        /// GENERICINST &lt;generic type&gt; &lt;argCnt&gt; &lt;arg1&gt; ... &lt;argn&gt;
        /// </summary>
        ELEMENT_TYPE_GENERICINST    = 0x15,
        /// <summary>
        /// TYPEDREF  (it takes no args) a typed referece to some other type
        /// </summary>
        ELEMENT_TYPE_TYPEDBYREF     = 0x16,
        /// <summary>
        /// native integer size
        /// </summary>
        ELEMENT_TYPE_I              = 0x18,
        /// <summary>
        /// native unsigned integer size
        /// </summary>
        ELEMENT_TYPE_U              = 0x19,
        /// <summary>
        /// FNPTR &lt;complete sig for the function including calling convention&gt;
        /// </summary>
        ELEMENT_TYPE_FNPTR          = 0x1b,
        /// <summary>
        /// Shortcut for System.Object
        /// </summary>
        ELEMENT_TYPE_OBJECT         = 0x1c,
        /// <summary>
        /// Shortcut for single dimension zero lower bound array
        /// </summary>
        ELEMENT_TYPE_SZARRAY        = 0x1d,
        /// <summary>
        /// SZARRAY &lt;type&gt; a method type variable MVAR &lt;number&gt;
        /// </summary>
        ELEMENT_TYPE_MVAR           = 0x1e,
        // This is only for binding
        /// <summary>
        /// This is only for binding required C modifier : E_T_CMOD_REQD &lt;mdTypeRef/mdTypeDef&gt;
        /// </summary>
        ELEMENT_TYPE_CMOD_REQD      = 0x1f,
        /// <summary>
        /// This is only for binding optional C modifier : E_T_CMOD_OPT &lt;mdTypeRef/mdTypeDef&gt;
        /// </summary>
        ELEMENT_TYPE_CMOD_OPT       = 0x20,
        /// <summary>
        /// This is for signatures generated internally (which will not be persisted in any way). INTERNAL &lt;typehandle&gt;
        /// </summary>
        ELEMENT_TYPE_INTERNAL       = 0x21,
        /// <summary>
        /// first invalid element type
        /// </summary>
        ELEMENT_TYPE_MAX            = 0x22,
        ELEMENT_TYPE_MODIFIER       = 0x40,
        /// <summary>
        /// sentinel for varargs
        /// </summary>
        ELEMENT_TYPE_SENTINEL       = 0x01 | ELEMENT_TYPE_MODIFIER,
        ELEMENT_TYPE_PINNED         = 0x05 | ELEMENT_TYPE_MODIFIER,
    }
}