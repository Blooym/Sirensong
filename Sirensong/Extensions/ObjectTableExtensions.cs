using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Sirensong.Game.Helpers;

namespace Sirensong.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="ObjectTable" />.
    /// </summary>
    public static class ObjectTableExtensions
    {
        /// <summary>
        ///     Gets all <see cref="PlayerCharacter" />s in the <see cref="ObjectTable" />.
        /// </summary>
        /// <param name="objectTable"></param>
        /// <param name="includeSelf">Whether or not to include the local player.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="PlayerCharacter" />s.</returns>
        public static IEnumerable<PlayerCharacter> GetPlayerCharacters(this ObjectTable objectTable, bool includeSelf = true) => objectTable
            .Where(x => x is PlayerCharacter).Cast<PlayerCharacter>()
            .Where(x => includeSelf || x.ObjectId != SharedServices.ClientState.LocalPlayer?.ObjectId)
            .Where(x => x.ObjectId > 240);

        /// <summary>
        ///     Gets all nearby friend <see cref="PlayerCharacter" />s in the <see cref="ObjectTable" />.
        /// </summary>
        /// <param name="objectTable"></param>
        /// <returns></returns>
        public static IEnumerable<PlayerCharacter> GetFriendCharacters(this ObjectTable objectTable) => objectTable
            .Where(x => x is PlayerCharacter).Cast<PlayerCharacter>()
            .Where(x => x.ObjectId > 240)
            .Where(x => x.IsFriend());
    }
}
