using System.Reflection;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Sirensong
{
    /// <summary>
    ///     A class containing shared services.
    /// </summary>
    internal sealed class SharedServices
    {
        // Dalamud services
        [PluginService] internal static ClientState ClientState { get; private set; } = GetDalamudService<ClientState>();
        [PluginService] internal static DataManager DataManager { get; private set; } = GetDalamudService<DataManager>();
        [PluginService] internal static SigScanner SigScanner { get; private set; } = GetDalamudService<SigScanner>();
        [PluginService] internal static ToastGui ToastGui { get; private set; } = GetDalamudService<ToastGui>();
        [PluginService] internal static ChatGui ChatGui { get; private set; } = GetDalamudService<ChatGui>();
        [PluginService] internal static TargetManager TargetManager { get; private set; } = GetDalamudService<TargetManager>();
        [PluginService] internal static ObjectTable ObjectTable { get; private set; } = GetDalamudService<ObjectTable>();
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;

        // Dalamud service shortcuts
        internal static UiBuilder UiBuilder => PluginInterface.UiBuilder;

        /// <summary>
        ///     Gets a service from Dalamud using reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>
        ///    <para>
        ///    This method was taken from XivCommon, which is licensed under the MIT license.
        ///    See https://git.anna.lgbt/ascclemens/XivCommon/raw/branch/main/LICENCE for more information.
        ///    </para>
        /// </remarks>
        internal static T GetDalamudService<T>()
        {
            var service = typeof(IDalamudPlugin).Assembly.GetType("Dalamud.Service`1")!.MakeGenericType(typeof(T));
            var get = service.GetMethod("Get", BindingFlags.Public | BindingFlags.Static)!;
            return (T)get.Invoke(null, null)!;
        }
    }
}
