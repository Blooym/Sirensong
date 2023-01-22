using ImGuiNET;

namespace Sirensong.UserInterface
{
    /// <summary>
    ///     A collection of UI elements for ImGui.
    /// </summary>
    public static partial class SiUI
    {
        /// <summary>
        ///     Adds a tooltip to the last item when hovered.
        /// </summary>
        /// <param name="tooltip">The tooltip text.</param>
        public static void TooltipLast(string tooltip)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(tooltip);
            }
        }
    }
}
