using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using Sirensong.UserInterface.Style;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     Draws coloured text that wraps.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="text"></param>
        public static void TextWrappedColoured(Vector4 colour, string text)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            ImGui.TextWrapped(text);
            ImGui.PopStyleColor();
        }

        /// <summary>
        ///     Draws a text heading.
        /// </summary>
        /// <param name="text"></param>
        public static void TextHeading(string text)
        {
            ImGui.TextWrapped(text);
            ImGui.Separator();
            ImGui.Dummy(Spacing.HeaderSpacing);
        }

        /// <summary>
        ///     Draws a text footer.
        /// </summary>
        /// <param name="text"></param>
        public static unsafe void TextFooter(string text)
        {
            ImGui.Dummy(Spacing.FooterSpacing);
            ImGui.Separator();
            ImGui.TextDisabled(text);
        }
    }
}