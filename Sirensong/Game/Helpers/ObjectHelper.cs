using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.SubKinds;

namespace Sirensong.Game.Helpers
{
    public static class ObjectHelper
    {
        private const uint HighestValidObjectId = 240;

        /// <summary>
        ///     Gets all nearby <see cref="IPlayerCharacter" />s from the <see cref="IObjectTable" />.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IPlayerCharacter" />s.</returns>
        public static unsafe IEnumerable<IPlayerCharacter> GetCharacters() =>
            (IEnumerable<IPlayerCharacter>)SharedServices.ObjectTable.Where(
                x => x.ObjectKind == ObjectKind.Player &&
                x.GetType() == typeof(IPlayerCharacter) &&
                x.EntityId > HighestValidObjectId);
    }
}