using Dalamud.Interface;
using Dalamud.Interface.Internal.Notifications;
using Sirensong.Game;
using Sirensong.Game.Enums;

namespace Sirensong.UserInterface
{
    /// <summary>
    ///     A collection of UI elements for ImGui.
    /// </summary>
    public static partial class SiUI
    {
        /// <summary>
        ///     Shows a toast notification, wrapper for <see cref="UiBuilder.AddNotification(string, string?, NotificationType, uint)"/>
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="type">The type of notification.</param>
        /// <param name="msDelay">The delay in milliseconds before the notification disappears.</param>
        /// <param name="sfx">The sound effect to play.</param>
        private static void HandleToast(string message, NotificationType type, uint msDelay = 3000, SoundEffect? sfx = null)
        {
            SharedServices.UiBuilder.AddNotification(message, SirenCore.InitializerName, type, msDelay);
            if (sfx != null)
            {
                PlaySound.Invoke(sfx.Value, 0, 0);
            }
        }

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint,SoundEffect?)"/>
        public static void ShowToast(string message) => HandleToast(message, NotificationType.None, default, default);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint,SoundEffect?)"/>
        public static void ShowToast(string message, NotificationType type) => HandleToast(message, type, default, default);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint,SoundEffect?)"/>
        public static void ShowToast(string message, NotificationType type, uint msDelay) => HandleToast(message, type, msDelay, default);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint,SoundEffect?)"/>
        public static void ShowToast(string message, NotificationType type, SoundEffect sfx) => HandleToast(message, type, default, sfx);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint,SoundEffect?)"/>
        public static void ShowToast(string message, NotificationType type, uint msDelay, SoundEffect sfx) => HandleToast(message, type, msDelay, sfx);
    }
}
