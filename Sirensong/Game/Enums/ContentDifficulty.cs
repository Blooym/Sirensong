using System;
using Sirensong.Resources.Localization;

namespace Sirensong.Game.Enums
{
    /// <summary>
    /// Represents content difficulty, does not map from real game data.
    /// </summary>
    public enum ContentDifficulty
    {
        Normal,
        Hard,
        Extreme,
        Savage,
        Ultimate,
        Unreal
    }

    /// <summary>
    /// Extensions for <see cref="ContentDifficulty" />. 
    /// </summary>
    public static class ContentDifficultyExtensions
    {
        /// <summary>
        /// Gets the localized name of the <see cref="ContentDifficulty" /> according to the current CultureInfo.
        /// </summary>
        /// <param name="contentDifficulty"></param>
        /// <returns>The localized name of the <see cref="ContentDifficulty" />.</returns>
        public static string GetLocalizedName(this ContentDifficulty contentDifficulty) => contentDifficulty switch
        {
            ContentDifficulty.Normal => Strings.ContentDifficulty_Normal,
            ContentDifficulty.Hard => Strings.ContentDifficulty_Hard,
            ContentDifficulty.Extreme => Strings.ContentDifficulty_Extreme,
            ContentDifficulty.Savage => Strings.ContentDifficulty_Savage,
            ContentDifficulty.Ultimate => Strings.ContentDifficulty_Ultimate,
            ContentDifficulty.Unreal => Strings.ContentDifficulty_Unreal,
            _ => throw new ArgumentOutOfRangeException(nameof(contentDifficulty), contentDifficulty, null)
        };
    }
}