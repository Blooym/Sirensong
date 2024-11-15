using FFXIVClientStructs.FFXIV.Client.UI;
using Sirensong.Game.Enums;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for playing sound effects.
    /// </summary>
    public static class SoundEffectHelper
    {
        private const nint PlaySoundA2Default = 0;
        private const nint PlaySoundA3Default = 0;
        private const byte PlaySoundA4Default = 0;

        /// <summary>
        ///     Plays a sound effect.
        /// </summary>
        /// <param name="soundEffect">The sound effect to play.</param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        public static void PlaySound(SoundEffect soundEffect, nint a2 = PlaySoundA2Default, nint a3 = PlaySoundA3Default, byte a4 = PlaySoundA4Default) => UIGlobals.PlaySoundEffect((uint)soundEffect, a2, a3, a4);

        /// <inheritdoc cref="PlaySound(SoundEffect,nint,nint,byte)" />
        public static void PlaySound(uint soundEffect, nint a2 = PlaySoundA2Default, nint a3 = PlaySoundA3Default, byte a4 = PlaySoundA4Default) => UIGlobals.PlaySoundEffect(soundEffect, a2, a3, a4);

        /// <summary>
        ///     Plays a chat sound effect.
        /// </summary>
        /// <param name="effectId">The sound effect to play.</param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        public static void PlayChatSoundEffect(uint effectId) => UIGlobals.PlayChatSoundEffect(effectId);
    }
}