using System;
using ImGuiNET;

namespace Sirensong.UserInterface.Components
{
    /// <summary>
    ///     A collection of UI elements for ImGui.
    /// </summary>
    public static partial class SiUIComponents
    {
        /// <summary>
        ///     A version string component.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="commitHash">The commit hash.</param>
        public static void Version(Version version, string? commitHash = null)
        {
            var text = $"v{version}{(commitHash != null ? $" (#{commitHash})" : string.Empty)}";
            var textSize = ImGui.CalcTextSize(text);

            ImGui.SetCursorPosX((ImGui.GetWindowSize().X / 2) - (textSize.X / 2));
            ImGui.TextDisabled(text);
        }
    }
}