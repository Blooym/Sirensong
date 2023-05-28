using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Info;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for interacting with the player friends.
    /// </summary>
    public static class FriendHelper
    {
        /// <summary>
        ///     Gets the local player's friends list.
        /// </summary>
        /// <returns>A list of <see cref="InfoProxyCommonList.CharacterData" />.</returns>
        public static unsafe IList<InfoProxyCommonList.CharacterData> FriendList
        {
            get
            {
                if (!SharedServices.ClientState.IsLoggedIn)
                {
                    return Array.Empty<InfoProxyCommonList.CharacterData>();
                }

                var friendAgent = AgentFriendList.Instance();
                var friends = new List<InfoProxyCommonList.CharacterData>();
                if (friendAgent == null || friendAgent->Count == 0)
                {
                    return Array.Empty<InfoProxyCommonList.CharacterData>();
                }

                for (uint i = 0; i < friendAgent->Count; i++)
                {
                    var friend = friendAgent->GetFriend(i);
                    if (friend != null)
                    {
                        friends.Add(*friend);
                    }
                }

                return friends;
            }
        }

        /// <summary>
        ///     Checks if a <see cref="PlayerCharacter" /> is a friend.
        /// </summary>
        /// <param name="playerCharacter"></param>
        /// <returns></returns>
        public static unsafe bool IsFriend(this PlayerCharacter playerCharacter)
        {
            if (playerCharacter == null)
            {
                return false;
            }

            var friendList = FriendList;
            if (!friendList.Any())
            {
                return false;
            }

            var isFriend = false;
            foreach (var friend in friendList)
            {
                if (MemoryHelper.ReadSeStringNullTerminated((IntPtr)friend.Name).TextValue == playerCharacter.Name.TextValue &&
                    friend.HomeWorld == playerCharacter.HomeWorld.Id)
                {
                    isFriend = true;
                    break;
                }
            }
            return isFriend;
        }
    }
}