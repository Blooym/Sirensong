using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using Sirensong.UserInterface.Style;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        public static void Text(string text) => ImGui.TextUnformatted(text);

        public static void TextWrapped(string text) => ImGui.TextWrapped(text);

        public static void TextColoured(Vector4 colour, string text) => ImGui.TextColored(colour, text);

        public static void TextDisabled(string text) => ImGui.TextDisabled(text);

        public static void TextWrappedColoured(Vector4 colour, string text)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            ImGui.TextWrapped(text);
            ImGui.PopStyleColor();
        }

        public static void Heading(string text)
        {
            ImGui.TextDisabled(text);
            ImGui.Separator();
            ImGui.Dummy(Spacing.HeaderSpacing);
        }

        public static unsafe void Footer(string text)
        {
            ImGui.Dummy(Spacing.FooterSpacing);
            ImGui.Separator();
            ImGui.TextDisabled(text);
        }
    }
}