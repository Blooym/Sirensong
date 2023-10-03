using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using Sirensong.Game.Helpers;

namespace Sirensong.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="IObjectTable" />.
    /// </summary>
    public static class IObjectTableExtensions
    {
        /// <summary>
        ///     Gets all <see cref="PlayerCharacter" />s in the <see cref="IObjectTable" />.
        /// </summary>
        /// <param name="objectTable"></param>
        /// <param name="includeSelf">Whether or not to include the local player.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="PlayerCharacter" />s.</returns>
        public static IEnumerable<PlayerCharacter> GetPlayerCharacters(this IObjectTable objectTable, bool includeSelf = true) => objectTable
            .Where(x => x is PlayerCharacter).Cast<PlayerCharacter>()
            .Where(x => includeSelf || x.ObjectId != SharedServices.ClientState.LocalPlayer?.ObjectId)
            .Where(x => x.ObjectId > 240);

        /// <summary>
        ///     Gets all nearby friend <see cref="PlayerCharacter" />s in the <see cref="IObjectTable" />.
        /// </summary>
        /// <param name="objectTable"></param>
        /// <returns></returns>
        public static IEnumerable<PlayerCharacter> GetFriendCharacters(this IObjectTable objectTable) => objectTable
            .Where(x => x is PlayerCharacter).Cast<PlayerCharacter>()
            .Where(x => x.ObjectId > 240)
            .Where(x => x.IsFriend());
    }
}
