using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     Centers the next ImGui item
        /// </summary>
        /// <param name="itemSize"></param>
        public static void CenterNext(float itemSize) => ImGui.SetCursorPosX((ImGui.GetWindowWidth() - itemSize) / 2);
    }
}