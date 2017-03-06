using System;

using CorApi.Pinvoke;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class MscoreeHelpers
    {
        [NotNull]
        public static ICorDebug CreateDebuggingInterfaceFromVersion(int iDebuggerVersion, [NotNull] string sDebuggeeVersion)
        {
            if(sDebuggeeVersion == null)
                throw new ArgumentNullException(nameof(sDebuggeeVersion));

            void* pCorDb;
            ICorDebug cordbg;
            using(Com.UsingReference(&pCorDb))
            {
                Console.Error.WriteLine(1.1);
                fixed(char* pchVer = sDebuggeeVersion)
                    MscoreeDll.CreateDebuggingInterfaceFromVersion(iDebuggerVersion, (ushort*)pchVer, &pCorDb).AssertSucceeded($"Could not CreateDebuggingInterfaceFromVersion for debugger v{iDebuggerVersion} on debuggee {sDebuggeeVersion}.");
                Console.Error.WriteLine(1.2);
                cordbg = Com.QueryInteface<ICorDebug>(pCorDb);
                Console.Error.WriteLine(1.3);
            }
            return cordbg;
        }

        [NotNull]
        public static string GetCORVersion()
        {
            return GetCORVersionCore(0x10);
        }

        [NotNull]
        private static string GetCORVersionCore(uint cchBuf)
        {
            ushort* pBuffer = stackalloc ushort[((int)cchBuf)];
            uint cchActual;

            int hr = MscoreeDll.GetCORVersion(pBuffer, cchBuf, &cchActual);
            if((hr == (int)HResults.E_INSUFFICIENT_BUFFER) && (cchActual > cchBuf))
                return GetCORVersionCore(cchActual);

            hr.AssertSucceeded($"Could not call GetCORVersion on the buffer of size {cchBuf:N0}.");
            return new string((char*)pBuffer);
        }
    }
}