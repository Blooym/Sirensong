using System;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using Sirensong.IoC.Internal;

namespace Sirensong.Game.State
{
    /// <summary>
    ///     A service for tracking information about the current fate state.
    /// </summary>
    [SirenServiceClass]
    public sealed class FateStateService : IDisposable
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
        ///     Creates a new <see cref="FateStateService" />.
        /// </summary>
        private FateStateService() => SharedServices.Framework.Update += this.HandleFateEvents;

        /// <summary>
        ///     The current fate the player is in.
        /// </summary>
        public unsafe FateContext* CurrentFate { get; private set; }

        /// <summary>
        ///     Disposes of the <see cref="FateStateService" />.
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
        private unsafe void HandleFateEvents(IFramework framework)
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
                    SirenLog.Debug($"Player left fate: {this.CurrentFate->FateId}");
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
                    SirenLog.Debug($"Player joined fate: {currentFate->FateId}");
                }
            }
        }
    }
}
