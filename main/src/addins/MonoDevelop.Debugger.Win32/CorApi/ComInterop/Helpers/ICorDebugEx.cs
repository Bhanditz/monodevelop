using System;
using System.Globalization;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugEx
    {
        /// <summary>
        /// Launch a process under the control of the debugger.
        /// Parameters are the same as the Win32 CreateProcess call.
        /// The caller should remember to execute:
        ///    Microsoft.Win32.Interop.Windows.CloseHandle (
        ///      processInformation.hProcess);
        /// after CreateProcess returns.
        /// </summary>
        public static ICorDebugProcess CreateProcess(this ICorDebug cordebug, string applicationName, string commandLine, SECURITY_ATTRIBUTES processAttributes, SECURITY_ATTRIBUTES threadAttributes, bool inheritHandles, int creationFlags, string environment, string currentDirectory, STARTUPINFOW startupInfo, ref PROCESS_INFORMATION processInformation, CorDebugCreateProcessFlags debuggingFlags)
        {
            /*
             * If commandLine is: <c:\a b\a arg1 arg2> and c:\a.exe does not exist, 
             *    then without this logic, "c:\a b\a.exe" would be tried next.
             * To prevent this ambiguity, this forces the user to quote if the path 
             *    has spaces in it: <"c:\a b\a" arg1 arg2>
             */
            if((applicationName == null) && (!commandLine.StartsWith("\"")))
            {
                int firstSpace = commandLine.IndexOf(" ", StringComparison.Ordinal);
                if(firstSpace != -1)
                    commandLine = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}", commandLine.Substring(0, firstSpace), commandLine.Substring(firstSpace, commandLine.Length - firstSpace));
            }

            ICorDebugProcess proc;
            fixed(char* pApplicationName = applicationName)
            fixed(char* pCommandLine = commandLine)
            fixed(char* pCurrentDirectory = currentDirectory)
            fixed(char* pEnv = environment)
                cordebug.CreateProcess((ushort*)pApplicationName, (ushort*)pCommandLine, &processAttributes, &threadAttributes, inheritHandles ? 1 : 0, (uint)creationFlags, pEnv, (ushort*)pCurrentDirectory, &startupInfo, &processInformation, debuggingFlags, out proc);

            return proc;
        }
    }
}