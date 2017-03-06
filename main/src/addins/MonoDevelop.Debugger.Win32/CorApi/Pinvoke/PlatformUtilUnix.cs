using System;
using System.Runtime.InteropServices;

namespace CorApi.Pinvoke
{
    /// <summary>
    /// A separate class so that JITting the main <see cref="PlatformUtil"/> class does not cause actually trying to load types from the mono-posix assembly on winnt.
    /// </summary>
    internal static class PlatformUtilUnix
    {
        [DllImport ("libc")]
        private static extern int uname (IntPtr buf);

        private static string GetSysnameFromUname()
        {
            var buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal (8192);
                // This is a hacktastic way of getting sysname from uname ()
                var rc = uname(buf);
                if (rc != 0)
                {
                    throw new Exception("uname from libc returned " + rc);
                }

                var os = Marshal.PtrToStringAnsi(buf);
                return os;
            } finally {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal (buf);
            }
        }

        internal static PlatformUtil.Platform GetUnixPlatform()
        {
            var sysname = GetSysnameFromUname();
            switch(sysname)
            {
            case "Darwin":
                return PlatformUtil.Platform.MacOsX;
            case "Linux":
                return PlatformUtil.Platform.Linux;
            default:
                throw new Exception("uname() returned unsupported system: " + sysname);
            }
        }
    }
}