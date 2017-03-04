//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------
using System;
using System.Text;
using System.Diagnostics;
using CorApi.ComInterop;

namespace Microsoft.Samples.Debugging.CorDebug
{



    public sealed class CorHandleValue : ICorDebugReferenceValue, System.IDisposable
    {

        internal CorHandleValue(ICorDebugHandleValue handleValue) : base(handleValue)
        {
            m_handleVal = handleValue;
        }

        public void Dispose()
        {
            // The underlying ICorDebugHandle has a  Dispose() method which will free
            // its resources (a GC handle). We call that now to free things sooner.
            // If we don't call it now, it will still get freed at some random point after
            // the final release (which the finalizer will call).
            try
            {
                // This is just a best-effort to cleanup resources early.
                // If it fails, just swallow and move on.
                // May throw if handle was already disposed, or if process is not stopped.
                m_handleVal.Dispose();    
            }
            catch
            {
                // swallow all
            }
        }

        [CLSCompliant(false)]
        public CorDebugHandleType HandleType
        {
            get 
            {
                CorDebugHandleType ht;
                m_handleVal.GetHandleType(out ht);
                return ht;
            }
        }
        private ICorDebugHandleValue m_handleVal = null;
    }


    public unsafe sealed class ICorDebugGenericValue : ICorDebugValue
    {
        internal ICorDebugGenericValue(CorApi.ComInterop.ICorDebugGenericValue genericValue) : base(genericValue)
        {
            m_genVal = genericValue;
        }

        // Convert the supplied value to the type of this CorGenericValue using System.IConvertable.
        // Then store the value into this CorGenericValue.  Any compatible type can be supplied.
        // For example, if you supply a string and the underlying type is ELEMENT_TYPE_BOOLEAN,
        // Convert.ToBoolean will attempt to match the string against "true" and "false".
        public void SetValue(object value)
        {
            try
            {
                switch (this.Type)
                {
                    case CorElementType.ELEMENT_TYPE_BOOLEAN:
                        bool v = Convert.ToBoolean( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &v ) );
                        }
                        break;
                        
                    case CorElementType.ELEMENT_TYPE_I1:
                        SByte sbv = Convert.ToSByte( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &sbv ) );
                        }
                        break;

                    case CorElementType.ELEMENT_TYPE_U1:                
                        Byte bv = Convert.ToByte( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &bv ) );
                        }
                        break;
                        
                    case CorElementType.ELEMENT_TYPE_CHAR:              
                        Char cv = Convert.ToChar( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &cv ) );
                        }
                        break;
                        
                    case CorElementType.ELEMENT_TYPE_I2:
                        Int16 i16v = Convert.ToInt16( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &i16v ) );
                        }
                        break;

                    case CorElementType.ELEMENT_TYPE_U2:
                        UInt16 u16v = Convert.ToUInt16( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &u16v ) );
                        }
                        break;
                        
                    case CorElementType.ELEMENT_TYPE_I4:
                        Int32 i32v = Convert.ToInt32( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &i32v ) );
                        }
                        break;
                        
                    case CorElementType.ELEMENT_TYPE_U4:
                        UInt32 u32v = Convert.ToUInt32( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &u32v ) );
                        }
                        break;

                    case CorElementType.ELEMENT_TYPE_I:
                        Int64 ip64v = Convert.ToInt64( value );
                        IntPtr ipv = new IntPtr( ip64v );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &ipv ) );
                        }
                        break;

                    case CorElementType.ELEMENT_TYPE_U:         
                        UInt64 ipu64v = Convert.ToUInt64( value );
                        UIntPtr uipv = new UIntPtr( ipu64v );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &uipv ) );
                        }
                        break;

                    case CorElementType.ELEMENT_TYPE_I8:                
                        Int64 i64v = Convert.ToInt64( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &i64v ) );
                        }
                        break;
                        
                    case CorElementType.ELEMENT_TYPE_U8:
                        UInt64 u64v = Convert.ToUInt64( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &u64v ) );
                        }
                        break;

                    case CorElementType.ELEMENT_TYPE_R4:                                
                        Single sv = Convert.ToSingle( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &sv ) );
                        }
                         break;

                    case CorElementType.ELEMENT_TYPE_R8:                                                            
                        Double dv = Convert.ToDouble( value );
                        unsafe
                        {
                            SetValueInternal( new IntPtr( &dv ) );
                        }
                        break;  
                        
                    case CorElementType.ELEMENT_TYPE_VALUETYPE:                 
                        byte[] bav = (byte[]) value;
                        unsafe
                        {                       
                            fixed (byte* bufferPtr = &bav[0]) 
                            {
                                Debug.Assert(this.Size == bav.Length);          
                                m_genVal.SetValue(new IntPtr(bufferPtr));               
                            }
                        }
                        break;
                        
                    default:                
                        throw new InvalidOperationException("Type passed is not recognized.");
                }
            }
            catch (InvalidCastException e)
            {
                throw new InvalidOperationException("Wrong type used for SetValue command", e);
            }
        }
        
        public object GetValue()
        {
            return UnsafeGetValueAsType(this.Type);
        }

        /// <summary>
        /// Get the value as an array of IntPtrs.
        /// </summary>
        public IntPtr[] GetValueAsIntPtrArray()
        {
            int ptrsize=IntPtr.Size;            
            int cElem = (this.Size + ptrsize-1) / ptrsize;
            IntPtr[] buffer = new IntPtr[cElem];
            
            unsafe
            {
                fixed (IntPtr* bufferPtr = &buffer[0]) 
                {
                    this.GetValueInternal( new IntPtr(bufferPtr));               
                }
            }
            return buffer;
        }

        public Object UnsafeGetValueAsType(CorElementType type)
        {
            switch(type)
            {
                case CorElementType.ELEMENT_TYPE_BOOLEAN:
                    byte bValue=4; // just initialize to avoid compiler warnings
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(byte));
                        this.GetValueInternal(&bValue);
                    }
                    return (object) (bValue!=0);
                                    
                case CorElementType.ELEMENT_TYPE_CHAR:
                    char cValue='a'; // initialize to avoid compiler warnings
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(char));
                        this.GetValueInternal(&cValue);
                    }
                    return (object) cValue;

                case CorElementType.ELEMENT_TYPE_I1:
                    SByte i1Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(SByte));
                        this.GetValueInternal(&i1Value);
                    }
                    return (object) i1Value;
                    
                case CorElementType.ELEMENT_TYPE_U1:
                    Byte u1Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(Byte));
                        this.GetValueInternal(&u1Value);
                    }
                    return (object) u1Value;
                    
                case CorElementType.ELEMENT_TYPE_I2:
                    Int16 i2Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(Int16));
                        this.GetValueInternal(&i2Value);
                    }
                    return (object) i2Value;
                    
                case CorElementType.ELEMENT_TYPE_U2:
                    UInt16 u2Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(UInt16));
                        this.GetValueInternal(&u2Value);
                    }
                    return (object) u2Value;
                    
                case CorElementType.ELEMENT_TYPE_I:
                    IntPtr ipValue=IntPtr.Zero;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(IntPtr));
                        this.GetValueInternal(&ipValue);
                    }
                    return (object) ipValue;

                case CorElementType.ELEMENT_TYPE_U:
                    UIntPtr uipValue=UIntPtr.Zero;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(UIntPtr));
                        this.GetValueInternal(&uipValue);
                    }
                    return (object) uipValue;
                    
                case CorElementType.ELEMENT_TYPE_I4:
                    Int32 i4Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(Int32));
                        this.GetValueInternal(&i4Value);
                    }
                    return (object) i4Value;

                case CorElementType.ELEMENT_TYPE_U4:
                    UInt32 u4Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(UInt32));
                        this.GetValueInternal(&u4Value);
                    }
                    return (object) u4Value;

                case CorElementType.ELEMENT_TYPE_I8:
                    Int64 i8Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(Int64));
                        this.GetValueInternal(&i8Value);
                    }
                    return (object) i8Value;

                case CorElementType.ELEMENT_TYPE_U8:
                    UInt64 u8Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(UInt64));
                        this.GetValueInternal(&u8Value);
                    }
                    return (object) u8Value;

                case CorElementType.ELEMENT_TYPE_R4:
                    Single r4Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(Single));
                        this.GetValueInternal(&r4Value);
                    }
                    return (object) r4Value;

                case CorElementType.ELEMENT_TYPE_R8:
                    Double r8Value=4;
                    unsafe 
                    {
                        Debug.Assert(this.Size==sizeof(Double));
                        this.GetValueInternal(&r8Value);
                    }
                    return (object) r8Value;


                case CorElementType.ELEMENT_TYPE_VALUETYPE:
                    byte[] buffer = new byte[this.Size];
                    unsafe
                    {
                        fixed (byte* bufferPtr = &buffer[0]) 
                        {
                            Debug.Assert(this.Size == buffer.Length);           
                            this.GetValueInternal( bufferPtr);               
                        }
                    }
                    return buffer;
                    
                default:
                    Debug.Assert(false,"Generic value should not be of any other type");
                    throw new NotSupportedException();
            }
        }


        private void SetValueInternal(void* valPtr) 
        {
            m_genVal.SetValue(valPtr);
        }

        private void GetValueInternal( void* valPtr )
        {
            m_genVal.GetValue(valPtr);
        }
        
        private CorApi.ComInterop.ICorDebugGenericValue m_genVal = null;
    }


} /* namespace  */
