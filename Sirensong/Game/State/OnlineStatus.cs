using Dalamud.Game.ClientState.Objects.SubKinds;
using Sirensong.Game.Enums;

namespace Sirensong.Game.State
{
    public static class OnlineStatus
    {
        /// <summary>
        /// Whether or not the player has the given online status.
        /// </summary>
        /// <param name="pc">The player character to check.</param>
        /// <param name="status">The online status to check for.</param>
        /// <returns>True if the player has the given online status, false otherwise.</returns>
        public static bool HasStatus(PlayerCharacter pc, OnlineStatusType status) => pc?.OnlineStatus.Id == (uint)status;

        /// <inheritdoc cref="HasStatus(PlayerCharacter,OnlineStatusType)"/>
        public static bool HasStatus(PlayerCharacter pc, uint status) => pc?.OnlineStatus.Id == status;

        /// <summary>
        /// Whether or not the local player has the given online status.
        /// </summary>
        /// <param name="status">The online status to check for.</param>
        /// <returns>True if the local player has the given online status, false otherwise.</returns>
        public static bool HasStatus(OnlineStatusType status) => HasStatus(SharedServices.ClientState.LocalPlayer!, status);

        /// <inheritdoc cref="HasStatus(OnlineStatusType)"/>
        public static bool HasStatus(uint status) => HasStatus(SharedServices.ClientState.LocalPlayer!, status);
    }
}