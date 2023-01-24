using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using Sirensong.Extensions;

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

        /// <summary>
        ///     Draws a text heading.
        /// </summary>
        /// <param name="text"></param>
        public static unsafe void TextHeading(string text)
        {
            var textDisabled = *(Vector4*)ImGui.GetStyleColorVec4(ImGuiCol.TextDisabled);

            ImGui.PushStyleColor(ImGuiCol.Text, textDisabled.ToUint());
            ImGui.TextUnformatted(text);
            ImGui.PopStyleColor();
            ImGui.Separator();
        }
    }
}