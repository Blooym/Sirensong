using System;

namespace Sirensong.Utility
{
    /// <summary>
    ///     A collection of common utility methods.
    /// </summary>
    public static class Common
    {
        /// <summary>
        ///     Returns the operating system of the executing system or "Wine" if in a WINEPREFIX.
        /// </summary>
        public static string GetOS()
        {
            if (bool.TryParse(Environment.GetEnvironmentVariable("XL_WINEONLINUX"), out var isWineOnLinux) && isWineOnLinux)
            {
                return "Linux";
            }
            else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WINEPREFIX")))
            {
                return "Wine";
            }
            return Environment.OSVersion.Platform switch
            {
                PlatformID.Win32NT => "Windows",
                PlatformID.Unix => "Linux",
                PlatformID.MacOSX => "OSX",
                PlatformID.Win32S => "Windows",
                PlatformID.Win32Windows => "Windows",
                PlatformID.WinCE => "Windows",
                PlatformID.Xbox => "Xbox",
                PlatformID.Other => "Unknown",
                _ => "Unknown",
            };
        }
    }
}