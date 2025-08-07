using Dalamud.Bindings.ImGui;

namespace Sirensong.UserInterface.Windowing
{
    /// <summary>
    ///     A collection of <see cref="ImGuiWindowFlags" /> that are not included in ImGui.NET.
    /// </summary>
    public static class ImGuiWindowFlagExtra
    {
        /// <summary>
        ///     A window that cannot be moved or resized.
        /// </summary>
        public const ImGuiWindowFlags LockedPosAndSize = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;

        /// <summary>
        ///     Prevents the window from scrolling with a mouse or scrollbar.
        /// </summary>
        public const ImGuiWindowFlags NoScroll = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
    }
}
