using System.Linq;

namespace Sirensong.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Removes all whitespace from a string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string with all whitespace removed.</returns>
        public static string TrimWhitepace(this string str) => new(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}