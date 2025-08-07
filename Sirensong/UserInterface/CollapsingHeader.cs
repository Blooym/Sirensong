using Dalamud.Bindings.ImGui;
using Sirensong.UserInterface.Style;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string)" /> element.
        /// </summary>
        /// <param name="label">The label of the collapsible header.</param>
        /// <returns>Whether the collapsible header is open.</returns>
        public static bool CollapsingHeader(string label)
        {
            var header = ImGui.CollapsingHeader(label);
            ImGui.Dummy(Spacing.InnerCollapsibleHeaderSpacing);
            return header;
        }

        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string, ImGuiTreeNodeFlags)" /> element.
        /// </summary>
        /// <param name="label">The label of the collapsible header.</param>
        /// <param name="flags">The flags of the collapsible header.</param>
        /// <returns>Whether the collapsible header is open.</returns>
        public static bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags)
        {
            var header = ImGui.CollapsingHeader(label, flags);
            ImGui.Dummy(Spacing.InnerCollapsibleHeaderSpacing);
            return header;
        }

        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string, ref bool)" /> element.
        /// </summary>
        /// <param name="label">The label of the collapsible header.</param>
        /// <param name="open">The reference to the open state of the collapsible header.</param>
        /// <returns>Whether the collapsible header is open.</returns>
        public static bool CollapsingHeader(string label, ref bool open)
        {
            var header = ImGui.CollapsingHeader(label, ref open);
            ImGui.Dummy(Spacing.InnerCollapsibleHeaderSpacing);
            return header;
        }

        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string, ref bool, ImGuiTreeNodeFlags)" /> element.
        /// </summary>
        /// <param name="label">The label of the collapsible header.</param>
        /// <param name="open">The reference to the open state of the collapsible header.</param>
        /// <param name="flags">The flags of the collapsible header.</param>
        /// <returns>Whether the collapsible header is open.</returns>
        public static bool CollapsingHeader(string label, ref bool open, ImGuiTreeNodeFlags flags)
        {
            var header = ImGui.CollapsingHeader(label, ref open, flags);
            ImGui.Dummy(Spacing.InnerCollapsibleHeaderSpacing);
            return header;
        }
    }
}
