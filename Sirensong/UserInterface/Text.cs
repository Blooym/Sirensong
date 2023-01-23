using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiUI
    {
        /// <summary>
        ///     Draws coloured text that wraps.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colour"></param>
        public static void TextWrappedColoured(string text, Vector4 colour)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, colour);
            ImGui.TextWrapped(text);
            ImGui.PopStyleColor();
        }
    }
}