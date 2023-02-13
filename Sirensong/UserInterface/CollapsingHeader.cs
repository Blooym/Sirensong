using ImGuiNET;
using Sirensong.UserInterface.Style;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string)" /> element with a <see cref="Spacing.HeaderSpacing" />.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool CollapsingHeader(string label)
        {
            var header = ImGui.CollapsingHeader(label);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }

        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string, ImGuiTreeNodeFlags)" /> element with a
        ///     <see cref="Spacing.HeaderSpacing" />.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags)
        {
            var header = ImGui.CollapsingHeader(label, flags);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }

        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string, ref bool)" /> element with a <see cref="Spacing.HeaderSpacing" />.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="open"></param>
        /// <returns></returns>
        public static bool CollapsingHeader(string label, ref bool open)
        {
            var header = ImGui.CollapsingHeader(label, ref open);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }

        /// <summary>
        ///     A <see cref="ImGui.CollapsingHeader(string, ref bool, ImGuiTreeNodeFlags)" /> element with a
        ///     <see cref="Spacing.HeaderSpacing" />.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="open"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool CollapsingHeader(string label, ref bool open, ImGuiTreeNodeFlags flags)
        {
            var header = ImGui.CollapsingHeader(label, ref open, flags);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }
    }
}
