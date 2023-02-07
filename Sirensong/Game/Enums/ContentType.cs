using Sirensong.Resources.Localization;

namespace Sirensong.Game.Enums
{
    /// <summary>
    /// Represents a ContentType, mapping from <see cref="ContentFinderCondition.ContentType" />.
    /// </summary>
    public enum ContentType : int
    {
        /// <remarks>
        /// This isn't a ContentType from the game, but is useful to split <see cref="Raids" /> apart from alliance raids.
        /// </remarks>
        AllianceRaid = -1,

        /// <summary>
        /// This isn't a ContentType from the game, but is used when unable to determine the ContentType.
        /// </summary>
        Unknown = -2,

        Roulette = 1,
        Dungeons = 2,
        Guildhests = 3,
        Trials = 4,
        Raids = 5,
        PvP = 6,
        QuestBattles = 7,
        Fates = 8,
        TreasureHunt = 9,
        Levequests = 10,
        GrandCompany = 11,
        Companions = 12,
        TribalQuests = 13,
        OverallCompletion = 14,
        PlayerCommendation = 15,
        DisciplesOfTheLand = 16,
        DisciplesOfTheHand = 17,
        RetainerVentures = 18,
        GoldSaucer = 19,
        Unknown1 = 20,
        DeepDungeon = 21,
        Unknown2 = 22,
        Unknown3 = 23,
        WonderousTails = 24,
        CustomDeliveries = 25,
        Eureka = 26,
        Unknown4 = 27,
        SouthernFront = 29,
        VCDungeon = 30,
    }

    /// <summary>
    /// Extensions for <see cref="ContentType" />.
    /// </summary>
    public static class ContentTypeExtensions
    {
        /// <summary>
        /// Gets the localized name of the <see cref="ContentType" /> according to the current CultureInfo.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns>The localized name of the <see cref="ContentType" />.</returns>
        public static string GetLocalizedName(this ContentType contentType) => contentType switch
        {
            ContentType.AllianceRaid => Strings.ContentType_AllianceRaid,
            ContentType.Unknown => Strings.ContentType_Unknown,
            ContentType.Roulette => Strings.ContentType_Roulette,
            ContentType.Dungeons => Strings.ContentType_Dungeon,
            ContentType.Guildhests => Strings.ContentType_Guildhest,
            ContentType.Trials => Strings.ContentType_Trial,
            ContentType.Raids => Strings.ContentType_Raid,
            ContentType.PvP => Strings.ContentType_PvP,
            ContentType.QuestBattles => Strings.ContentType_QuestBattle,
            ContentType.Fates => Strings.ContentType_FATE,
            ContentType.TreasureHunt => Strings.ContentType_TreasureHunt,
            ContentType.Levequests => Strings.ContentType_Levequest,
            ContentType.GrandCompany => Strings.ContentType_GrandCompany,
            ContentType.Companions => Strings.ContentType_Companion,
            ContentType.TribalQuests => Strings.ContentType_TribalQuest,
            ContentType.OverallCompletion => Strings.ContentType_OverallCompletion,
            ContentType.PlayerCommendation => Strings.ContentType_PlayerCommendation,
            ContentType.DisciplesOfTheLand => Strings.ContentType_DiscipleOfTheLand,
            ContentType.DisciplesOfTheHand => Strings.ContentType_DiscipleOfTheHand,
            ContentType.RetainerVentures => Strings.ContentType_RetainerVenture,
            ContentType.GoldSaucer => Strings.ContentType_GoldSaucer,
            ContentType.Unknown1 => Strings.ContentType_Unknown,
            ContentType.DeepDungeon => Strings.ContentType_DeepDungeon,
            ContentType.Unknown2 => Strings.ContentType_Unknown,
            ContentType.Unknown3 => Strings.ContentType_Unknown,
            ContentType.WonderousTails => Strings.ContentType_WonderousTails,
            ContentType.CustomDeliveries => Strings.ContentType_CustomDelivery,
            ContentType.Eureka => Strings.ContentType_Eureka,
            ContentType.Unknown4 => Strings.ContentType_Unknown,
            ContentType.SouthernFront => Strings.ContentType_SouthernFront,
            ContentType.VCDungeon => Strings.ContentType_VCDungeon,
            _ => Strings.ContentType_Unknown,
        };

        /// <summary>
        /// Gets the localized plural name of the <see cref="ContentType" /> according to the current CultureInfo.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetLocalizedNamePlural(this ContentType contentType) => contentType switch
        {
            ContentType.AllianceRaid => Strings.ContentType_AllianceRaid_Plural,
            ContentType.Unknown => Strings.ContentType_Unknown_Plural,
            ContentType.Roulette => Strings.ContentType_Roulette_Plural,
            ContentType.Dungeons => Strings.ContentType_Dungeon_Plural,
            ContentType.Guildhests => Strings.ContentType_Guildhest_Plural,
            ContentType.Trials => Strings.ContentType_Trial_Plural,
            ContentType.Raids => Strings.ContentType_Raid_Plural,
            ContentType.PvP => Strings.ContentType_PvP_Plural,
            ContentType.QuestBattles => Strings.ContentType_QuestBattle_Plural,
            ContentType.Fates => Strings.ContentType_FATE_Plural,
            ContentType.TreasureHunt => Strings.ContentType_TreasureHunt_Plural,
            ContentType.Levequests => Strings.ContentType_Levequest_Plural,
            ContentType.GrandCompany => Strings.ContentType_GrandCompany_Plural,
            ContentType.Companions => Strings.ContentType_Companion_Plural,
            ContentType.TribalQuests => Strings.ContentType_TribalQuest_Plural,
            ContentType.OverallCompletion => Strings.ContentType_OverallCompletion_Plural,
            ContentType.PlayerCommendation => Strings.ContentType_PlayerCommendation_Plural,
            ContentType.DisciplesOfTheLand => Strings.ContentType_DiscipleOfTheLand_Plural,
            ContentType.DisciplesOfTheHand => Strings.ContentType_DiscipleOfTheHand_Plural,
            ContentType.RetainerVentures => Strings.ContentType_RetainerVenture_Plural,
            ContentType.GoldSaucer => Strings.ContentType_GoldSaucer_Plural,
            ContentType.Unknown1 => Strings.ContentType_Unknown_Plural,
            ContentType.DeepDungeon => Strings.ContentType_DeepDungeon_Plural,
            ContentType.Unknown2 => Strings.ContentType_Unknown_Plural,
            ContentType.Unknown3 => Strings.ContentType_Unknown_Plural,
            ContentType.WonderousTails => Strings.ContentType_WonderousTails_Plural,
            ContentType.CustomDeliveries => Strings.ContentType_CustomDelivery_Plural,
            ContentType.Eureka => Strings.ContentType_Eureka_Plural,
            ContentType.Unknown4 => Strings.ContentType_Unknown_Plural,
            ContentType.SouthernFront => Strings.ContentType_SouthernFront_Plural,
            ContentType.VCDungeon => Strings.ContentType_VCDungeon_Plural,
            _ => Strings.ContentType_Unknown_Plural,
        };
    }
}