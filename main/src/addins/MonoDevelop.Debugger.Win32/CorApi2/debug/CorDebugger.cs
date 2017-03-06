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
        private const int MaxVersionStringLength = 256; // == MAX_PATH

        public static string GetDebuggerVersionFromFile(string pathToExe)
        {
            Debug.Assert(!string.IsNullOrEmpty(pathToExe));
            if(string.IsNullOrEmpty(pathToExe))
                throw new ArgumentException("Value cannot be null or empty.", "pathToExe");
            uint dwLength;
            ushort* pch = stackalloc ushort[MaxVersionStringLength];
            fixed(char* pchPathCopy = pathToExe.ToCharArray() /*declared as a mutable string, so shan't pass the original*/)
                MscoreeDll.GetRequestedRuntimeVersion((ushort*)pchPathCopy, pch, MaxVersionStringLength, &dwLength).AssertSucceeded($"Could not get the requested runtime version from an executable file “{pathToExe}”.");
            return new string((char*)pch);
        }

        public static string GetDebuggerVersionFromPid(uint pid)
        {
            using(var hProcess = new ProcessSafeHandle(Kernel32Dll.OpenProcess((int)(PROCESS_ACCESS.PROCESS_VM_READ | PROCESS_ACCESS.PROCESS_QUERY_INFORMATION | PROCESS_ACCESS.PROCESS_DUP_HANDLE | PROCESS_ACCESS.SYNCHRONIZE), 0, /*inherit handle*/ pid), true))
            {
                if(hProcess.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                uint dwLength;
                ushort* pch = stackalloc ushort[MaxVersionStringLength];
                MscoreeDll.GetVersionFromProcess(hProcess.Value, pch, MaxVersionStringLength, &dwLength).AssertSucceeded($"Could not get the debugger version from a running process with PID {pid:N0}.");
                return new string((char*)pch);
            }
        }

        public static List<string> GetProcessLoadedRuntimes (uint pid)
        {
            using (ProcessSafeHandle ph = new ProcessSafeHandle(Kernel32Dll.OpenProcess (
                (int) (PROCESS_ACCESS.PROCESS_VM_READ |
                    PROCESS_ACCESS.PROCESS_QUERY_INFORMATION |
                    PROCESS_ACCESS.PROCESS_DUP_HANDLE |
                    PROCESS_ACCESS.SYNCHRONIZE),
                0, // inherit handle
                pid), true)) {
                if (ph.IsInvalid)
                    return new List<string> ();
                int neededSize = MaxVersionStringLength;
                IClrMetaHost host;
                void* pClrMetaHost = null;
                using(Com.UsingReference(&pClrMetaHost))
                {
                    Guid CLSID_CLRMetaHost = typeof(MscoreeDll.CLSID_CLRMetaHost).GUID;
                    Guid IID_ICLRMetaHost = typeof(IClrMetaHost).GUID;
                    MscoreeDll.CLRCreateInstance(&CLSID_CLRMetaHost, &IID_ICLRMetaHost, &pClrMetaHost).AssertSucceeded("Failed to create the CLR Meta Host instance.");
                    host = Com.QueryInteface<IClrMetaHost>(pClrMetaHost);
                }
                var result = new List<string> ();
                var runtimes = host.EnumerateLoadedRuntimes (ph);
                var array = new object[1];
                int count;
                while (runtimes.Next (1, array, out count) == 0) {
                    var info = array[0] as IClrRuntimeInfo;
                    if (info == null)
                        continue;
                    var stringBuilder = new StringBuilder (MaxVersionStringLength);
                    info.GetVersionString (stringBuilder, ref neededSize);
                    result.Add (stringBuilder.ToString ());
                }
                return result;
            }
        }

        public static string GetDefaultDebuggerVersion()
        {
            return MscoreeHelpers.GetCORVersion();
        }
     

        /// <summary>Creates a debugger wrapper from Guid.</summary>
        public CorDebugger(Guid debuggerGuid)
        {
            object rawDebuggingAPI;
            Guid iidiCorDebug = typeof(ICorDebug).GUID;
            Ole32Dll.CoCreateInstance(ref debuggerGuid,
                                           null, // pUnkOuter
                                           CLSCTX.CLSCTX_INPROC_SERVER,
                                           ref iidiCorDebug,
                                           out rawDebuggingAPI);
            InitFromICorDebug(Com.QueryInteface<ICorDebug>(rawDebuggingAPI));
        }
        /// <summary>Creates a debugger interface that is able debug requested verison of CLR</summary>
        /// <param name="debuggerVersion">Version number of the debugging interface.</param>
        /// <remarks>The version number is usually retrieved either by calling one of following mscoree functions:
        /// GetCorVerison, GetRequestedRuntimeVersion or GetVersionFromProcess.</remarks>
        public CorDebugger (string debuggerVersion)
        {
            InitFromVersion(debuggerVersion);
        }


        [CLSCompliant(false)]
        public CorDebugger (ICorDebug corDebug)
        {
            InitFromICorDebug (corDebug);
        }


        ~CorDebugger()
        {
            if(m_debugger!=null)
                try 
                {
                    Terminate();
                } 
                catch
                {
                    // sometimes we cannot terminate because GC collects object in wrong
                    // order. But since the whole process is shutting down, we really
                    // don't care.
                    
                }
        }


        /**
         * Closes the debugger.  After this method is called, it is an error
         * to call any other methods on this object.
         */
        public void Terminate ()
        {
            Debug.Assert(m_debugger!=null);
            ICorDebug d= m_debugger;
            m_debugger = null;
            d.Terminate ();
        }

        /**
         * Specify the callback object to use for managed events.
         */
        internal void SetManagedHandler (ICorDebugManagedCallback managedCallback)
        {
            m_debugger.SetManagedHandler (managedCallback);
        }

        /**
         * Specify the callback object to use for unmanaged events.
         */
        internal void SetUnmanagedHandler (ICorDebugUnmanagedCallback nativeCallback)
        {
            m_debugger.SetUnmanagedHandler (nativeCallback);
        }

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         */
        public CorProcess CreateProcess (
                                         String applicationName,
                                         String commandLine
                                         )
        {
            return CreateProcess (applicationName, commandLine, ".");
        }

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         */
        public CorProcess CreateProcess (
                                         String applicationName,
                                         String commandLine,
                                         String currentDirectory
                                         )
		{
			// [Xamarin] ASP.NET Debugging.
			return CreateProcess (applicationName, commandLine, currentDirectory, null, 0);
        }

		/**
		 * Launch a process under the control of the debugger.
		 *
		 * Parameters are the same as the Win32 CreateProcess call.
		 */
		// [Xamarin] ASP.NET Debugging.
		public CorProcess CreateProcess (
										 String applicationName,
										 String commandLine,
										 String currentDirectory,
										 IDictionary<string,string> environment
										 )
		{
			return CreateProcess (applicationName, commandLine, currentDirectory, environment, 0);
		}

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         */
        public CorProcess CreateProcess (
                                         String applicationName,
                                         String commandLine,
                                         String currentDirectory,
										 IDictionary<string,string> environment,
                                         int    flags
                                         )
        {
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION ();

            STARTUPINFOW si = new STARTUPINFOW ();
            si.cb = (uint) Marshal.SizeOf(si);

            // initialize safe handles 
			// [Xamarin] ASP.NET Debugging and output redirection.
			SafeFileHandle outReadPipe = null, errorReadPipe = null;
            Action closehandles;
            DebuggerExtensions.SetupOutputRedirection (si, ref flags, out outReadPipe, out errorReadPipe, out closehandles);
			string env = Kernel32Dll.Helpers.GetEnvString(environment);

            CorProcess ret;

            //constrained execution region (Cer)
            System.Runtime.CompilerServices.RuntimeHelpers.PrepareConstrainedRegions();
            try 
            {
            } 
            finally
            {
                ret = CreateProcess (
                                     applicationName,
                                     commandLine, 
                                     default(SECURITY_ATTRIBUTES),
                                     default(SECURITY_ATTRIBUTES),
                                     true,   // inherit handles
                                     flags,  // creation flags
									 env,      // environment
                                     currentDirectory,
                                     si,     // startup info
                                     ref pi, // process information
                                     CorDebugCreateProcessFlags.DEBUG_NO_SPECIAL_OPTIONS);
                if(pi.hProcess != null)
                    Kernel32Dll.CloseHandle(pi.hProcess);
                if(pi.hThread != null)
                    Kernel32Dll.CloseHandle(pi.hThread);
            }

			DebuggerExtensions.TearDownOutputRedirection (outReadPipe, errorReadPipe, ret, closehandles);

            return ret;
        }

        /**
         * Launch a process under the control of the debugger.
         *
         * Parameters are the same as the Win32 CreateProcess call.
         *
         * The caller should remember to execute:
         *
         *    Microsoft.Win32.Interop.Windows.CloseHandle (
         *      processInformation.hProcess);
         *
         * after CreateProcess returns.
         */
        public CorProcess CreateProcess (
                                         string                      applicationName,
                                         string                      commandLine,
                                         SECURITY_ATTRIBUTES         processAttributes,
                                         SECURITY_ATTRIBUTES         threadAttributes,
                                         bool                        inheritHandles,
                                         int                         creationFlags,
                                         string                      environment,  
                                         string                      currentDirectory,
                                         STARTUPINFOW                 startupInfo,
                                         ref PROCESS_INFORMATION     processInformation,
                                         CorDebugCreateProcessFlags  debuggingFlags)
        {
            /*
             * If commandLine is: <c:\a b\a arg1 arg2> and c:\a.exe does not exist, 
             *    then without this logic, "c:\a b\a.exe" would be tried next.
             * To prevent this ambiguity, this forces the user to quote if the path 
             *    has spaces in it: <"c:\a b\a" arg1 arg2>
             */
            if(null == applicationName && !commandLine.StartsWith("\""))
            {
                var firstSpace = commandLine.IndexOf(" ", StringComparison.Ordinal);
                if(firstSpace != -1)
                    commandLine = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}",
                        commandLine.Substring(0,firstSpace), commandLine.Substring(firstSpace, commandLine.Length-firstSpace));
            }

            ICorDebugProcess proc;
            fixed(char* pApplicationName = applicationName)
            fixed(char* pCommandLine = commandLine)
            fixed(char* pCurrentDirectory = currentDirectory)
            fixed(char* pEnv = environment)
            {
                m_debugger.CreateProcess 
                (
                    (ushort*)pApplicationName,
                    (ushort*)pCommandLine,
                    &processAttributes,
                    &threadAttributes,
                    inheritHandles ? 1 : 0,
                    (uint) creationFlags,
                    pEnv,
                    (ushort*)pCurrentDirectory,
                    &startupInfo,
                    &processInformation,
                    debuggingFlags,
                    out proc
                );
            }

            return CorProcess.GetCorProcess(proc);
        }

        /** 
         * Attach to an active process
         */
        public CorProcess DebugActiveProcess (int processId, bool win32Attach)
        {
            ICorDebugProcess proc = null;
            m_debugger.DebugActiveProcess ((uint)processId, win32Attach ? 1 : 0, out proc);
            return CorProcess.GetCorProcess(proc);
        }

        /**
         * Enumerate all processes currently being debugged.
         */
        public IEnumerable Processes
        {
            get
            {
                ICorDebugProcessEnum eproc = null;
                m_debugger.EnumerateProcesses (out eproc);
                return new CorProcessEnumerator (eproc);
            }
        }

        /**
         * Get the Process object for the given PID.
         */
        public CorProcess GetProcess (int processId)
        {
            ICorDebugProcess proc = null;
            m_debugger.GetProcess ((uint) processId, out proc);
            return CorProcess.GetCorProcess(proc);
        }

        /**
         * Warn us of potentional problems in using debugging (eg. whether a kernel debugger is 
         * attached).  This API should probably be renamed or the warnings turned into errors
         * in CreateProcess/DebugActiveProcess
         */
        public void CanLaunchOrAttach(int processId, bool win32DebuggingEnabled)
        {
            m_debugger.CanLaunchOrAttach((uint) processId,
                                         win32DebuggingEnabled?1:0);
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        // CorDebugger private implement part
        //
        ////////////////////////////////////////////////////////////////////////////////

        // called by constructors during initialization
        private void InitFromVersion(string debuggerVersion)
        {
            if( debuggerVersion.StartsWith("v1") )
            {
                throw new ArgumentException( "Can't debug a version 1 CLR process (\"" + debuggerVersion + 
                    "\").  Run application in a version 2 CLR, or use a version 1 debugger instead." );
            }
            
            ICorDebug rawDebuggingAPI;
			// [Xamarin] .NET 4 API Version.
#if MDBG_FAKE_COM
			// TODO: Ideally, there wouldn't be any difference in the corapi code for MDBG_FAKE_COM.
			// This would require puting this initialization logic into the wrapper and interop assembly, which doesn't seem right.
			// We should also release this pUnk, but doing that here would be difficult and we aren't done with it until
			// we shutdown anyway.
			IntPtr pUnk = NativeMethods.CreateDebuggingInterfaceFromVersion((int)CorDebuggerVersion.Whidbey, debuggerVersion);
			rawDebuggingAPI = new NativeApi.CorDebugClass(pUnk);
#else
			int apiVersion = debuggerVersion.StartsWith ("v4") ? 4 : 3;
            rawDebuggingAPI = MscoreeHelpers.CreateDebuggingInterfaceFromVersion(apiVersion, debuggerVersion);
#endif
		    InitFromICorDebug(rawDebuggingAPI);
    	}

        private void InitFromICorDebug(ICorDebug rawDebuggingAPI)
        {
            Debug.Assert(rawDebuggingAPI != null);
            if(rawDebuggingAPI == null)
                throw new ArgumentException("Cannot be null.", nameof(rawDebuggingAPI));

            m_debugger = rawDebuggingAPI;
            m_debugger.Initialize().AssertSucceeded("Could not initialize the debugger interface.");
            m_debugger.SetManagedHandler(new ManagedCallback(this)).AssertSucceeded("Could not assign the managed handler to the debugger interface.");
        }            

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
            CorProcess owner;
            ICorDebugController c = e.Controller;
            Debug.Assert(c!=null);
            if(c is CorProcess)
                owner = (CorProcess)c ;
            else 
            {
                Debug.Assert(c is ICorDebugAppDomain);
                owner = (c as ICorDebugAppDomain).Process;
            }
            Debug.Assert(owner!=null);
            try 
            {
                owner.DispatchEvent(callbackType,e);
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
            protected override void HandleEvent(ManagedCallbackType eventId, CorEventArgs args)
            {
                m_outer.InternalFireEvent(eventId, args);
            }
            private CorDebugger m_outer;
        }

         
        
        private ICorDebug m_debugger = null;
    } /* class Debugger */


  ////////////////////////////////////////////////////////////////////////////////
    //
    // CorEvent Classes & Corresponding delegates
    //
    ////////////////////////////////////////////////////////////////////////////////

    /**
     * All of the Debugger events make a Controller available (to specify
     * whether or not to continue the program, or to stop, etc.).
     *
     * This serves as the base class for all events used for debugging.
     *
     * NOTE: If you don't want <b>Controller.Continue(false)</b> to be
     * called after event processing has finished, you need to set the
     * <b>Continue</b> property to <b>false</b>.
     */

    /**
     * This class is used for all events that only have access to the 
     * CorProcess that is generating the event.
     */

    /**
     * The event arguments for events that contain both a CorProcess
     * and an CorAppDomain.
     */

    /**
     * The base class for events which take an CorAppDomain as their
     * source, but not a CorProcess.
     */

    /**
     * Arguments for events dealing with threads.
     */

    /**
     * Arguments for events involving breakpoints.
     */

    /**
     * Arguments for when a Step operation has completed.
     */

    /**
     * For events dealing with exceptions.
     */

    /**
     * For events dealing the evaluation of something...
     */

    /**
     * For events dealing with module loading/unloading.
     */

    /**
     * For events dealing with class loading/unloading.
     */

    /**
     * For events dealing with debugger errors.
     */

    /**
     * For events dealing with Assemblies.
     */

    /**
     * For events dealing with logged messages.
     */

    /**
     * For events dealing with logged messages.
     */

    /**
     * For events dealing with MDA messages.
     */

    /**
     * For events dealing module symbol updates.
     */

    /**
     * Edit and Continue callbacks
     */

    // Helper class to convert from COM-classic callback interface into managed args.
    // Derived classes can overide the HandleEvent method to define the handling.
} /* namespace */
