
using System;
using System.IO;

namespace Mono.Debugging.Win32.Tests
{
    internal static class Constants
    {
        private static readonly string RootFolder = typeof(Constants).Assembly.Location;

        public static ApplicationDescriptor Net45ConsoleApp
        {
            get
            {
                return Environment.Is64BitProcess
                    ? Net45ConsoleApp64Bit
                    : Net45ConsoleApp32Bit;
            }
        }

        public static ApplicationDescriptor Net45ConsoleApp32Bit
        {
            get
            {
                return new ApplicationDescriptor
                {
                    BinaryPath = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..",
                        "data", "Net45ConsoleApp32bit", "bin", "Debug", "Net45ConsoleApp32bit.exe")),
                    WorkingDirectory = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..",
                        "data", "Net45ConsoleApp32bit", "bin", "Debug"))
                };
            }
        }

        public static ApplicationDescriptor Net45ConsoleApp64Bit
        {
            get
            {
                return new ApplicationDescriptor
                {
                    BinaryPath = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..",
                        "data", "Net45ConsoleApp64bit", "bin", "Debug", "Net45ConsoleApp64bit.exe")),
                    WorkingDirectory = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..",
                        "data", "Net45ConsoleApp64bit", "bin", "Debug"))
                };
            }
        }
    }
}