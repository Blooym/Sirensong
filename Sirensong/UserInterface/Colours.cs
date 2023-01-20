using Dalamud.Interface.Colors;
using FFXIVClientStructs.FFXIV.Common.Math;

namespace Sirensong.UserInterface
{
    public static partial class SiUI
    {
        /// <summary>
        ///     Gets the <see cref="ImGuiColors"> colour from the given role ID.
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static Vector4 GetColourForRole(uint roleID) => roleID switch
        {
            1 => ImGuiColors.TankBlue,
            2 => ImGuiColors.DPSRed,
            3 => ImGuiColors.DPSRed,
            4 => ImGuiColors.HealerGreen,
            _ => ImGuiColors.DalamudGrey
        };
    }
}