//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using CorApi;
using CorApi.ComInterop;

using CorApi2.Extensions;

namespace CorApi2.Metadata
{
    public sealed class MetadataFieldInfo : FieldInfo
    {
        internal MetadataFieldInfo(IMetadataImport importer,uint fieldToken, MetadataType declaringType)
        {
            m_importer = importer;
            m_fieldToken = fieldToken;
            m_declaringType = declaringType;

            // Initialize
            int mdTypeDef;
            int pchField,pcbSigBlob,pdwCPlusTypeFlab,pcchValue, pdwAttr;
            IntPtr ppvSigBlob;
            IntPtr ppvRawValue;
            m_importer.GetFieldProps(m_fieldToken,
                                     out mdTypeDef,
                                     null,
                                     0,
                                     out pchField,
                                     out pdwAttr,
                                     out ppvSigBlob,
                                     out pcbSigBlob,
                                     out pdwCPlusTypeFlab,
                                     out ppvRawValue,
                                     out pcchValue
                                     );
            
            StringBuilder szField = new StringBuilder(pchField);
            m_importer.GetFieldProps(m_fieldToken,
                                     out mdTypeDef,
                                     szField,
                                     szField.Capacity,
                                     out pchField,
                                     out pdwAttr,
                                     out ppvSigBlob,
                                     out pcbSigBlob,
                                     out pdwCPlusTypeFlab,
                                     out ppvRawValue,
                                     out pcchValue
                                     );
            m_fieldAttributes = (FieldAttributes)pdwAttr;
            m_name = szField.ToString();

            // Get the values for static literal fields with primitive types
            FieldAttributes staticLiteralField = FieldAttributes.Static | FieldAttributes.HasDefault | FieldAttributes.Literal;
            if ((m_fieldAttributes & staticLiteralField) == staticLiteralField)
            {
                m_value = ParseDefaultValue(declaringType,ppvSigBlob,ppvRawValue, pcchValue);
            }
			// [Xamarin] Expression evaluator.
			MetadataHelperFunctionsExtensions.GetCustomAttribute (m_importer, m_fieldToken, typeof (DebuggerBrowsableAttribute));
        }

        private static object ParseDefaultValue(MetadataType declaringType, IntPtr ppvSigBlob, IntPtr ppvRawValue, int rawValueSize)
        {
                IntPtr ppvSigTemp = ppvSigBlob;
                CorCallingConvention callingConv = MetadataHelperFunctions.CorSigUncompressCallingConv(ref ppvSigTemp);
                Debug.Assert(callingConv == CorCallingConvention.Field);

                CorElementType elementType = MetadataHelperFunctions.CorSigUncompressElementType(ref ppvSigTemp);
#pragma warning disable 618
                if (elementType == CorElementType.ELEMENT_TYPE_VALUETYPE)
#pragma warning restore 618
                {
                        uint token = MetadataHelperFunctions.CorSigUncompressToken(ref ppvSigTemp);

                        if (token == declaringType.MetadataToken)
                        {
                            // Static literal field of the same type as the enclosing type
                            // may be one of the value fields of an enum
                            if (declaringType.ReallyIsEnum)
                            {
                                // If so, the value will be of the enum's underlying type,
                                // so we change it from VALUETYPE to be that type so that
                                // the following code will get the value
                                elementType = declaringType.EnumUnderlyingType;
                            }                           
                        }
                }

                switch (elementType)
                {
                    case CorElementType.ELEMENT_TYPE_CHAR:
                        return (char)Marshal.ReadByte(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_I1:
                        return (sbyte)Marshal.ReadByte(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_U1:
                        return Marshal.ReadByte(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_I2:
                        return Marshal.ReadInt16(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_U2:
                        return (ushort)Marshal.ReadInt16(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_I4:
                        return Marshal.ReadInt32(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_U4:
                        return (uint)Marshal.ReadInt32(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_I8:
                        return Marshal.ReadInt64(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_U8:
                        return (ulong)Marshal.ReadInt64(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_I:
                        return Marshal.ReadIntPtr(ppvRawValue);
                    case CorElementType.ELEMENT_TYPE_STRING:
                        return Marshal.PtrToStringAuto (ppvRawValue, rawValueSize);
                    case CorElementType.ELEMENT_TYPE_R4:
                        unsafe {
                            return *(float*) ppvRawValue.ToPointer ();
                        }
                    case CorElementType.ELEMENT_TYPE_R8:
                        unsafe {
                            return *(double*) ppvRawValue.ToPointer ();
                        }
                    case CorElementType.ELEMENT_TYPE_BOOLEAN:
                        unsafe {
                            return *(bool*) ppvRawValue.ToPointer ();
                        }

                    default:
                        return null;
                }
        }

        public override Object GetValue(Object obj)
        {
            FieldAttributes staticLiteralField = FieldAttributes.Static | FieldAttributes.HasDefault | FieldAttributes.Literal;
            if ((m_fieldAttributes & staticLiteralField) != staticLiteralField)
            {
                throw new InvalidOperationException("Field is not a static literal field.");
            }
            if (m_value == null)
            {
                throw new NotImplementedException("GetValue not implemented for the given field type.");
            }
            else
            {
                return m_value;
            }
        }

        public override void SetValue(Object obj, Object value,BindingFlags invokeAttr,Binder binder,CultureInfo culture)
        {
            throw new NotImplementedException();
        }

		// [Xamarin] Expression evaluator.
		public override object[] GetCustomAttributes (bool inherit)
		{
			if (m_customAttributes == null)
				m_customAttributes = MetadataHelperFunctionsExtensions.GetDebugAttributes (m_importer, m_fieldToken);
			return m_customAttributes;
		}

		// [Xamarin] Expression evaluator.
		public override object[] GetCustomAttributes (Type attributeType, bool inherit)
		{
			ArrayList list = new ArrayList ();
			foreach (object ob in GetCustomAttributes (inherit)) {
				if (attributeType.IsInstanceOfType (ob))
					list.Add (ob);
			}
			return list.ToArray ();
		}

		// [Xamarin] Expression evaluator.
		public override bool IsDefined (Type attributeType, bool inherit)
		{
			return GetCustomAttributes (attributeType, inherit).Length > 0;
		}


        public  override Type FieldType 
        {
            get 
            {
                throw new NotImplementedException();
            }
        }   

        public override RuntimeFieldHandle FieldHandle 
        {
            get 
            {
                throw new NotImplementedException();
            }
        }

        public override FieldAttributes Attributes 
        {
            get 
            {
                return m_fieldAttributes;
            }
        }

        public override MemberTypes MemberType 
        {
            get 
            {
                throw new NotImplementedException();
            }
        }
    
        public override String Name 
        {
            get 
            {
                return m_name;
            }
        }
    
        public override Type DeclaringType 
        {
            get 
            {
                throw new NotImplementedException();
            }
        }
    
        public override Type ReflectedType 
        {
            get 
            {
                throw new NotImplementedException();
            }
        }

        public override int MetadataToken 
        {
            get 
            {
                return (int)m_fieldToken;
            }
        }

        private readonly IMetadataImport m_importer;
        private readonly uint m_fieldToken;
        private MetadataType m_declaringType;

        private readonly string m_name;
        private readonly FieldAttributes m_fieldAttributes;
        private readonly Object m_value;
		// [Xamarin] Expression evaluator.
		private object[] m_customAttributes;
    }
}
