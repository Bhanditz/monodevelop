//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CorApi.ComInterop;

namespace Microsoft.Samples.Debugging.CorDebug
{
    public sealed class CorMDA : WrapperBase
    {
        private ICorDebugMDA m_mda;
        internal CorMDA(ICorDebugMDA mda)
            :base(mda)
        {
            m_mda = mda;
        }

        public unsafe CorDebugMDAFlags Flags
        {
            get
            {
                CorDebugMDAFlags flags;
                m_mda.GetFlags(&flags);
                return flags;
            }
        }

        string m_cachedName = null;
        public string Name        
        {
            get 
            {
                // This is thread safe because even in a race, the loser will just do extra work.
                // but no harm done.
                if (m_cachedName == null)
                {
                    uint len = 0;
                    m_mda.GetName(0, out len, null);
                                    
                    char[] name = new char[len];
                    uint fetched = 0;

                    m_mda.GetName ((uint) name.Length, out fetched, name);
                    // ``fetched'' includes terminating null; String doesn't handle null, so we "forget" it.
                    m_cachedName = new String (name, 0, (int) (fetched-1));
                }
                return m_cachedName;               
            } // end get
        }

        public string XML
        {
            get 
            {
                uint len = 0;
                m_mda.GetXML(0, out len, null);
                                
                char[] name = new char[len];
                uint fetched = 0;

                m_mda.GetXML ((uint) name.Length, out fetched, name);
                // ``fetched'' includes terminating null; String doesn't handle null, so we "forget" it.
                return new String (name, 0, (int) (fetched-1));
            }            
        }

        public string Description
        {
            get 
            {
                uint len = 0;
                m_mda.GetDescription(0, out len, null);
                                
                char[] name = new char[len];
                uint fetched = 0;

                m_mda.GetDescription((uint) name.Length, out fetched, name);
                // ``fetched'' includes terminating null; String doesn't handle null, so we "forget" it.
                return new String (name, 0, (int) (fetched-1));
            }            
        }

        public int OsTid
        {
            get
            {
                uint tid;
                m_mda.GetOSThreadId(out tid);
                return (int) tid;
            }            
        }
    } // end CorMDA

 /* class Module */
} /* namespace */
