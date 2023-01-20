using ImGuiNET;

namespace Sirensong.UserInterface
{
    /// <summary>
    ///     A collection of UI elements for ImGui.
    /// </summary>
    public static partial class SiUI
    {
        /// <summary>
        ///     Adds a tooltip to the last item.
        /// </summary>
        /// <param name="text">The text to display in the tooltip.</param>
        public static void TooltipLast(string text)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted(text);
                ImGui.EndTooltip();
            }
        }
    }
}
