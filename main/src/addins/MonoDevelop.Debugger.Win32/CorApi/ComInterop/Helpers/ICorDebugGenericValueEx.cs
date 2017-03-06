using System;
using System.Diagnostics;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugGenericValueEx
    {
        public static object GetValue(this ICorDebugGenericValue corvalue, CorElementType type)
        {
            uint dwSize;
            corvalue.GetSize(&dwSize).AssertSucceeded("Could not get the value size.");
            switch(type)
            {
            case CorElementType.ELEMENT_TYPE_BOOLEAN:
                byte bValue = 4; // just initialize to avoid compiler warnings
                Debug.Assert(dwSize == sizeof(byte));
                corvalue.GetValue(&bValue).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return bValue != 0;

            case CorElementType.ELEMENT_TYPE_CHAR:
                char cValue = 'a'; // initialize to avoid compiler warnings
                Debug.Assert(dwSize == sizeof(char));
                corvalue.GetValue(&cValue).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return cValue;

            case CorElementType.ELEMENT_TYPE_I1:
                sbyte i1Value = 4;
                Debug.Assert(dwSize == sizeof(sbyte));
                corvalue.GetValue(&i1Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return i1Value;

            case CorElementType.ELEMENT_TYPE_U1:
                byte u1Value = 4;
                Debug.Assert(dwSize == sizeof(byte));
                corvalue.GetValue(&u1Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return u1Value;

            case CorElementType.ELEMENT_TYPE_I2:
                short i2Value = 4;
                Debug.Assert(dwSize == sizeof(short));
                corvalue.GetValue(&i2Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return i2Value;

            case CorElementType.ELEMENT_TYPE_U2:
                ushort u2Value = 4;
                Debug.Assert(dwSize == sizeof(ushort));
                corvalue.GetValue(&u2Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return u2Value;

            case CorElementType.ELEMENT_TYPE_I:
                IntPtr ipValue = IntPtr.Zero;
                Debug.Assert(dwSize == sizeof(IntPtr));
                corvalue.GetValue(&ipValue).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return ipValue;

            case CorElementType.ELEMENT_TYPE_U:
                UIntPtr uipValue = UIntPtr.Zero;
                Debug.Assert(dwSize == sizeof(UIntPtr));
                corvalue.GetValue(&uipValue).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return uipValue;

            case CorElementType.ELEMENT_TYPE_I4:
                int i4Value = 4;
                Debug.Assert(dwSize == sizeof(int));
                corvalue.GetValue(&i4Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return i4Value;

            case CorElementType.ELEMENT_TYPE_U4:
                uint u4Value = 4;
                Debug.Assert(dwSize == sizeof(uint));
                corvalue.GetValue(&u4Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return u4Value;

            case CorElementType.ELEMENT_TYPE_I8:
                long i8Value = 4;
                Debug.Assert(dwSize == sizeof(long));
                corvalue.GetValue(&i8Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return i8Value;

            case CorElementType.ELEMENT_TYPE_U8:
                ulong u8Value = 4;
                Debug.Assert(dwSize == sizeof(ulong));
                corvalue.GetValue(&u8Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return u8Value;

            case CorElementType.ELEMENT_TYPE_R4:
                float r4Value = 4;
                Debug.Assert(dwSize == sizeof(float));
                corvalue.GetValue(&r4Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return r4Value;

            case CorElementType.ELEMENT_TYPE_R8:
                double r8Value = 4;
                Debug.Assert(dwSize == sizeof(double));
                corvalue.GetValue(&r8Value).AssertSucceeded($"Could not get the debugger value as type {type}.");
                return r8Value;

            case CorElementType.ELEMENT_TYPE_VALUETYPE:
                var buffer = new byte[dwSize];
                fixed(byte* bufferPtr = &buffer[0])
                {
                    Debug.Assert(dwSize == buffer.Length);
                    corvalue.GetValue(bufferPtr).AssertSucceeded($"Could not get the debugger value as type {type}.");
                }
                return buffer;

            default:
                Debug.Assert(false, "Generic value should not be of any other type");
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Convert the supplied value to the type of this CorGenericValue using System.IConvertable.
        /// Then store the value into this CorGenericValue.  Any compatible type can be supplied.
        /// For example, if you supply a string and the underlying type is ELEMENT_TYPE_BOOLEAN,
        /// Convert.ToBoolean will attempt to match the string against "true" and "false".
        /// </summary>
        /// <param name="corvalue"></param>
        /// <param name="value"></param>
        public static void SetValue(this ICorDebugGenericValue corvalue, object value)
        {
            CorElementType type;
            corvalue.GetType(&type).AssertSucceeded("Cound not get the value type.");
            try
            {
                switch(type)
                {
                case CorElementType.ELEMENT_TYPE_BOOLEAN:
                    bool v = Convert.ToBoolean(value);
                    corvalue.SetValue(&v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_I1:
                    sbyte sbv = Convert.ToSByte(value);
                    corvalue.SetValue(&sbv).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_U1:
                    byte bv = Convert.ToByte(value);
                    corvalue.SetValue(&bv).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_CHAR:
                    char cv = Convert.ToChar(value);
                    corvalue.SetValue(&cv).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_I2:
                    short i16v = Convert.ToInt16(value);
                    corvalue.SetValue(&i16v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_U2:
                    ushort u16v = Convert.ToUInt16(value);
                    corvalue.SetValue(&u16v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_I4:
                    int i32v = Convert.ToInt32(value);
                    corvalue.SetValue(&i32v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_U4:
                    uint u32v = Convert.ToUInt32(value);
                    corvalue.SetValue(&u32v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_I:
                    long ip64v = Convert.ToInt64(value);
                    corvalue.SetValue(&ip64v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_U:
                    ulong ipu64v = Convert.ToUInt64(value);
                    var uipv = new UIntPtr(ipu64v);
                    corvalue.SetValue(&uipv).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_I8:
                    long i64v = Convert.ToInt64(value);
                    corvalue.SetValue(&i64v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_U8:
                    ulong u64v = Convert.ToUInt64(value);
                    corvalue.SetValue(&u64v).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_R4:
                    float sv = Convert.ToSingle(value);
                    corvalue.SetValue(&sv).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_R8:
                    double dv = Convert.ToDouble(value);
                    corvalue.SetValue(&dv).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                case CorElementType.ELEMENT_TYPE_VALUETYPE:
                    var bav = (byte[])value;
                    uint dwSize = 0;
                    corvalue.GetSize(&dwSize).AssertSucceeded("For a value of type ELEMENT_TYPE_VALUETYPE, could not get its size.");
                    if(bav.Length != dwSize)
                        throw new ArgumentOutOfRangeException(nameof(value), value, $"For a value of type ELEMENT_TYPE_VALUETYPE, the size of the user-supplied buffer {bav.Length:N0} does not match the value size {dwSize:N0}.");
                    fixed(byte* bufferPtr = &bav[0])
                        corvalue.SetValue(bufferPtr).AssertSucceeded($"Could not set the debugger value of type {type} to value “{value}”.");
                    break;

                default:
                    throw new InvalidOperationException($"Do not know how to set debugger values of type {type}.");
                }
            }
            catch(InvalidCastException e)
            {
                throw new InvalidOperationException($"The user-supplied value of type {(value != null ? value.GetType().FullName : "<NULL>")} could not be assigned to the debugger value of type {type}.", e);
            }
        }
    }
}