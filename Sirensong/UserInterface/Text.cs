using System.Numerics;
using ImGuiNET;
using Sirensong.UserInterface.Style;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        /// <see cref="ImGui.Text(string)"/> without the formatting.
        /// </summary>
        /// <param name="text"></param>
        public static void Text(string text) => ImGui.TextUnformatted(text);

        /// <summary>
        /// <see cref="ImGui.TextWrapped(string)"/> without the formatting.
        /// </summary>
        /// <param name="text"></param>
        public static void TextWrapped(string text)
        {
            var wrapPoint = ImGui.GetContentRegionAvail().X;
            ImGui.PushTextWrapPos(wrapPoint);
            ImGui.TextUnformatted(text);
            ImGui.PopTextWrapPos();
        }

        /// <summary>
        /// <see cref="ImGui.TextColored(Vector4, string)"/> without the formatting.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="text"></param>
        public static void TextColoured(Vector4 colour, string text)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            ImGui.TextUnformatted(text);
            ImGui.PopStyleColor();
        }

        /// <summary>
        /// <see cref="ImGui.TextDisabled(string)"/> without the formatting.
        /// </summary>
        /// <param name="text"></param>
        public static unsafe void TextDisabled(string text)
        {
            var disabledCol = ImGui.GetStyleColorVec4(ImGuiCol.TextDisabled);
            ImGui.PushStyleColor(ImGuiCol.Text, *disabledCol);
            ImGui.TextUnformatted(text);
            ImGui.PopStyleColor();
        }

        /// <summary>
        /// <see cref="ImGui.TextDisabled(string)"/> wrapped without the formatting.
        /// </summary>
        /// <param name="text"></param>
        public static unsafe void TextDisabledWrapped(string text)
        {
            var disabledCol = ImGui.GetStyleColorVec4(ImGuiCol.TextDisabled);
            ImGui.PushStyleColor(ImGuiCol.Text, *disabledCol);
            TextWrapped(text);
            ImGui.PopStyleColor();
        }

        /// <summary>
        /// <see cref="ImGui.TextWrapped(string)"/> with a custom colour.
        /// </summary>
        /// <param name="colour"></param>
        public static void TextWrappedColoured(Vector4 colour, string text)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            TextWrapped(text);
            ImGui.PopStyleColor();
        }

        /// <summary>
        /// A heading with a separator and spacing below.
        /// </summary>
        /// <param name="text"></param>
        public static void Heading(string text)
        {
            TextDisabled(text);
            ImGui.Separator();
            ImGui.Dummy(Spacing.HeaderSpacing);
        }

        /// <summary>
        /// A footer with a separator and spacing above.
        /// </summary>
        /// <param name="text"></param>
        public static unsafe void Footer(string text)
        {
            ImGui.Dummy(Spacing.FooterSpacing);
            ImGui.Separator();
            TextDisabled(text);
        }
    }
}