using Dalamud.Game.Text.SeStringHandling;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods to format and show in-game chat messages.
    /// </summary>
    public static class ChatHelper
    {
        /// <summary>
        ///     Creates a base <see cref="SeStringBuilder" /> with the plugin name prepended.
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
        public static void Print(string message, ushort pluginTagColour = 707)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .AddText(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void PrintWarning(string message, ushort pluginTagColour = 707)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .AddUiForeground(706)
                    .AddText(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void PrintImportant(string message, ushort pluginTagColour = 707) => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .AddUiForeground(555)
                    .AddText(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void PrintError(string message, ushort pluginTagColour = 707) => SharedServices.ChatGui.PrintError(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .AddText(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void Print(SeString message, ushort pluginTagColour = 707)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .Append(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void PrintWarning(SeString message, ushort pluginTagColour = 707)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .AddUiForeground(706)
                    .Append(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void PrintImportant(SeString message, ushort pluginTagColour = 707)
            => SharedServices.ChatGui.Print(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .AddUiForeground(555)
                    .Append(message)
                    .Build()
            );

        /// <inheritdoc cref="Print(string,ushort,ushort)" />
        public static void PrintError(SeString message, ushort pluginTagColour = 706)
            => SharedServices.ChatGui.PrintError(
                CreateBaseString(SirenCore.InitializerName, pluginTagColour)
                    .Append(message)
                    .Build()
            );
    }
}
