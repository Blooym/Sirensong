using System.IO;
using System.Runtime.CompilerServices;
using Dalamud.Logging;
using Sirensong.IoC.Internal;

namespace Sirensong.Logging
{
    /// <summary>
    ///     A wrapper around <see cref="PluginLog"/> that provides a nicer format, for use by plugins.
    /// </summary>
    // Notes: The reason this has to be a service is because all methods must be called directly by the plugin
    //        in order for it to show up as the right assembly in the log.
    [SirenServiceClass]
    public class BetterLog
    {
#pragma warning disable CA1822 // Mark members as static
        /// <summary>
        ///     Creates a new <see cref="BetterLog"/>.
        /// </summary>
        internal BetterLog() { }

        /// <summary>
        ///     Formats a log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caller"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static string Format(string message, string? caller, string? file) => $"<{Path.GetFileName(file)}::{caller}> via {SirenCore.InitializerName}: {message}";

        /// <inheritdoc cref="PluginLog.Verbose(string, object[])"/>
        internal void Verbose(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Verbose(Format(message, caller, file));

        /// <inheritdoc cref="PluginLog.Debug(string, object[])"/>
        internal void Debug(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Debug(Format(message, caller, file));

        /// <inheritdoc cref="PluginLog.Information(string, object[])"/>
        internal void Information(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Information(Format(message, caller, file));

        /// <inheritdoc cref="PluginLog.Warning(string, object[])"/>
        internal void Warning(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Warning(Format(message, caller, file));

        /// <inheritdoc cref="PluginLog.Error(string, object[])"/>
        internal void Error(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Error(Format(message, caller, file));
#pragma warning restore CA1822 // Mark members as static
    }
}