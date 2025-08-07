using Dalamud.Bindings.ImGui;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     Adds a tooltip to the last element if it is hovered.
        /// </summary>
        /// <param name="tooltip">The tooltip text.</param>
        public static void AddTooltip(string tooltip)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(tooltip);
            }
        }
    }
}
