using System;

namespace CorApi.Pinvoke
{
    public static class PlatformUtil
    {
        static PlatformUtil()
        {
            IsRunningUnderWindows =
                Environment.OSVersion.Platform == PlatformID.Win32NT ||
                Environment.OSVersion.Platform == PlatformID.Win32S ||
                Environment.OSVersion.Platform == PlatformID.Win32Windows ||
                Environment.OSVersion.Platform == PlatformID.WinCE;

            IsRunningOnMono = Type.GetType("Mono.Runtime") != null;

            if (IsRunningUnderWindows)
                RuntimePlatform = Platform.Windows;
            else
            {
                if(!IsRunningOnMono)
                    throw new Exception("Running on non-Mono runtime is not supported under Unix");
                RuntimePlatform = PlatformUtilUnix.GetUnixPlatform();
            }
        }

        public static readonly bool IsRunningUnderWindows;

        public static bool IsRunningOnMono;

        public static readonly Platform RuntimePlatform;

        public enum Platform
        {
            Windows,
            MacOsX,
            Linux
        }
    }
}