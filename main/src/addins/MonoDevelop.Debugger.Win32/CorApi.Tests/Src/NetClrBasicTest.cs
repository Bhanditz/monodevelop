using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using CorApi.ComInterop;
using CorApi.Tests.Infra;

using NUnit.Framework;

namespace CorApi.Tests
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe class NetClrBasicTest : BasicTestCommon
    {
        private const string ShimLibraryName = "mscoree.dll";

        [DllImport(ShimLibraryName, CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern Int32 CreateDebuggingInterfaceFromVersion(Int32 iDebuggerVersion, UInt16* szDebuggeeVersion, void**ppCordb);

        [Test]
        public void Run()
        {
            Console.Error.WriteLine(0.0);
            //            MessageBox.Show("Stop!");
            ApplicationDescriptor app = Constants.Net45ConsoleApp;
            try
            {
                Console.Error.WriteLine(1.0);
                void* pCorDb;
                ICorDebug cordbg;
                using(Com.UsingReference(&pCorDb))
                {
                    Console.Error.WriteLine(1.1);
                    fixed(char* pchVer = "v4.0.30319")
                        CreateDebuggingInterfaceFromVersion(4, (ushort*)pchVer, &pCorDb).AssertSucceeded("Could not CreateDebuggingInterfaceFromVersion.");
                    Console.Error.WriteLine(1.2);
                    cordbg = Com.QueryInteface<ICorDebug>(pCorDb);
                    Console.Error.WriteLine(1.3);
                }

                CheckBaseCorDbg(cordbg, cdbg =>
                {
                    var si = new STARTUPINFOW();
                    si.cb = (uint)Marshal.SizeOf(si);

                    var pi = new PROCESS_INFORMATION();

                    fixed(char* pchPath = app.BinaryPath)
                    fixed(char* pchCmdl = $"\"{app.BinaryPath}\" NO_BREAK")
                    fixed(char* pchWorkdir = app.WorkingDirectory)
                    {
                        ICorDebugProcess process;
                        cdbg.CreateProcess((ushort*)pchPath, (ushort*)pchCmdl, null, null, 0, 0, null, (ushort*)pchWorkdir, &si, &pi, CorDebugCreateProcessFlags.DEBUG_NO_SPECIAL_OPTIONS, out process);
                        return process;
                    }
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                /*
                          if (session != null)
                          {
                            session.Dispose();
                            session = null;
                          }
                */
            }
        }
    }
}