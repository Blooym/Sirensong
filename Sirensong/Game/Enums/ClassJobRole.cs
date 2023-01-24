using Dalamud.Interface.Colors;
using FFXIVClientStructs.FFXIV.Common.Math;
using Lumina.Excel.GeneratedSheets;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Sirensong.Game.Enums
{
    /// <summary>
    ///     A mapping of <see cref="ClassJob"/> role IDs to their names.
    /// </summary>
    public enum ClassJobRole : uint
    {
        Misc = 0,
        Tank = 1,
        MeleeDPS = 2,
        RangedDPS = 3,
        Healer = 4,
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    ///     Extensions for <see cref="ClassJobRole" />
    /// </summary>
    public static class ClassJobRoleExtensions
    {
        /// <summary>
        ///     Gets the Dalamud colour for the given role.
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns>A Vector4 colour from the current dalamud style </returns>
        public static Vector4 GetColourForRole(this ClassJobRole roleID) => roleID switch
        {
            ClassJobRole.Tank => ImGuiColors.TankBlue,
            ClassJobRole.MeleeDPS => ImGuiColors.DPSRed,
            ClassJobRole.RangedDPS => ImGuiColors.DPSRed,
            ClassJobRole.Healer => ImGuiColors.HealerGreen,
            ClassJobRole.Misc => ImGuiColors.DalamudGrey,
            _ => ImGuiColors.DalamudGrey,
        };
    }
}
