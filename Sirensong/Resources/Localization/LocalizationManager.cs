using System;
using System.Globalization;
using Sirensong.IoC.Internal;

namespace Sirensong.Resources.Localization
{
    [SirenServiceClass]
    internal sealed class LocalizationManager
    {
        private bool disposedValue;

        /// <summary>
        /// Creates a new resource manager and sets up resources.
        /// </summary>
        private LocalizationManager()
        {
            SetupLocalization(SharedServices.PluginInterface.UiLanguage);
            SharedServices.PluginInterface.LanguageChanged += SetupLocalization;
        }

        /// <summary>
        /// Disposes of the <see cref="LocalizationManager" />
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                SharedServices.PluginInterface.LanguageChanged -= SetupLocalization;
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Sets up localization for the given language, or uses fallbacks if not found.
        /// </summary>
        /// <param name="language">The language to use.</param>
        private static void SetupLocalization(string language)
        {
            try
            {
                SirenLog.Information($"Setting up localization for {language}");
                Strings.Culture = new CultureInfo(language);
            }
            catch (Exception e)
            {
                SirenLog.Error($"Failed to set language to {language}: {e.Message}");
            }
        }
    }
}