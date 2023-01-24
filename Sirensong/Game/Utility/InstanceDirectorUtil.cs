using FFXIVClientStructs.FFXIV.Client.Game.Event;
using Sirensong.Game.Enums;

namespace Sirensong.Game.Utility
{
    public static class InstanceDirectorUtil
    {
        /// <summary>
        ///     Gets the ContentFlag of the current instance.
        /// </summary>
        /// <remarks>
        ///     This is a wrapper around <see cref="EventFramework.Instance()"/> -> <see cref="InstanceContentDirector"/> -> <see cref="ContentDirector"/> -> <see cref="Director"/> -> <see cref="ContentFlags"/>.
        /// </remarks>
        /// <returns>The ContentFlag of the current instance.</returns>
        public static unsafe ContentFlag? GetInstanceContentFlag()
        {
            var instanceCD = EventFramework.Instance()->GetInstanceContentDirector();
            if (instanceCD == null)
            {
                return null;
            }

            var contentFlags = instanceCD->ContentDirector.Director.ContentFlags;
            return (ContentFlag)contentFlags;
        }
    }
}