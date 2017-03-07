using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

using CorApi;
using CorApi.ComInterop;

using CorApi2.Extensions;

namespace CorApi2.Metadata
{
    public sealed class MetadataMethodInfo : MethodInfo
    {
        internal MetadataMethodInfo(IMetadataImport importer, uint methodToken, Instantiation instantiation)
        {
            if(!importer.IsValidToken((uint)methodToken))
                throw new ArgumentException();

            m_importer = importer;
            m_methodToken=methodToken;

            int size;
            uint pdwAttr;
            IntPtr ppvSigBlob;
            uint pulCodeRVA,pdwImplFlags;
            uint pcbSigBlob;

            m_importer.GetMethodProps((uint)methodToken,
                out m_classToken,
                null,
                0,
                out size,
                out pdwAttr,
                out ppvSigBlob, 
                out pcbSigBlob,
                out pulCodeRVA,
                out pdwImplFlags);

            StringBuilder szMethodName = new StringBuilder(size);
            m_importer.GetMethodProps((uint)methodToken,
                out m_classToken,
                szMethodName,
                szMethodName.Capacity,
                out size,
                out pdwAttr,
                out ppvSigBlob, 
                out pcbSigBlob,
                out pulCodeRVA,
                out pdwImplFlags);

            // [Xamarin] Expression evaluator.
            CorCallingConvention callingConv;
            MetadataHelperFunctionsExtensions.ReadMethodSignature (importer, instantiation, ref ppvSigBlob, out callingConv, out m_retType, out m_argTypes, out m_sentinelIndex);
            m_name = szMethodName.ToString();
            m_methodAttributes = (MethodAttributes)pdwAttr;
        }

        // [Xamarin] Expression evaluator.
        public override Type ReturnType 
        {
            get 
            {
                return m_retType;
            }
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes 
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

        public override Type DeclaringType 
        { 
            get 
            {
                if(TokenUtils.IsNullToken(m_classToken))
                    return null;                            // this is method outside of class
                
                return new MetadataType(m_importer,m_classToken);
            }
        }

        public override string Name 
        {
            get 
            {
                return m_name;
            }
        }

        public override MethodAttributes Attributes 
        { 
            get
            {
                return m_methodAttributes;
            }
        }

        public override RuntimeMethodHandle MethodHandle 
        { 
            get 
            {
                throw new NotImplementedException();
            }
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        // [Xamarin] Expression evaluator.
        public override bool IsDefined (Type attributeType, bool inherit)
        {
            return GetCustomAttributes (attributeType, inherit).Length > 0;
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
        public override object[] GetCustomAttributes(bool inherit)
        {
            if (m_customAttributes == null)
                m_customAttributes = MetadataHelperFunctionsExtensions.GetDebugAttributes (m_importer, m_methodToken);
            return m_customAttributes;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        // [Xamarin] Expression evaluator.
        public override System.Reflection.ParameterInfo[] GetParameters()
        {
            ArrayList al = new ArrayList();
            IntPtr hEnum = new IntPtr();
            int nArg = 0;
            try 
            {
                while(true) 
                {
                    uint count;
                    int paramToken;
                    m_importer.EnumParams(ref hEnum,
                        m_methodToken, out paramToken,1,out count);
                    if(count!=1)
                        break;
                    // this fixes IndexOutOfRange exception. Sometimes EnumParams gives you a param with position that is out of m_argTypes.Count
                    // return typeof(object) for unmatched parameter
                    Type argType = nArg < m_argTypes.Count ? m_argTypes[nArg++] : typeof(object);

                    var mp = new MetadataParameterInfo (m_importer, paramToken, this, argType);
                    if (mp.Name != String.Empty)
                        al.Add (mp);
                    //al.Add(new MetadataParameterInfo(m_importer,paramToken,
                    //                                 this,DeclaringType));
                }
            }
            finally 
            {
                m_importer.CloseEnum(hEnum);
            }
            return (ParameterInfo[]) al.ToArray(typeof(ParameterInfo));
        }

        public override int MetadataToken
        {
            get 
            {
                return (int)m_methodToken;
            }
        }

        public string[] GetGenericArgumentNames() 
        {
            return MetadataHelperFunctions.GetGenericArgumentNames(m_importer,m_methodToken);
        }

        public int VarargStartIndex
        {
            get
            {
                return m_sentinelIndex;
            }
        }

        private readonly IMetadataImport m_importer;
        private readonly string m_name;
        private readonly uint m_classToken;
        private readonly uint m_methodToken;
        private readonly MethodAttributes m_methodAttributes;
        // [Xamarin] Expression evaluator.
        private readonly List<Type> m_argTypes;
        private readonly Type m_retType;
        private object[] m_customAttributes;
        private readonly int m_sentinelIndex;
    }
}