using Dalamud.Game.ClientState.Conditions;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Caching;
using Sirensong.Game.Enums;

namespace Sirensong.Game.State
{
    public static class Condition
    {
        /// <summary>
        /// Returns if the given condition flag is set.
        /// </summary>
        /// <param name="flag">The condition flag to check.</param>
        /// <returns>True if the flag is set, false otherwise.</returns>
        public static bool HasFlag(ConditionFlag flag) => SharedServices.Condition[flag];

        /// <summary>
        /// Returns if the player is bound by duty.
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
        /// Returns if the player is in an island sanctuary.
        /// </summary>
        /// <returns>True if in an island sanctuary, false otherwise.</returns>
        public static bool IsInIslandSanctuary()
        {
            var territoryInfo = SirenCore.GetOrCreateService<LuminaCacheService<TerritoryType>>().GetRow(SharedServices.ClientState.TerritoryType);
            if (territoryInfo is null)
            {
                return false;
            }
            return territoryInfo.TerritoryIntendedUse == (byte)TerritoryIntendedUseType.IslandSanctuary;
        }

        /// <summary>
        /// Returns if the player is in a cutscene.
        /// </summary>
        /// <returns>True if in a cutscene, false otherwise.</returns>
        public static bool IsInCutscene()
            => HasFlag(ConditionFlag.OccupiedInCutSceneEvent) ||
                HasFlag(ConditionFlag.WatchingCutscene) ||
                HasFlag(ConditionFlag.WatchingCutscene78);

        /// <summary>
        /// Returns if the player is currently moving between areas.
        /// </summary>
        /// <returns>True if moving between areas, false otherwise.</returns>
        public static bool IsBetweenAreas()
            => HasFlag(ConditionFlag.BetweenAreas) ||
                HasFlag(ConditionFlag.BetweenAreas51);
    }
}