using System.Diagnostics.CodeAnalysis;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Cache;
using Sirensong.IoC;
using Sirensong.Localization;

namespace Sirensong
{
    /// <summary>
    ///     A class containing shared services.
    /// </summary>
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
#pragma warning disable IDE0051
    internal sealed class SharedServices
    {
        // Dalamud services
        [PluginService] internal static IClientState ClientState { get; private set; } = null!;
        [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
        [PluginService] internal static IToastGui ToastGui { get; private set; } = null!;
        [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;
        [PluginService] internal static ITargetManager TargetManager { get; private set; } = null!;
        [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;
        [PluginService] internal static ICondition Condition { get; private set; } = null!;
        [PluginService] internal static IFramework Framework { get; private set; } = null!;
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
        [PluginService] internal static IPluginLog PluginLog { get; private set; } = null!;

        // Dalamud service shortcuts
        internal static UiBuilder UiBuilder => PluginInterface.UiBuilder;

        // Siren services
        [SirenService] internal static ImageCacheService ImageCache { get; private set; } = null!;
        [SirenService] internal static LuminaCacheService<TerritoryType> TerritoryTypeCache { get; private set; } = null!;
        [SirenService] private static LocalizationManager LocalizationManager { get; set; } = null!;

        internal static void Initialize(DalamudPluginInterface pi)
        {
            pi.Create<SharedServices>();
            SirenCore.InjectServices<SharedServices>();
        }
    }
}
