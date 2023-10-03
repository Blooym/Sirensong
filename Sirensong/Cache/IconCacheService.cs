using System;
using System.Threading.Tasks;
using Dalamud.Interface.Internal;
using Dalamud.Utility;
using Sirensong.Cache.Collections;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    /// <summary>
    ///     Provides a way to load and cache icon textures.
    /// </summary>
    [SirenServiceClass]
    public sealed class IconCacheService : IDisposable
    {
        /// <summary>
        ///     The path to the icon textures.
        /// </summary>
        private const string IconFilePath = "ui/icon/{0:D3}000/{1:D6}_hr1.tex";

        /// <summary>
        ///     The dictionary of icon textures.
        /// </summary>
        private readonly CacheCollection<uint, IDalamudTextureWrap> iconTexCache = new(new CacheOptions<uint, IDalamudTextureWrap>
        {
            SlidingExpiry = TimeSpan.FromMinutes(10),
            AbsoluteExpiry = null,
            ExpireInterval = TimeSpan.FromMinutes(5),
            OnExpiry = (key, value) => value.Dispose(),
        });

        private bool disposedValue;

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
                        SirenLog.Verbose($"Loaded texture for icon {iconId}");
                        this.iconTexCache.AddOrUpdate(iconId, value => tex);
                    }
                    else
                    {
                        tex?.Dispose();
                        SirenLog.Verbose($"Texture for icon {iconId} does not exist, using placeholder.");
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
        ///     Disposes of the icon at the given path.
        /// </summary>
        /// <param name="iconId">The icon ID to dispose of.</param>
        /// <param name="remove">If true, the image will be removed from the cache.</param>
        private void DisposeIcon(uint iconId, bool remove = false)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            this.iconTexCache[iconId]?.Dispose();

            if (remove)
            {
                this.iconTexCache.Remove(iconId);
            }

            SirenLog.Verbose($"Disposed of texture for icon {iconId}");
        }

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

            return this.iconTexCache.GetOrAdd(iconId, key =>
            {
                this.LoadIconTexture(key);
                return null!;
            });
        }

        /// <summary>
        ///     Clears the icon cache.
        /// </summary>
        public void ClearCache()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            foreach (var texture in this.iconTexCache.Keys)
            {
                this.DisposeIcon(texture);
            }
        }

        /// <summary>
        ///     Clears the given iconId from the cache.
        /// </summary>
        /// <param name="iconId">The icon ID to clear from the cache.</param>
        public void ClearFromCache(int iconId)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            if (this.iconTexCache[(uint)iconId] is not null)
            {
                this.DisposeIcon((uint)iconId, true);
            }
        }
    }
}
