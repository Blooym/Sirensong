using FFXIVClientStructs.FFXIV.Client.UI;
using Sirensong.Game.Enums;

namespace Sirensong.Game.Helpers
{
    /// <summary>
    ///     Helper methods for playing sound effects.
    /// </summary>
    public static class SoundEffectHelper
    {
        private const long PlaySoundA2Default = 0;
        private const long PlaySoundA3Default = 0;
        private const byte PlaySoundA4Default = 0;

        /// <summary>
        ///     Plays a sound effect.
        /// </summary>
        /// <param name="soundEffect">The sound effect to play.</param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        public static void PlaySound(SoundEffect soundEffect, long a2 = PlaySoundA2Default, long a3 = PlaySoundA3Default, byte a4 = PlaySoundA4Default) => UIModule.PlaySound((uint)soundEffect, a2, a3, a4);

        /// <inheritdoc cref="PlaySound(SoundEffect,long,long,byte)" />
        public static void PlaySound(uint soundEffect, long a2 = PlaySoundA2Default, long a3 = PlaySoundA3Default, byte a4 = PlaySoundA4Default) => UIModule.PlaySound(soundEffect, a2, a3, a4);

        /// <summary>
        ///     Plays a chat sound effect.
        /// </summary>
        /// <param name="effectId">The sound effect to play.</param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        public static void PlayChatSoundEffect(uint effectId) => UIModule.PlayChatSoundEffect(effectId);
    }
}