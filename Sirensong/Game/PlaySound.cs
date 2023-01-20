using System;
using System.Runtime.InteropServices;
using Sirensong.Game.Enums;

// From GatherBuddy, licensed under the apache 2.0 license.
// https://github.com/Ottermandias/GatherBuddy/blob/main/LICENSE
// https://github.com/Ottermandias/GatherBuddy/blob/main/GatherBuddy/SeFunctions/PlaySound.cs

namespace Sirensong.Game
{
    /// <summary>
    ///     Methods to play in-game sound effects.
    /// </summary>
    public static class PlaySound
    {
        private static class Signatures
        {
            internal const string PlaySound = "E8 ?? ?? ?? ?? 4D 39 BE";
        }

        /// <summary>
        ///     If this is true, the PlaySound functions have been disabled.
        /// </summary>
        public static bool Disabled { get; private set; }

        private delegate IntPtr PlaySoundDelegate(SoundEffect id, IntPtr a2, IntPtr a3);

        private static PlaySoundDelegate invoke = null!;

        /// <summary>
        ///     Initializes the PlaySound delegate.
        /// </summary>
        /// <returns>True if the delegate was initialized successfully, false otherwise</returns>
        private static bool Initialize()
        {
            try
            {
                if (Disabled)
                {
                    return false;
                }

                if (invoke != null)
                {
                    return true;
                }

                var examineFound = SharedServices.SigScanner.TryScanText(Signatures.PlaySound, out var soundData);
                if (!examineFound || soundData == IntPtr.Zero)
                {
                    SirenLog.IWarning("Could not find signature for PlaySound, sound functions will be disabled.");
                    Disabled = true;
                }
                invoke = Marshal.GetDelegateForFunctionPointer<PlaySoundDelegate>(soundData);
                return true;
            }
            catch (Exception e)
            {
                SirenLog.IWarning($"Could not initialize PlaySound functions, they will be disabled. {e}");
                Disabled = true;
                return false;
            }
        }

        /// <summary>
        ///     Invokes the PlaySound delegate with the given parameters to play a sound effect.
        /// </summary>
        public static void Invoke(SoundEffect id, IntPtr a2 = 0, IntPtr a3 = 0)
        {
            if (!Initialize())
            {
                return;
            }

            invoke(id, a2, a3);
        }
    }
}
