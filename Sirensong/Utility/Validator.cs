using System;
using System.Collections.Generic;

namespace Sirensong.Utility
{
    public static class Validator
    {
        /// <summary>
        ///     Tests if a format string is valid.
        /// </summary>
        /// <param name="message">The message to test.</param>
        /// <param name="highestPlaceholder">The highest placeholder index expected in the message.</param>
        /// <param name="checkContainAll">Ensure the message contains all placeholders.</param>
        /// <param name="allowDuplicates">Whether or nto allow duplicates that would otherwise be valid to show up multiple times.</param>
        /// <returns>Whether or not the message is valid.</returns>
        public static bool TestFormatString(string message, uint highestPlaceholder, bool containAll = true, bool allowDuplicates = true)
        {
            try
            {
                var placeholders = new List<uint>();

                var split = message.Split('{');
                for (var i = 1; i < split.Length; i++)
                {
                    var placeholder = split[i].Split('}')[0];

                    // Ensure the placeholder is a number.
                    if (!uint.TryParse(placeholder, out var placeholderIndex))
                    {
                        return false;
                    }

                    // Ensure the placeholder is within the placeholder count.
                    if (placeholderIndex >= highestPlaceholder)
                    {
                        return false;
                    }

                    // Handle duplicate placeholders.
                    if (placeholders.Contains(placeholderIndex))
                    {
                        // Don't allow them.
                        if (!allowDuplicates)
                        {
                            return false;
                        }

                        continue;
                    }

                    // Allow them.
                    placeholders.Add(placeholderIndex);
                }

                // If we want to check the message contains all placeholders, ensure the count of the placeholders
                // is equal to the expected amount.
                if (containAll && placeholders.Count != highestPlaceholder)
                {
                    return false;
                }

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
