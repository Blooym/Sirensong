using Dalamud;
using Dalamud.Game.Text.SeStringHandling;
using Newtonsoft.Json;

namespace Sirensong.DataStructures
{
    /// <summary>
    ///     Represents a translatable <see cref="SeString" />
    /// </summary>
    public readonly record struct TranslatableSeString
    {
        public required SeString EN { get; init; } // Default language
        public SeString DE { get; init; }
        public SeString FR { get; init; }
        public SeString JA { get; init; }

        /// <summary>
        ///     Gets the <see cref="SeString" /> for given ClientLanguage.
        /// </summary>
        /// <param name="language">The ClientLanguage to get the <see cref="SeString" /> for.</param>
        /// <returns>
        ///     The <see cref="SeString" /> for the specified ClientLanguage, or the English <see cref="SeString" /> if the
        ///     ClientLanguage is not supported or missing in the data.
        /// </returns>
        public SeString this[ClientLanguage language] => language switch
        {
            ClientLanguage.English => this.EN,
            ClientLanguage.German => string.IsNullOrEmpty(this.DE.ToString()) ? this.EN : this.DE,
            ClientLanguage.French => string.IsNullOrEmpty(this.FR.ToString()) ? this.EN : this.FR,
            ClientLanguage.Japanese => string.IsNullOrEmpty(this.JA.ToString()) ? this.EN : this.JA,
            _ => this.EN,
        };

        /// <summary>
        ///     Gets the <see cref="SeString" /> for given ISO code.
        /// </summary>
        /// <param name="isoCode">The ISO code to get the <see cref="SeString" /> for.</param>
        /// <returns>
        ///     The <see cref="SeString" /> for the specified ISO code, or the English <see cref="SeString" /> if the ISO code
        ///     is not supported or missing in the data.
        /// </returns>
        public SeString this[string isoCode] => isoCode switch
        {
            "en" => this.EN,
            "de" => string.IsNullOrEmpty(this.DE.ToString()) ? this.EN : this.DE,
            "fr" => string.IsNullOrEmpty(this.FR.ToString()) ? this.EN : this.FR,
            "ja" => string.IsNullOrEmpty(this.JA.ToString()) ? this.EN : this.JA,
            _ => this.EN,
        };

        /// <summary>
        ///     Gets the <see cref="SeString" /> for the current game language or English if not found.
        /// </summary>
        [JsonIgnore] public SeString GameCurrent => this[SharedServices.ClientState.ClientLanguage];

        /// <summary>
        ///     Returns the <see cref="SeString" /> for the current Dalamud UI language or English if not found.
        /// </summary>
        [JsonIgnore] public SeString UICurrent => this[SharedServices.PluginInterface.UiLanguage];
    }
}
