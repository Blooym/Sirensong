using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        /// Adds a tooltip to the last item when hovered.
        /// </summary>
        /// <param name="tooltip">The tooltip text.</param>
        public static void TooltipLast(string tooltip)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(tooltip);
            }
        }

        /// <summary>
        /// Creates a label with a tooltip.
        /// </summary>
        /// <param name="label">The label text</param>
        /// <param name="text">The value text</param>
        /// <param name="tooltip">The tooltip text</param>
        public static void Label(string label, string text, string tooltip)
        {
            ImGui.TextUnformatted($"{label}: ");
            ImGui.SameLine();
            ImGui.TextUnformatted($"{text}*");
            TooltipLast(tooltip);
        }
    }
}
