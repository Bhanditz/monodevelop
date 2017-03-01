using System.IO;
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
    }
}