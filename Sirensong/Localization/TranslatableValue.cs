using Dalamud;
using Newtonsoft.Json;

namespace Sirensong.Localization
{
    /// <summary>
    ///     Represents a translatable string.
    /// </summary>
    public readonly record struct TranslatableValue<T> where T : notnull
    {
        public required T Fallback { get; init; }
        public T EN { get; init; }
        public T DE { get; init; }
        public T FR { get; init; }
        public T JA { get; init; }

        /// <summary>
        ///     Gets the value for given ClientLanguage.
        /// </summary>
        /// <param name="language">The ClientLanguage to get the value for.</param>
        /// <returns>
        ///     The value for the specified ClientLanguage, or the fallback value if the ClientLanguage is not supported or
        ///     missing in the data.
        /// </returns>
        public T this[ClientLanguage language] => language switch
        {
            ClientLanguage.English => this.EN == null ? this.Fallback : this.EN,
            ClientLanguage.German => this.DE == null ? this.Fallback : this.DE,
            ClientLanguage.French => this.FR == null ? this.Fallback : this.FR,
            ClientLanguage.Japanese => this.JA == null ? this.Fallback : this.JA,
            _ => this.Fallback,
        };

        /// <summary>
        ///     Gets the value for given ISO code.
        /// </summary>
        /// <param name="isoCode">The ISO code to get the value for.</param>
        /// <returns>
        ///     The value for the specified ISO code, or the fallback value if the ISO code is not supported or missing in
        ///     the data.
        /// </returns>
        public T this[string isoCode] => isoCode switch
        {
            "en" => this.EN,
            "de" => this.DE == null ? this.Fallback : this.DE,
            "fr" => this.FR == null ? this.Fallback : this.FR,
            "ja" => this.JA == null ? this.Fallback : this.JA,
            _ => this.Fallback,
        };

        /// <summary>
        ///     Gets the string for the current game language or English if not found.
        /// </summary>
        [JsonIgnore]
        public T GameCurrent => this[SharedServices.ClientState.ClientLanguage];

        /// <summary>
        ///     Returns the string for the current Dalamud UI language or English if not found.
        /// </summary>
        [JsonIgnore]
        public T DalamudCurrent => this[SharedServices.PluginInterface.UiLanguage];
    }
}
