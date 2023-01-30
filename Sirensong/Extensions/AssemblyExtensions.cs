using System.Reflection;

namespace Sirensong.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Assembly"/>.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Checks to see if the assembly is the executing assembly.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>True if the given assembly is the executing assembly.</returns>
        internal static bool IsExecutingAssembly(this Assembly assembly) => assembly == Assembly.GetExecutingAssembly();
    }
}