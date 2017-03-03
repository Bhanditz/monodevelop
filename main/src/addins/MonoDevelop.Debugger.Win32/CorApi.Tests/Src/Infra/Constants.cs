using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CorApi.Tests.Infra
{
    public static class Constants
    {
        private static readonly string RootFolder = typeof(Constants).Assembly.Location;

        public static string TestDataFolder => Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..", "TestData"));

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
                    BinaryPath = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "Net45ConsoleApp32bit", "bin", "Debug", "Net45ConsoleApp32bit.exe")),
                    WorkingDirectory = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "Net45ConsoleApp32bit", "bin", "Debug"))
                };
            }
        }

        public static ApplicationDescriptor Net45ConsoleApp64Bit
        {
            get
            {
                return new ApplicationDescriptor
                {
                    BinaryPath = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "Net45ConsoleApp64bit", "bin", "Debug", "Net45ConsoleApp64bit.exe")),
                    WorkingDirectory = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "Net45ConsoleApp64bit", "bin", "Debug"))
                };
            }
        }

        public static ApplicationDescriptor NetCoreApp10ConsoleAppPdb
        {
            get
            {
                if (!Environment.Is64BitProcess)
                    throw new InvalidOperationException("Only 64-bit supported");
                return new ApplicationDescriptor
                {
                    BinaryPath = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "NetCoreApp10ConsoleAppPdb", "bin", "Debug", "netcoreapp1.0", "NetCoreApp10ConsoleAppPdb.dll")),
                    WorkingDirectory = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "NetCoreApp10ConsoleAppPdb", "bin", "Debug"))
                };
            }
        }

        public static ApplicationDescriptor NetCoreApp10ConsoleAppPpdb
        {
            get
            {
                if (!Environment.Is64BitProcess)
                    throw new InvalidOperationException("Only 64-bit supported");
                return new ApplicationDescriptor
                {
                    BinaryPath = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "NetCoreApp10ConsoleAppPpdb", "bin", "Debug", "netcoreapp1.0", "NetCoreApp10ConsoleAppPpdb.dll")),
                    WorkingDirectory = Path.GetFullPath(Path.Combine(RootFolder, "..", "..", "..", "..",
                        "TestData", "NetCoreApp10ConsoleAppPpdb", "bin", "Debug"))
                };
            }
        }
    }
}