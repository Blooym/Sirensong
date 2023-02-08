using System;
using System.Globalization;
using System.Linq;

namespace Sirensong.Extensions
{
    /// <summary>
    /// Extensions for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all whitespace from a string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string with all whitespace removed.</returns>
        public static string RemoveWhitespace(this string str) => new(str.Where(c => !char.IsWhiteSpace(c)).ToArray());

        /// <summary>
        /// Removes all proceeding and trailing whitespace and any duplicate whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A trimmed and squished string.</returns>
        public static string TrimAndSquish(this string str) => string.IsNullOrEmpty(str) ? string.Empty : string.Join(" ", str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        /// <summary>
        /// Converts a string to title case using <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A new string in title case.</returns>
        public static string ToTitleCase(this string str) => string.IsNullOrEmpty(str) ? string.Empty : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
    }
}