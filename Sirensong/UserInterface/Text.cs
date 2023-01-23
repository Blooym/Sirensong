using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiUI
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
    }
}