//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using CorApi;
using CorApi.ComInterop;

using CorApi2.debug;

namespace CorApi2.Metadata
{
    public unsafe sealed class CorMetadataImport
    {
        public CorMetadataImport(ICorDebugModule managedModule)
        {
            Guid iid = typeof(IMetadataImport).GUID;
            void* pInterface = null;
            using(Com.UsingReference(&pInterface))
            {
                managedModule.GetMetaDataInterface(&iid, &pInterface).AssertSucceeded($"Could not query for metadata interface {iid.ToString("B").ToUpperInvariant()}.");
                m_importer = Com.QueryInteface<IMetadataImport>(pInterface);
            }
            Debug.Assert(m_importer != null);
        }

        public CorMetadataImport(object metadataImport)
        {
            m_importer = (IMetadataImport) metadataImport;
            Debug.Assert(m_importer != null);
        }

        // methods
        public MethodInfo GetMethodInfo(uint methodToken)
        {
            return new MetadataMethodInfo(m_importer,methodToken, Instantiation.Empty);
        }

        public Type GetType(uint typeToken)
        {
            return new MetadataType(m_importer,typeToken);
        }


        // Get number of generic parameters on a given type.
        // Eg, for 'Foo<t, u>', returns 2.
        // for 'Foo', returns 0.
        public int CountGenericParams(uint typeToken)
        {
            
            // This may fail in pre V2.0 debuggees.
            //Guid IID_IMetadataImport2 = new Guid("FCE5EFA0-8BBA-4f8e-A036-8F2022B08466");
            if( ! (m_importer is IMetadataImport2) )
                return 0; // this means we're pre v2.0 debuggees.
            

            IMetadataImport2 importer2 = (m_importer as IMetadataImport2);
            Debug.Assert( importer2!=null );
            
            int dummy;            
            uint dummy2;
            IntPtr hEnum = IntPtr.Zero;
            int count;
            importer2.EnumGenericParams(ref hEnum, typeToken, out dummy, 1, out dummy2);
            try
            {
                m_importer.CountEnum(hEnum, out count);
            }
            finally
            {
                if( hEnum != IntPtr.Zero )
                    importer2.CloseEnum(hEnum);
            }
            return count;
        }

        // Returns filename of scope, if available.
        // Returns null if filename is not available.
        public string GetScopeFilename()
        {
            int size;

            try
            {
                Guid mvid;
                m_importer.GetScopeProps(null, 0, out size, out mvid);
                StringBuilder sb = new StringBuilder(size);
                m_importer.GetScopeProps(sb, sb.Capacity, out size, out mvid);
                sb.Length = size;
                return sb.ToString();
            }
            catch
            {
                return null;            
            }
        }

        public string GetUserString(int token)
        {
            int size;
            m_importer.GetUserString(token,null,0,out size);
            StringBuilder sb = new StringBuilder(size);
            m_importer.GetUserString(token,sb,sb.Capacity,out size);
            sb.Length=size;
            return sb.ToString();
        }

        public const int TokenNotFound = -1;
        public const int TokenGlobalNamespace = 0;
        
        // returns a type token from name
        // when the function fails, we return token TokenNotFound value.
        public int GetTypeTokenFromName(string name)
        {
            int token = CorMetadataImport.TokenNotFound;
            if( name.Length==0 )
                // this is special global type (we'll return token 0)
                token = CorMetadataImport.TokenGlobalNamespace;
            else
            {
                try 
                {
                    m_importer.FindTypeDefByName(name,0,out token);
                }
                catch(COMException e)
                {
                    token=CorMetadataImport.TokenNotFound;
                    if((HResult)e.ErrorCode==HResult.CLDB_E_RECORD_NOTFOUND)
                    {
                        int i = name.LastIndexOfAny(TypeDelimeters);
                        if(i>0)
                        {
                            int parentToken = GetTypeTokenFromName(name.Substring(0,i));
                            if( parentToken!=CorMetadataImport.TokenNotFound )
                            {
                                try 
                                {
                                    m_importer.FindTypeDefByName(name.Substring(i+1),parentToken,out token);
                                }
                                catch(COMException e2) 
                                {
                                    token=CorMetadataImport.TokenNotFound;
                                    if((HResult)e2.ErrorCode!=HResult.CLDB_E_RECORD_NOTFOUND)
                                        throw;
                                }
                            }
                        } 
                    }
                    else
                    throw;
                }
            }
            return token;
        }

        public string GetTypeNameFromRef(uint token)
        {
            int resScope,size;
            m_importer.GetTypeRefProps(token,out resScope,null,0,out size);
            StringBuilder sb = new StringBuilder(size);
            m_importer.GetTypeRefProps(token,out resScope,sb,sb.Capacity,out size);
            return sb.ToString();
        }

        public string GetTypeNameFromDef(uint token,out uint extendsToken)
        {
            int size;
            TypeAttributes pdwTypeDefFlags;
            m_importer.GetTypeDefProps(token,null,0,out size,
                                       out pdwTypeDefFlags,out extendsToken);
            StringBuilder sb = new StringBuilder(size);
            m_importer.GetTypeDefProps(token,sb,sb.Capacity,out size,
                                       out pdwTypeDefFlags,out extendsToken);
            return sb.ToString();
        }


        public string GetMemberRefName(int token)
        {
            if(!m_importer.IsValidToken((uint)token))
                throw new ArgumentException();

            uint size;
            uint classToken;
            IntPtr ppvSigBlob;
            int pbSig;

            m_importer.GetMemberRefProps((uint)token,
                                         out classToken,
                                         null,
                                         0,
                                         out size,
                                         out ppvSigBlob,
                                         out pbSig
                                         );

            StringBuilder member = new StringBuilder((int)size);
            m_importer.GetMemberRefProps((uint)token,
                                         out classToken,
                                         member,
                                         member.Capacity,
                                         out size,
                                         out ppvSigBlob,
                                         out pbSig
                                         );

            string className=null;
            switch(TokenUtils.TypeFromToken(classToken))
            {
            default:
                Debug.Assert(false);
                break;
            case CorTokenType.mdtTypeRef:
                className = GetTypeNameFromRef(classToken);
                break;
            case CorTokenType.mdtTypeDef: 
                {           
                    uint parentToken;
                    className = GetTypeNameFromDef(classToken,out parentToken);
                    break;
                }
            }
            return className + "." + member.ToString();
        }


        public IEnumerable DefinedTypes
        {
            get 
            {
                return new TypeDefEnum(this);
            }
        }

        public object RawCOMObject
        {
            get 
            {
                return m_importer;
            }
        }


        // properties


        //////////////////////////////////////////////////////////////////////////////////
        //
        // CorMetadataImport variables
        //
        //////////////////////////////////////////////////////////////////////////////////

        internal IMetadataImport  m_importer;
        private static readonly char[] TypeDelimeters = new[] {'.', '+'};
    }

    //////////////////////////////////////////////////////////////////////////////////
    //
    // MetadataMethodInfo
    //
    //////////////////////////////////////////////////////////////////////////////////

    // Struct isn't complete yet; just here for the IsTokenOfType method
} // namspace Microsoft.Debugger.MetadataWrapper
