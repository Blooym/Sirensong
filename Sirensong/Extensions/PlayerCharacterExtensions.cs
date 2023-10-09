using System;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Sirensong.Game.Enums;

namespace Sirensong.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="PlayerCharacter" />.
    /// </summary>
    public static class PlayerCharacterExtensions
    {
        /// <summary>
        ///     Casts a <see cref="PlayerCharacter" /> to a <see cref="FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject" />.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static unsafe FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject* ToCsGameObject(this PlayerCharacter pc)
            => (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)pc.Address;

        public static unsafe FFXIVClientStructs.FFXIV.Client.Game.Character.Character* ToCSPlayerCharacter(this PlayerCharacter pc)
            => (FFXIVClientStructs.FFXIV.Client.Game.Character.Character*)pc.Address;

        /// <summary>
        ///     Casts a <see cref="PlayerCharacter" /> to a <see cref="GameObject" />.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns>The Dalamud GameObject or null if unable to create one.</returns>
        public static GameObject? ToDalamudGameObject(this PlayerCharacter pc) => SharedServices.ObjectTable.CreateObjectReference(pc.Address);

        /// <summary>
        ///     Set the player as the <see cref="IClientState.LocalPlayer" />'s target.
        /// </summary>
        /// <param name="pc"></param>
        public static void Target(this PlayerCharacter pc) => SharedServices.TargetManager.Target = pc.ToDalamudGameObject();

        /// <summary>
        ///     Set the player as the <see cref="IClientState.LocalPlayer" />'s reticle target.
        /// </summary>
        /// <param name="pc"></param>
        public static void SoftTarget(this PlayerCharacter pc) => SharedServices.TargetManager.SoftTarget = pc.ToDalamudGameObject();

        /// <summary>
        ///     Set the player as the <see cref="IClientState.LocalPlayer" />'s focus target.
        /// </summary>
        /// <param name="pc"></param>
        public static void FocusTarget(this PlayerCharacter pc) => SharedServices.TargetManager.FocusTarget = pc.ToDalamudGameObject();

        /// <summary>
        ///     Set the player as the <see cref="IClientState.LocalPlayer" />'s mouse over target.
        /// </summary>
        /// <param name="pc"></param>
        public static void MouseOverTarget(this PlayerCharacter pc) => SharedServices.TargetManager.MouseOverTarget = pc.ToDalamudGameObject();

        /// <summary>
        ///     Gets a boolean value indicating whether the player is from the current world.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns>True if the player is from the current world, false otherwise.</returns>
        public static bool IsFromCurrentWorld(this PlayerCharacter pc) => pc.CurrentWorld == pc.HomeWorld;

        /// <summary>
        ///     Gets a boolean value indicating whether the player is from the current datacenter.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns>True if the player is from the current datacenter, false otherwise.</returns>
        public static bool IsFromCurrentDatacenter(this PlayerCharacter pc) => pc.CurrentWorld.GameData?.DataCenter == pc.HomeWorld.GameData?.DataCenter;

        /// <summary>
        ///     Opens the player's adventurer plate or "chara card".
        /// </summary>
        /// <param name="pc"></param>
        public static unsafe void OpenCharaCard(this PlayerCharacter pc) => AgentCharaCard.Instance()->OpenCharaCard(pc.ToCsGameObject());

        /// <summary>
        ///     Opens the player's examine window.
        /// </summary>
        /// <param name="pc"></param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if unable to create a
        ///     <see cref="GameObject" /> from the <see cref="PlayerCharacter" /> address.
        /// </exception>
        public static unsafe void OpenExamine(this PlayerCharacter pc) => AgentInspect.Instance()->ExamineCharacter(pc.ObjectId);

        /// <summary>
        ///     Gets a boolean value indicating whether the player has the specified online status.
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="status">The status to check for.</param>
        /// <returns></returns>
        public static bool HasOnlineStatus(this PlayerCharacter pc, OnlineStatusType status) => pc.OnlineStatus.Id == (uint)status;

        /// <inheritdoc cref="HasOnlineStatus(PlayerCharacter, OnlineStatusType)" />
        public static bool HasOnlineStatus(this PlayerCharacter pc, uint status) => pc.OnlineStatus.Id == status;
    }
}
