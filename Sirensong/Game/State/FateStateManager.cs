using System;
using Dalamud.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using Sirensong.IoC.Internal;

namespace Sirensong.Game.State
{
    [SirenServiceClass]
    public sealed class FateStateManager : IDisposable
    {
        /// <summary>
        ///     The delegate for when a fate event occurs with a context.
        /// </summary>
        /// <param name="fate"></param>
        public unsafe delegate void FateContextHandler(FateContext* fate);

        /// <summary>
        ///     The delegate for when a fate event occurs with no context.
        /// </summary>
        public delegate void FateNoContext();

        /// <summary>
        ///     Creates a new <see cref="FateStateManager" />.
        /// </summary>
        private FateStateManager() => SharedServices.Framework.Update += this.HandleFateEvents;

        /// <summary>
        ///     The current fate the player is in.
        /// </summary>
        public unsafe FateContext* CurrentFate { get; private set; }

        /// <summary>
        ///     Disposes of the <see cref="FateStateManager" />.
        /// </summary>
        public void Dispose() => SharedServices.Framework.Update -= this.HandleFateEvents;

        /// <summary>
        ///     Called when the player first joins a fate.
        /// </summary>
        public event FateContextHandler? FateJoined;

        /// <summary>
        ///     Called when the player leaves a fate.
        /// </summary>
        public event FateNoContext? FateLeft;

        /// <summary>
        ///     Handles detecting fate events and invoking the appropriate event.
        /// </summary>
        /// <param name="framework"></param>
        private unsafe void HandleFateEvents(Framework framework)
        {
            var fateManager = FateManager.Instance();
            if (fateManager == null)
            {
                return;
            }

            var currentFate = fateManager->CurrentFate;
            if (currentFate == null)
            {
                if (this.CurrentFate != null)
                {
                    this.CurrentFate = null;
                    this.FateLeft?.Invoke();
                }
            }
            else
            {
                if (this.CurrentFate != currentFate)
                {
                    this.CurrentFate = currentFate;
                    this.FateJoined?.Invoke(currentFate);
                }
            }
        }
    }
}
