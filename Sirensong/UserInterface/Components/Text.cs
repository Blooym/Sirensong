using System;
using Dalamud.Interface;
using ImGuiNET;

namespace Sirensong.UserInterface.Components
{
    /// <summary>
    ///     A collection of UI elements for ImGui.
    /// </summary>
    public static partial class SiGUIComponent
    {
        /// <summary>
        ///     A version string component.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="commitHash">The commit hash.</param>
        public static void VersionInfo(Version version, string? commitHash = null)
        {
            var text = $"v{version}{(commitHash != null ? $" (#{commitHash})" : string.Empty)}";
            ImGuiHelpers.CenterCursorForText(text);
            ImGui.TextDisabled(text);
        }
    }
}