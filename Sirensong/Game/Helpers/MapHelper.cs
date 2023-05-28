using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for interacting with the map.
    /// </summary>
    public static class MapHelper
    {
        /// <summary>
        ///     The current map ID of the local player's location.
        /// </summary>
        /// <returns>The current map ID.</returns>
        public static unsafe uint CurrentMapId => AgentMap.Instance()->CurrentMapId;

        /// <summary>
        ///     Opens the map with the given ID.
        /// </summary>
        /// <param name="mapId">The map ID to open.</param>
        public static unsafe void OpenMap(uint mapId) => AgentMap.Instance()->OpenMapByMapId(mapId);

        /// <summary>
        ///     Places a flag on the current map at the given position and then opens the map.
        /// </summary>
        /// <param name="position">The position to place the flag.</param>
        /// <param name="title">The title of the flag.</param>
        /// <param name="type">The type of flag to place.</param>
        public static unsafe void FlagAndOpenCurrentMap(Vector3 position, string? title = null, MapType type = MapType.FlagMarker)
        {
            var agent = AgentMap.Instance();
            agent->SetFlagMapMarker(agent->CurrentTerritoryId, agent->CurrentMapId, position);
            agent->OpenMap(agent->CurrentMapId, agent->CurrentTerritoryId, title, type);
        }
    }
}
