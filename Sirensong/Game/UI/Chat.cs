using Dalamud.Game.Text.SeStringHandling;
using Sirensong.Game.Enums;

namespace Sirensong.Game.UI
{
    /// <summary>
    ///     Methods to format and show in-game chat messages.
    /// </summary>
    public static class Chat
    {
        /// <summary>
        ///     Creates a base <see cref="SeStringBuilder"/> with the plugin name prepended.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <param name="pluginTagColour">The colour of the plugin name.</param>
        /// <returns></returns>
        private static SeStringBuilder CreateBaseString(string pluginName, ushort pluginTagColour)
            => new SeStringBuilder()
                .AddUiForeground(pluginTagColour)
                .AddText($"[{pluginName}]")
                .AddUiForegroundOff()
                .AddText(" ");

        /// <summary>
        ///     Prints a message to the chat log with the plugin name prepended.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="pluginTagColour">The colour of the plugin name tag.</param>
        public static void Print(string message, ushort pluginTagColour = (ushort)ChatUiColourKey.LightBlue2)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                .AddText(message)
                .Build()
            );

        /// <summary>
        ///     Prints an error message to the chat log with the plugin name prepended.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pluginTagColour"></param>
        public static void PrintError(string message, ushort pluginTagColour = (ushort)ChatUiColourKey.LightBlue2)
            => SharedServices.ChatGui.PrintError(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                .AddText(message)
                .Build()
            );

        /// <summary>
        ///     Prints a message to the chat log with the plugin name prepended.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="pluginTagColour"></param>
        public static void Print(SeString message, ushort pluginTagColour = (ushort)ChatUiColourKey.LightBlue2)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                .Append(message)
                .Build()
            );

        /// <summary>
        ///     Prints an error message to the chat log with the plugin name prepended.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="pluginTagColour"></param>
        public static void PrintError(SeString message, ushort pluginTagColour = (ushort)ChatUiColourKey.LightBlue2)
            => SharedServices.ChatGui.PrintError(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                .Append(message)
                .Build()
            );
    }
}