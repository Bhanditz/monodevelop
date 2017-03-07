//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

using CorApi.ComInterop;
using CorApi.Pinvoke;

using CorApi2.Extensions;
using CorApi2.Metahost;

using Microsoft.Win32.SafeHandles;

namespace CorApi2.debug
{
    /**
     * Wraps the native CLR Debugger.
     * Note that we don't derive the class from WrapperBase, becuase this
     * class will never be returned in any callback.
     */
    public sealed unsafe class CorDebugger : MarshalByRefObject
    {

    



        /**
         * Helper for invoking events.  Checks to make sure that handlers
         * are hooked up to a handler before the handler is invoked.
         *
         * We want to allow maximum flexibility by our callers.  As such,
         * we don't require that they call <code>e.Controller.Continue</code>,
         * nor do we require that this class call it.  <b>Someone</b> needs
         * to call it, however.
         *
         * Consequently, if an exception is thrown and the process is stopped,
         * the process is continued automatically.
         */

        void InternalFireEvent(ManagedCallbackType callbackType,CorEventArgs e)
        {
            ICorDebugProcess owner;
            ICorDebugController c = e.Controller;
            Debug.Assert(c != null);
            Debug.Assert(c is ICorDebugAppDomain);
            ((ICorDebugAppDomain)c).GetProcess(out owner).AssertSucceeded("owner = (((ICorDebugAppDomain)c).GetProcess(out owner)");
            Debug.Assert(owner != null);
            try
            {
                owner.DispatchEvent(callbackType, e);
            }
            finally
            {
                if(e.Continue)
                {
                    e.Controller.Continue(false);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        // ManagedCallback
        //
        ////////////////////////////////////////////////////////////////////////////////

        /**
         * This is the object that gets passed to the debugger.  It's
         * the intermediate "source" of the events, which repackages
         * the event arguments into a more approprate form and forwards
         * the call to the appropriate function.
         */
        private class ManagedCallback : ManagedCallbackBase
        {
            public ManagedCallback (CorDebugger outer)
            {
                m_outer = outer;
            }
            protected override int HandleEvent(ManagedCallbackType eventId, CorEventArgs args)
            {
                m_outer.InternalFireEvent(eventId, args);
            }
            private readonly CorDebugger m_outer;
        }

         

    } /* class Debugger */
} /* namespace */
