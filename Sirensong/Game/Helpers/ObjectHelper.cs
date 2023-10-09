using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.Interop;

namespace Sirensong.Game.Helpers
{
    public static class ObjectHelper
    {
        private const uint ObjectListSize = 599;
        private const uint HighestValidObjectId = 240;

        /// <summary>
        ///     Gets all nearby <see cref="Character" />s from the <see cref="GameObjectManager" />.
        /// </summary>
        /// <param name="includeSelf">Whether or not to include the local player.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="Pointer{T}" />s to <see cref="Character" />s.</returns>
        public static unsafe IEnumerable<Pointer<Character>> GetCharacters(bool includeSelf = true)
        {
            var table = GameObjectManager.Instance()->ObjectList;
            if (table is null)
            {
                return Enumerable.Empty<Pointer<Character>>();
            }
            var list = new List<Pointer<Character>>();
            for (var i = 0; i < ObjectListSize; i++)
            {
                var obj = (GameObject*)table[i];
                if (obj is null)
                {
                    continue;
                }

                if (obj->ObjectKind != (byte)ObjectKind.Pc && obj->ObjectID > HighestValidObjectId && (includeSelf || obj->ObjectID != SharedServices.ClientState.LocalPlayer?.ObjectId))
                {
                    continue;
                }

                list.Add((Character*)obj);
            }
            return list;
        }
    }
}