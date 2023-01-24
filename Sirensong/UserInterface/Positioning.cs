using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     Centers the next ImGui item
        /// </summary>
        /// <param name="itemSize"></param>
        public static void CenterNext(Vector4 itemSize) => ImGui.SetCursorPosX((ImGui.GetWindowWidth() - itemSize.X) / 2);
    }
}