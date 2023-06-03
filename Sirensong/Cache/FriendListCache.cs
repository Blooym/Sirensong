using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.UI.Info;
using Sirensong.Game.Helpers;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    [SirenServiceClass]
    public sealed class FriendListCache
    {
        /// <summary>
        ///     The cached list of friends for when they are not available (e.g. instances).
        /// </summary>
        private IList<InfoProxyCommonList.CharacterData> friendsListCache = Array.Empty<InfoProxyCommonList.CharacterData>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="FriendListCache" /> class.
        /// </summary>
        private FriendListCache()
        {

        }

        /// <summary>
        ///     Gets the cached version of the local player's friends list.
        /// </summary>
        /// <remarks>
        ///     This is cached because the friends list is not available in instances.
        ///     If the player is not logged in then the cache is cleared and an empty list is returned.
        /// </remarks>
        /// <returns>A list of <see cref="InfoProxyCommonList.CharacterData" />.</returns>
        public IList<InfoProxyCommonList.CharacterData> List
        {
            get
            {
                if (!SharedServices.ClientState.IsLoggedIn)
                {
                    this.friendsListCache = Array.Empty<InfoProxyCommonList.CharacterData>();
                    return Array.Empty<InfoProxyCommonList.CharacterData>();
                }

                var friendsList = FriendHelper.FriendList;
                if (!friendsList.Any())
                {
                    return this.friendsListCache;
                }

                this.friendsListCache = friendsList;
                return friendsList;
            }
        }
    }
}