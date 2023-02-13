using Dalamud.Interface;
using Dalamud.Interface.Internal.Notifications;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     Shows a toast notification, wrapper for
        ///     <see cref="UiBuilder.AddNotification(string, string?, NotificationType, uint)" />
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="type">The type of notification.</param>
        /// <param name="msDelay">The delay in milliseconds before the notification disappears.</param>
        private static void HandleToast(string message, NotificationType type, uint msDelay = 3000) => SharedServices.UiBuilder.AddNotification(message, SirenCore.InitializerName, type, msDelay);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint)" />
        public static void ShowToast(string message) => HandleToast(message, NotificationType.None);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint)" />
        public static void ShowToast(string message, NotificationType type) => HandleToast(message, type);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint)" />
        public static void ShowToast(string message, uint msDelay) => HandleToast(message, default, msDelay);

        /// <inheritdoc cref="HandleToast(string,NotificationType,uint)" />
        public static void ShowToast(string message, NotificationType type, uint msDelay) => HandleToast(message, type, msDelay);
    }
}
