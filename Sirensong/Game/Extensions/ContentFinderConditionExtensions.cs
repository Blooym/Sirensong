using System;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;
using ContentType = Sirensong.Game.Enums.ContentType;

namespace Sirensong.Game.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="ContentFinderCondition" />
    /// </summary>
    public static class ContentFinderConditionExtensions
    {
        /// <summary>
        ///     Gets the <see cref="ContentDifficulty" /> from the <see cref="ContentFinderCondition" /> name. Only works for the
        ///     English sheet!
        /// </summary>
        /// <param name="cfCond">The English <see cref="ContentFinderCondition" /> sheet to use.</param>
        /// <returns>The <see cref="ContentDifficulty" /> </returns>
        public static ContentDifficulty GetContentDifficulty(this ContentFinderCondition cfCond) => (string)cfCond.Name switch
        {
            string s when s.Contains("(Hard)", StringComparison.OrdinalIgnoreCase) => ContentDifficulty.Hard,
            string s when s.Contains("(Extreme)", StringComparison.OrdinalIgnoreCase) => ContentDifficulty.Extreme,
            string s when s.StartsWith("The Minstrel's Ballad:", StringComparison.OrdinalIgnoreCase) => ContentDifficulty.Extreme,
            string s when s.Contains("(Savage)", StringComparison.OrdinalIgnoreCase) => ContentDifficulty.Savage,
            string s when s.Contains("(Ultimate)", StringComparison.OrdinalIgnoreCase) => ContentDifficulty.Ultimate,
            string s when s.Contains("(Unreal)", StringComparison.OrdinalIgnoreCase) => ContentDifficulty.Unreal,
            _ => ContentDifficulty.Normal,
        };

        /// <summary>
        ///     Gets the <see cref="Lumina.Excel.GeneratedSheets.ContentType" /> from the given
        ///     <see cref="ContentFinderCondition" />
        /// </summary>
        /// <param name="cfCond">The <see cref="ContentFinderCondition" /> sheet to use.</param>
        /// <param name="splitRaids">Splits the raid content type into Alliance and Normal raids.</param>
        /// <returns>The <see cref="Lumina.Excel.GeneratedSheets.ContentType" />, or null if unable to find it </returns>
        public static ContentType? GetContentType(this ContentFinderCondition cfCond, bool splitRaids = false)
        {
            var typeRow = cfCond.ContentType.Value?.RowId;

            if (splitRaids && typeRow == (int)ContentType.Raids)
            {
                // Unknown8 = number of parties in the instance.
                if (cfCond.AllianceRoulette ||
                    cfCond.TerritoryType.Value?.TerritoryIntendedUse == (byte)TerritoryIntendedUseType.AllianceRaid ||
                    cfCond.ContentMemberType.Value?.Unknown8 == 3)
                {
                    return ContentType.AllianceRaid;
                }

                return ContentType.Raids;
            }

            return (ContentType?)typeRow;
        }
    }
}
