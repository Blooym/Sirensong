using Dalamud.Game.Gui.Toast;

namespace Sirensong.Game
{
    /// <summary>
    /// Methods to show in-game toast notifications.
    /// </summary>
    public static class GameToast
    {
        /// <summary>
        /// Wrapper for <see cref="ToastGui.ShowQuest(string, QuestToastOptions)"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowQuestToast(string message, QuestToastOptions options) => SharedServices.ToastGui.ShowQuest(message, options);

        /// <summary>
        /// Wrapper for <see cref="ToastGui.ShowQuest(Dalamud.Game.Text.SeStringHandling.SeString, QuestToastOptions)"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowQuestToast(Dalamud.Game.Text.SeStringHandling.SeString message, QuestToastOptions options) => SharedServices.ToastGui.ShowQuest(message, options);

        /// <summary>
        /// Wrapper for <see cref="ToastGui.ShowNormal(string, ToastOptions)"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        /// <param name="sfx">The sound effect to play.</param>
        public static void ShowNormalToast(string message, ToastOptions options) => SharedServices.ToastGui.ShowNormal(message, options);

        /// <summary>
        /// Wrapper for <see cref="ToastGui.ShowNormal(Dalamud.Game.Text.SeStringHandling.SeString, ToastOptions)"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="options">The options for the toast.</param>
        public static void ShowNormalToast(Dalamud.Game.Text.SeStringHandling.SeString message, ToastOptions options) => SharedServices.ToastGui.ShowNormal(message, options);

        /// <summary>
        /// Wrapper for <see cref="ToastGui.ShowError(string)"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void ShowErrorToast(string message) => SharedServices.ToastGui.ShowError(message);

        /// <summary>
        /// Wrapper for <see cref="ToastGui.ShowError(Dalamud.Game.Text.SeStringHandling.SeString)"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void ShowErrorToast(Dalamud.Game.Text.SeStringHandling.SeString message) => SharedServices.ToastGui.ShowError(message);
    }
}