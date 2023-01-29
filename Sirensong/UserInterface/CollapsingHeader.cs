using ImGuiNET;
using Sirensong.UserInterface.Style;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        public static bool CollapsingHeader(string label)
        {
            var header = ImGui.CollapsingHeader(label);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }

        public static bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags)
        {
            var header = ImGui.CollapsingHeader(label, flags);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }

        public static bool CollapsingHeader(string label, ref bool open)
        {
            var header = ImGui.CollapsingHeader(label, ref open);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }

        public static bool CollapsingHeader(string label, ref bool open, ImGuiTreeNodeFlags flags)
        {
            var header = ImGui.CollapsingHeader(label, ref open, flags);
            ImGui.Dummy(Spacing.HeaderSpacing);
            return header;
        }
    }
}