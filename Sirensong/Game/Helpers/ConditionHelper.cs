using Dalamud.Game.ClientState.Conditions;
using Sirensong.Game.Enums;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for interacting with player condition flags.
    /// </summary>
    public static class ConditionHelper
    {
        /// <summary>
        ///     Returns if the given condition flag is set.
        /// </summary>
        /// <param name="flag">The condition flag to check.</param>
        /// <returns>True if the flag is set, false otherwise.</returns>
        public static bool HasFlag(ConditionFlag flag) => SharedServices.Condition[flag];

        /// <summary>
        ///     Returns if the player is bound by duty.
        /// </summary>
        /// <returns>True if bound by duty, false otherwise.</returns>
        public static bool IsBoundByDuty()
        {
            if (IsInIslandSanctuary())
            {
                return false;
            }

            return HasFlag(ConditionFlag.BoundByDuty) ||
                HasFlag(ConditionFlag.BoundByDuty56) ||
                HasFlag(ConditionFlag.BoundByDuty95);
        }

        /// <summary>
        ///     Returns if the player is in an island sanctuary.
        /// </summary>
        /// <returns>True if in an island sanctuary, false otherwise.</returns>
        public static bool IsInIslandSanctuary()
        {
            if (SharedServices.ClientState.TerritoryType is 0)
            {
                return false;
            }
            var territoryInfo = SharedServices.TerritoryTypeSheet.GetRow(SharedServices.ClientState.TerritoryType);
            return territoryInfo.TerritoryIntendedUse.RowId == (byte)TerritoryIntendedUseType.IslandSanctuary;
        }

        /// <summary>
        ///     Returns if the player is in a cutscene.
        /// </summary>
        /// <returns>True if in a cutscene, false otherwise.</returns>
        public static bool IsInCutscene()
            => HasFlag(ConditionFlag.OccupiedInCutSceneEvent) ||
                HasFlag(ConditionFlag.WatchingCutscene) ||
                HasFlag(ConditionFlag.WatchingCutscene78);

        /// <summary>
        ///     Returns if the player is currently moving between areas.
        /// </summary>
        /// <returns>True if moving between areas, false otherwise.</returns>
        public static bool IsBetweenAreas()
            => HasFlag(ConditionFlag.BetweenAreas) ||
                HasFlag(ConditionFlag.BetweenAreas51);

        /// <summary>
        ///     Returns if the player is currently crafting.
        /// </summary>
        /// <returns>True if crafting, false otherwise.</returns>
        public static bool IsCrafting
            => HasFlag(ConditionFlag.Crafting) ||
                HasFlag(ConditionFlag.ExecutingCraftingAction) ||
                HasFlag(ConditionFlag.PreparingToCraft);
    }
}
