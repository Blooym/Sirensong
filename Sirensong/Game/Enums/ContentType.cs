namespace Sirensong.Game.Enums
{
    /// <summary>
    ///     Represents a ContentType, mapping from <see cref="ContentFinderCondition.ContentType" />.
    /// </summary>
    public enum ContentType
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
        DonderousTails = 24,
        CustomDeliveries = 25,
        Eureka = 26,
        Unknown4 = 27,
        SouthernFront = 29,
        VariantCriteonDungeon = 30,
    }
}