using System.Numerics;
using Dalamud.Interface.Colors;

namespace Sirensong.UserInterface.Style
{
    /// <summary>
    /// Colour values for UI design.
    /// </summary>
    public static class Colours
    {
        /// <summary>
        /// Colour for errors.
        /// </summary>
        internal static Vector4 Error => ImGuiColors.DalamudRed;

        /// <summary>
        /// Colour for warnings.
        /// </summary>
        internal static Vector4 Warning => ImGuiColors.DalamudOrange;

        /// <summary>
        /// Colour for success.
        /// </summary>
        internal static Vector4 Success => ImGuiColors.ParsedGreen;

        /// <summary>
        /// Colour for important information.
        /// </summary>
        internal static Vector4 Important => ImGuiColors.DalamudViolet;
    }
}