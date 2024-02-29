using System;
using System.Threading.Tasks;
using Dalamud.Interface.Internal;
using Dalamud.Utility;
using Microsoft.Extensions.Caching.Memory;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    /// <summary>
    ///     Provides a way to load and cache icon textures.
    /// </summary>
    [SirenServiceClass]
    public sealed class IconCacheService : IDisposable
    {
        private bool disposedValue;

        private static readonly TimeSpan SlidingExpiryTime = TimeSpan.FromMinutes(5);

        /// <summary>
        ///     The path to the icon textures.
        /// </summary>
        private const string IconFilePath = "ui/icon/{0:D3}000/{1:D6}_hr1.tex";

        /// <summary>
        ///     The dictionary of icon textures.
        /// </summary>
        private readonly MemoryCache iconTexCache = new(new MemoryCacheOptions());

        /// <summary>
        ///     Initializes a new instance of the <see cref="IconCacheService" /> class.
        /// </summary>
        private IconCacheService()
        {

        }

        /// <summary>
        ///     Disposes of the icon provider.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.iconTexCache.Dispose();
                this.disposedValue = true;
            }
        }

        /// <summary>
        ///     Uses a task to load the icon texture for the given icon ID.
        /// </summary>
        /// <param name="iconId">The icon ID to load the texture for.</param>
        private void LoadIconTexture(uint iconId)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            Task.Run(() =>
            {
                try
                {
                    var path = GetIconPath(iconId);
                    var tex = SharedServices.TextureProvider.GetTextureFromGame(path);

                    if (tex is not null && tex.ImGuiHandle != nint.Zero)
                    {
                        var entry = this.iconTexCache.CreateEntry(iconId);
                        entry.SlidingExpiration = SlidingExpiryTime;
                        entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
                        {
                            EvictionCallback = (key, value, reason, state) =>
                            {
                                if (value is not null and IDisposable disposable)
                                {
                                    disposable.Dispose();
                                }
                            },
                        });
                        this.iconTexCache.Set(iconId, entry);
                        SirenLog.Verbose($"Loaded and cached texture for icon {iconId}");
                    }
                    else
                    {
                        tex?.Dispose();
                        SirenLog.Verbose($"Texture for icon {iconId} does not exist.");
                    }
                }
                catch (Exception ex)
                {
                    SirenLog.Error($"Something went wrong while loading icon {iconId}: {ex.Message}");
                }
            });
        }

        /// <summary>
        ///     Gets the path to the icon texture for the given icon ID.
        /// </summary>
        /// <param name="iconId"></param>
        /// <returns></returns>
        private static string GetIconPath(uint iconId) => IconFilePath.Format(iconId / 1000, iconId.ToString());

        /// <summary>
        ///     Gets the icon texture for the given icon ID.
        /// </summary>
        /// <param name="iconId">The icon ID to get the texture for.</param>
        /// <returns>The icon texture, or null if it could not be loaded.</returns>
        public IDalamudTextureWrap? GetIcon(uint iconId)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            var existingValue = this.iconTexCache.Get<IDalamudTextureWrap>(iconId);
            if (existingValue is not null)
            {
                return existingValue;
            }

            this.LoadIconTexture(iconId);
            return null;
        }

        /// <summary>
        ///     Clears the given iconId from the cache. Will also dispose of the cached icon texture.
        /// </summary>
        /// <param name="iconId">The icon ID to clear from the cache.</param>
        public void ClearFromCache(int iconId)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            this.iconTexCache.Remove(iconId);
        }
    }
}
