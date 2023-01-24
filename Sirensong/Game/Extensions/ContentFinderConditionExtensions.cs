using System;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;

namespace Sirensong.Game.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="ContentFinderCondition" />
    /// </summary>
    public static class ContentFinderConditionExtensions
    {
        /// <summary>
        ///     Gets the <see cref="ContentDifficulty"/> from the <see cref="ContentFinderCondition" /> name. Only works for the English sheet!
        /// </summary>
        /// <param name="cfCond">The English <see cref="ContentFinderCondition" /> sheet to use.</param>
        /// <returns>The <see cref="ContentDifficulty"/> </returns>
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
        ///     Gets the <see cref="ContentType" /> from the given <see cref="ContentFinderCondition" />
        /// </summary>
        /// <param name="id"></param>
        /// <param name="splitRaids">Whether the check should split <see cref="ContentType.Raids" /> and <see cref="ContentType.AllianceRaids" /> apart.
        /// <returns>The <see cref="ContentType" />, or null if unable to find it </returns>
        public static Enums.ContentType? GetContentType(this ContentFinderCondition cfCond, bool splitRaids = false)
        {
            var typeRow = cfCond.ContentType.Value?.RowId;

            if (splitRaids && typeRow == (int)Enums.ContentType.Raids)
            {
                // Unknown8 = number of parties in the instance.
                if (cfCond.AllianceRoulette || cfCond.ContentMemberType.Value?.Unknown8 == 3 || cfCond.ContentMemberType.Value?.TanksPerParty > 1)
                {
                    return Enums.ContentType.AllianceRaid;
                }

                return Enums.ContentType.Raids;
            }

            return (Enums.ContentType?)typeRow;
        }
    }
}