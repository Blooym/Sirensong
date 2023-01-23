using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiUI
    {
        public static void CenterNext(Vector4 itemSize) => ImGui.SetCursorPosX((ImGui.GetWindowWidth() - itemSize.X) / 2);
    }
}