using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for showing in-game toast notifications.
    /// </summary>
    public static class ToastHelper
    {
        /// <summary>
        ///     Wrapper for <see cref="IToastGui.ShowQuest(string, QuestToastOptions)" />.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowQuestToast(string message, QuestToastOptions options) => SharedServices.ToastGui.ShowQuest(message, options);

        /// <summary>
        ///     Wrapper for <see cref="IToastGui.ShowQuest(SeString, QuestToastOptions)" />.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowQuestToast(SeString message, QuestToastOptions options) => SharedServices.ToastGui.ShowQuest(message, options);

        /// <summary>
        ///     Wrapper for <see cref="IToastGui.ShowNormal(string, ToastOptions)" />.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowNormalToast(string message, ToastOptions options) => SharedServices.ToastGui.ShowNormal(message, options);

        /// <summary>
        ///     Wrapper for <see cref="IToastGui.ShowNormal(SeString, ToastOptions)" />.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowNormalToast(SeString message, ToastOptions options) => SharedServices.ToastGui.ShowNormal(message, options);

        /// <summary>
        ///     Wrapper for <see cref="IToastGui.ShowError(string)" />.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void ShowErrorToast(string message) => SharedServices.ToastGui.ShowError(message);

        /// <summary>
        ///     Wrapper for <see cref="IToastGui.ShowError(SeString)" />.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void ShowErrorToast(SeString message) => SharedServices.ToastGui.ShowError(message);
    }
}
