using System;
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
                    return [];
                }

                var friendAgent = AgentFriendlist.Instance();
                return friendAgent->InfoProxy->InfoProxyCommonList.CharDataSpan;
            }
        }
    }
}