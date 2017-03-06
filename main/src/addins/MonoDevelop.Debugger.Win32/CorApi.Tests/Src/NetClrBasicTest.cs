using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using CorApi.ComInterop;
using CorApi.Pinvoke;
using CorApi.Tests.Infra;

using NUnit.Framework;

namespace CorApi.Tests
{
    public unsafe class NetClrBasicTest : BasicTestCommon
    {
        [Test]
        public void Run()
        {
            Console.Error.WriteLine(0.0);
            //            MessageBox.Show("Stop!");
            ApplicationDescriptor app = Constants.Net45ConsoleApp;
            try
            {
                Console.Error.WriteLine(1.0);
                ICorDebug cordbg = MscoreeHelpers.CreateDebuggingInterfaceFromVersion(4, MscoreeHelpers.GetCORVersion());

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