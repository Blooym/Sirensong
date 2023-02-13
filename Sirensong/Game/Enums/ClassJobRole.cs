using Dalamud.Interface.Colors;
using FFXIVClientStructs.FFXIV.Common.Math;
using Lumina.Excel.GeneratedSheets;

namespace Sirensong.Game.Enums
{
    /// <summary>
    ///     A mapping of <see cref="ClassJob" /> role IDs to their names.
    /// </summary>
    public enum ClassJobRole : byte
    {
        Misc = 0,
        Tank = 1,
        MeleeDps = 2,
        RangedDps = 3,
        Healer = 4,
    }

    /// <summary>
    ///     Extensions for <see cref="ClassJobRole" />
    /// </summary>
    public static class ClassJobRoleExtensions
    {
        /// <summary>
        ///     Gets the Dalamud colour for the given role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>A Vector4 colour from the current dalamud style </returns>
        public static Vector4 GetColourForRole(this ClassJobRole roleId) => roleId switch
        {
            ClassJobRole.Tank => ImGuiColors.TankBlue,
            ClassJobRole.MeleeDps => ImGuiColors.DPSRed,
            ClassJobRole.RangedDps => ImGuiColors.DPSRed,
            ClassJobRole.Healer => ImGuiColors.HealerGreen,
            ClassJobRole.Misc => ImGuiColors.DalamudGrey,
            _ => ImGuiColors.DalamudGrey,
        };
    }
}
