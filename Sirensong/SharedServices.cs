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

namespace Sirensong
{
    /// <summary>
    /// A class containing shared services.
    /// </summary>
    internal sealed class SharedServices
    {
        // Dalamud services
        [PluginService] internal static ClientState ClientState { get; private set; } = null!;
        [PluginService] internal static DataManager DataManager { get; private set; } = null!;
        [PluginService] internal static SigScanner SigScanner { get; private set; } = null!;
        [PluginService] internal static ToastGui ToastGui { get; private set; } = null!;
        [PluginService] internal static ChatGui ChatGui { get; private set; } = null!;
        [PluginService] internal static TargetManager TargetManager { get; private set; } = null!;
        [PluginService] internal static ObjectTable ObjectTable { get; private set; } = null!;
        [PluginService] internal static Dalamud.Game.ClientState.Conditions.Condition Condition { get; private set; } = null!;
        [PluginService] internal static Framework Framework { get; private set; } = null!;
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;

        // Dalamud service shortcuts
        internal static UiBuilder UiBuilder => PluginInterface.UiBuilder;

        // Siren services
        [SirenService] internal static ImageCacheService ImageCache { get; private set; } = null!;
        [SirenService] internal static IconCacheService IconCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<TerritoryType> TerritoryTypeCache { get; private set; } = null!;
        [SirenService] internal static LocalizationManager LocalizationManager { get; private set; } = null!;

        internal static void Initialize(DalamudPluginInterface pi)
        {
            pi.Create<SharedServices>();
            SirenCore.InjectServices<SharedServices>();
        }
    }
}