using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Info;

namespace Sirensong.Game.Helpers
{
    public static class FriendsListHelper
    {
        /// <summary>
        ///     Scans the current player friends list to find all friends.
        /// </summary>
        /// <returns>A list of <see cref="InfoProxyCommonList.CharacterData" />.</returns>
        public static unsafe List<InfoProxyCommonList.CharacterData> GetFriendsList()
        {
            var list = new List<InfoProxyCommonList.CharacterData>();
            var friendAgent = AgentFriendList.Instance();
            for (uint i = 0; i < friendAgent->Count; i++)
            {
                var friend = friendAgent->GetFriend(i);
                if (friend != null)
                {
                    list.Add(*friend);
                }
            }
            return list;
        }
    }
}