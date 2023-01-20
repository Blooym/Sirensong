using System.IO;
using System.Runtime.CompilerServices;
using Dalamud.Logging;

namespace Sirensong
{
    /// <summary>
    ///     Logging utility wrapping <see cref="PluginLog"/> for internal Sirensong messages.
    /// </summary>
    internal static class SirenLog
    {
        /// <summary>
        ///     Formats a message for logging internal messages.
        /// </summary>
        private static string InternalFormat(string message, string? caller, string? file) => $"<{Path.GetFileName(file)}::{caller}> via {SirenCore.InitializerName}: {message}";

        /// <inheritdoc cref="PluginLog.Verbose(string, object[])"/>
        internal static void IVerbose(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Verbose(InternalFormat(message, caller, file));

        /// <inheritdoc cref="PluginLog.Debug(string, object[])"/>
        internal static void IDebug(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Debug(InternalFormat(message, caller, file));

        /// <inheritdoc cref="PluginLog.Information(string, object[])"/>
        internal static void IInformation(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Information(InternalFormat(message, caller, file));

        /// <inheritdoc cref="PluginLog.Warning(string, object[])"/>
        internal static void IWarning(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Warning(InternalFormat(message, caller, file));

        /// <inheritdoc cref="PluginLog.Error(string, object[])"/>
        internal static void IError(string message, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null) => PluginLog.Error(InternalFormat(message, caller, file));
    }
}
