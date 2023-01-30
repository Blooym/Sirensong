using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Sirensong.Game.Enums;
using Sirensong.Game.State;

namespace Sirensong.Game.Extensions
{
    /// <summary>
    /// Extensions for <see cref="PlayerCharacter"/>.
    /// </summary>
    public static class PlayerCharacterExtensions
    {
        /// <summary>
        /// Creates a <see cref="Dalamud.Game.ClientState.Objects.Types.GameObject"/> from the <see cref="PlayerCharacter"/> address.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns>The Dalamud GameObject or null if unable to create one.</returns>
        public static Dalamud.Game.ClientState.Objects.Types.GameObject? ToDalamudGameObject(this PlayerCharacter pc)
            => SharedServices.ObjectTable.CreateObjectReference(pc.Address);

        /// <summary>
        /// Creates a <see cref="FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject"/> from the <see cref="PlayerCharacter"/> address.
        /// </summary>  
        /// <param name="pc"></param>
        /// <returns></returns>
        public static unsafe FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject* ToCSGameObject(this PlayerCharacter pc)
            => (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)pc.Address;

        /// <summary>
        /// Set the player as the <see cref="ClientState.LocalPlayer"/>'s target.
        /// </summary>
        /// <param name="pc"></param>
        public static unsafe void Target(this PlayerCharacter pc) => SharedServices.TargetManager.SetTarget(pc.ToDalamudGameObject());

        /// <summary>
        /// Set the player as the <see cref="ClientState.LocalPlayer"/>'s reticle target.
        /// </summary>
        /// <param name="pc"></param>
        public static unsafe void SoftTarget(this PlayerCharacter pc) => SharedServices.TargetManager.SetSoftTarget(pc.ToDalamudGameObject());

        /// <summary>
        /// Set the player as the <see cref="ClientState.LocalPlayer"/>'s focus target.
        /// </summary>
        /// <param name="pc"></param>
        public static unsafe void FocusTarget(this PlayerCharacter pc) => SharedServices.TargetManager.SetFocusTarget(pc.ToDalamudGameObject());

        /// <summary>
        /// Set the player as the <see cref="ClientState.LocalPlayer"/>'s mouseover target.
        /// </summary>
        /// <param name="pc"></param>
        public static unsafe void MOTarget(this PlayerCharacter pc) => SharedServices.TargetManager.SetMouseOverTarget(pc.ToDalamudGameObject());

        /// <summary>
        /// Gets a boolean value indicating whether the player is from the current world.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns>True if the player is from the current world, false otherwise.</returns>
        public static bool IsFromCurrentWorld(this PlayerCharacter pc) => pc.CurrentWorld == pc.HomeWorld;

        /// <summary>
        /// Gets a boolean value indicating whether the player is from the current datacenter.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns>True if the player is from the current datacenter, false otherwise.</returns>
        public static bool IsFromCurrentDatacenter(this PlayerCharacter pc) => pc.CurrentWorld.GameData?.DataCenter == pc.HomeWorld.GameData?.DataCenter;

        /// <summary>
        /// Opens the player's adventurer plate.
        /// </summary>
        /// <param name="pc"></param>
        public static unsafe void OpenCharaCard(this PlayerCharacter pc) => AgentCharaCard.Instance()->OpenCharaCard(pc.ToCSGameObject());

        /// <summary>
        /// Opens the player's examine window.
        /// </summary>
        /// <param name="pc"></param>
        /// <exception cref="InvalidOperationException">Thrown if unable to create a <see cref="Dalamud.Game.ClientState.Objects.Types.GameObject"/> from the <see cref="PlayerCharacter"/> address.</exception>
        public static unsafe void OpenExamine(this PlayerCharacter pc) => AgentInspect.Instance()->ExamineCharacter(pc.ObjectId);

        /// <summary>
        /// Gets a boolean value indicating whether the player has the specified online status.
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="status">The status to check for.</param>
        /// <returns></returns>
        public static bool HasOnlineStatus(this PlayerCharacter pc, OnlineStatusType status) => OnlineStatus.HasStatus(pc, status);

        /// <inheritdoc cref="HasOnlineStatus(PlayerCharacter, OnlineStatusType)"/>
        public static bool HasOnlineStatus(this PlayerCharacter pc, uint status) => OnlineStatus.HasStatus(pc, status);
    }
}
