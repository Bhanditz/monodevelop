using System.IO;

using CorApi.Tests.Infra;

using Should;
using Xunit;

namespace Mono.Debugging.Win32.Tests
{
    public class ConstantsTests
    {
        [Fact]
        public void Net45ConsoleApp32BitShouldExistsInDataFolder()
        {
            File.Exists(Constants.Net45ConsoleApp32Bit.BinaryPath).ShouldBeTrue(
                string.Format("File {0} not exists", Constants.Net45ConsoleApp32Bit.BinaryPath));
            Directory.Exists(Constants.Net45ConsoleApp32Bit.WorkingDirectory).ShouldBeTrue(
                string.Format("Directory {0} not exists", Constants.Net45ConsoleApp32Bit.WorkingDirectory));
        }

        [Fact]
        public void Net45ConsoleApp64BitShouldExistsInDataFolder()
        {
            File.Exists(Constants.Net45ConsoleApp64Bit.BinaryPath).ShouldBeTrue(
                string.Format("File {0} not exists", Constants.Net45ConsoleApp64Bit.BinaryPath));
            Directory.Exists(Constants.Net45ConsoleApp64Bit.WorkingDirectory).ShouldBeTrue(
                string.Format("Directory {0} not exists", Constants.Net45ConsoleApp64Bit.WorkingDirectory));
        }

        [Fact]
        public void NetCoreApp10ConsoleAppPdbBitShouldExistsInDataFolder()
        {
            File.Exists(Constants.NetCoreApp10ConsoleAppPdb.BinaryPath).ShouldBeTrue(
                string.Format("File {0} not exists", Constants.NetCoreApp10ConsoleAppPdb.BinaryPath));
            Directory.Exists(Constants.NetCoreApp10ConsoleAppPdb.WorkingDirectory).ShouldBeTrue(
                string.Format("Directory {0} not exists", Constants.NetCoreApp10ConsoleAppPdb.WorkingDirectory));
        }

        [Fact]
        public void NetCoreApp10ConsoleAppPpdbBitShouldExistsInDataFolder()
        {
            File.Exists(Constants.NetCoreApp10ConsoleAppPpdb.BinaryPath).ShouldBeTrue(
                string.Format("File {0} not exists", Constants.NetCoreApp10ConsoleAppPpdb.BinaryPath));
            Directory.Exists(Constants.NetCoreApp10ConsoleAppPpdb.WorkingDirectory).ShouldBeTrue(
                string.Format("Directory {0} not exists", Constants.NetCoreApp10ConsoleAppPpdb.WorkingDirectory));
        }

    }
}