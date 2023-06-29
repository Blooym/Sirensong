using System;
using Dalamud.Utility;

namespace Sirensong.Utility
{
    [Obsolete($"No longer necessary - use Util.IsLinux from Dalamud instead.")]
    public static class OS
    {
        /// <summary>
        ///     Heuristically determine the operating system. Checks for Wine first, then the environment.
        /// </summary>
        /// <returns>The operating system, "UNIX/Wine" if Wine is detected, or "Unknown" if the OS is not recognized.</returns>
        public static string GetOSName()
        {
            if (Util.IsLinux())
            {
                return "UNIX/Wine";
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