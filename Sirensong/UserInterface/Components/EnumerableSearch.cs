using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace Sirensong.UserInterface.Components
{
    /// <summary>
    ///     A collection of UI components for Sirensong.
    /// </summary>
    public static partial class SirensongUIComponents
    {
        /// <summary>
        ///     A search bar for enumerables.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable.</typeparam>
        /// <param name="label">The label for the search bar.</param>
        /// <param name="enumerable">The enumerable to search.</param>
        /// <param name="search">The reference to the search string.</param>
        /// <param name="searchSelector">The selector for the search.</param>
        /// <param name="caseSensitive">Whether the search should be case sensitive.</param>
        /// <returns>The filtered enumerable.</returns>
        public static IEnumerable<T> EnumerableSearch<T>(string label, IEnumerable<T> enumerable, ref string search, Func<T, string[]> searchSelector, bool caseSensitive = false)
        {
            ImGui.BeginGroup();
            if (!label.StartsWith("##", StringComparison.InvariantCulture))
            {
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(label).X - 40);
            }
            else
            {
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 40);
            }

            // Draw UI Elements
            SiUI.InputText(label, ref search, 60, true);
            ImGui.SameLine();
            ImGuiComponents.IconButton(FontAwesomeIcon.Question);
            SiUI.TooltipLast("Search syntax:\n! - does not contain\n^ - starts with\n$ - ends with\n= - equals\n\nDefault - contains");
            ImGui.EndGroup();

            return Process(search, enumerable, searchSelector, caseSensitive);
        }

        private static IEnumerable<T> Process<T>(string search, IEnumerable<T> enumerable, Func<T, string[]> searchSelector, bool caseSensitive)
        {
            if (search.Trim() == string.Empty)
            {
                return enumerable;
            }

            var appliedSearch = caseSensitive ? search : search.ToLowerInvariant();

            try
            {
                var result = appliedSearch[0] switch
                {
                    '!' => enumerable.Where(x => !searchSelector(x).Any(y => y.ToLowerInvariant().Contains(appliedSearch[1..], StringComparison.InvariantCulture))),
                    '^' => enumerable.Where(x => searchSelector(x).Any(y => y.ToLowerInvariant().StartsWith(appliedSearch[1..], StringComparison.InvariantCulture))),
                    '$' => enumerable.Where(x => searchSelector(x).Any(y => y.ToLowerInvariant().EndsWith(appliedSearch[1..], StringComparison.InvariantCulture))),
                    '=' => enumerable.Where(x => searchSelector(x).Any(y => y.ToLowerInvariant().Equals(appliedSearch[1..], StringComparison.Ordinal))),
                    _ => enumerable.Where(x => searchSelector(x).Any(y => y.ToLowerInvariant().Contains(appliedSearch, StringComparison.InvariantCulture)))
                };

                return result;
            }
            catch (Exception)
            {
                return enumerable;
            }
        }
    }
}
