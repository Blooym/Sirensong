using System;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Info;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for interacting with the player friends.
    /// </summary>
    public static unsafe class FriendHelper
    {
        /// <summary>
        ///     Gets the local player's friends list.
        /// </summary>
        /// <returns>A list of <see cref="InfoProxyCommonList.CharacterData" />.</returns>
        public static unsafe ReadOnlySpan<InfoProxyCommonList.CharacterData> FriendList
        {
            get
            {
                if (!SharedServices.ClientState.IsLoggedIn)
                {
                    return Array.Empty<InfoProxyCommonList.CharacterData>();
                }

                var friendAgent = AgentFriendList.Instance();
                return friendAgent->InfoProxy->InfoProxyCommonList.CharDataSpan;
            }
        }

        /// <summary>
        ///     Checks if a <see cref="Character" /> is a friend.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        [Obsolete("Use FFXIVClientStructs.FFXIV.Client.Game.Character.Character#IsFriend instead")]
        public static unsafe bool IsFriend(Character* character) => character->IsFriend;
    }
}