using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Plugin;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Caching;
using Sirensong.IoC;
using Sirensong.Resources.Localization;
using Condition = Dalamud.Game.ClientState.Conditions.Condition;

namespace Sirensong
{
    /// <summary>
    ///     A class containing shared services.
    /// </summary>
    internal sealed class SharedServices
    {
        // Dalamud services
        [PluginService] internal static ClientState ClientState { get; } = null!;
        [PluginService] internal static DataManager DataManager { get; } = null!;
        [PluginService] internal static ToastGui ToastGui { get; } = null!;
        [PluginService] internal static ChatGui ChatGui { get; } = null!;
        [PluginService] internal static TargetManager TargetManager { get; } = null!;
        [PluginService] internal static ObjectTable ObjectTable { get; } = null!;
        [PluginService] internal static Condition Condition { get; } = null!;
        [PluginService] internal static Framework Framework { get; } = null!;
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; } = null!;

        // Dalamud service shortcuts
        internal static UiBuilder UiBuilder => PluginInterface.UiBuilder;

        // Siren services
        [SirenService] internal static ImageCacheService ImageCache { get; } = null!;
        [SirenService] internal static IconCacheService IconCache { get; } = null!;
        [SirenService] internal static LuminaCacheService<TerritoryType> TerritoryTypeCache { get; } = null!;
        [SirenService] private static LocalizationManager LocalizationManager { get; } = null!;

        internal static void Initialize(DalamudPluginInterface pi)
        {
            pi.Create<SharedServices>();
            SirenCore.InjectServices<SharedServices>();
        }
    }
}
