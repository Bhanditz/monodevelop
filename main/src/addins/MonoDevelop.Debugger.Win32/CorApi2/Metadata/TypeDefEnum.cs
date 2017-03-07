using System;
using System.Collections;

namespace CorApi2.Metadata
{
    class TypeDefEnum : IEnumerable, IEnumerator, IDisposable
    {
        public TypeDefEnum (CorMetadataImport corMeta)
        {
            m_corMeta = corMeta;
        }

        ~TypeDefEnum()
        {
            DestroyEnum();
        }

        public void Dispose()
        {
            DestroyEnum();
            GC.SuppressFinalize(this);
        }

        //
        // IEnumerable interface
        //
        public IEnumerator GetEnumerator ()
        {
            return this;
        }

        //
        // IEnumerator interface
        //
        public bool MoveNext ()
        {
            uint token;
            uint c;
            
            m_corMeta.m_importer.EnumTypeDefs(ref m_enum,out token,1, out c);
            if (c==1) // 1 new element
                m_type = m_corMeta.GetType(token);
            else
                m_type = null;
            return m_type != null;
        }

        public void Reset ()
        {
            DestroyEnum();
            m_type = null;
        }

        public Object Current
        {
            get 
            {
                return m_type;
            }
        }

        protected void DestroyEnum()
        {
            m_corMeta.m_importer.CloseEnum(m_enum);
            m_enum=new IntPtr();
        }

        private readonly CorMetadataImport m_corMeta;
        private IntPtr m_enum;                              
        private Type m_type;
    }
}