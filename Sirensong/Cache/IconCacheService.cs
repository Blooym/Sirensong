using System;
using System.Threading.Tasks;
using BitFaster.Caching;
using BitFaster.Caching.Lru;
using Dalamud.Interface.Internal;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    /// <summary>
    ///     Provides a way to load and cache icon textures.
    /// </summary>
    /// <remarks>
    ///     While the <see cref="TextureManager"/> provided by Dalamud already holds texture instances and disposes of
    ///     them when unused it is still a fair bit faster to hold each item in another cache with its own expiry anyway.
    ///     
    ///     This may change in the future and if it does the IconCache will be removed.
    /// </remarks>
    [SirenServiceClass]
    public sealed class IconCacheService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        ///     The cache of <see cref="IDalamudTextureWrap" /> obtained from lookups.
        /// </summary>
        /// <remarks>
        ///     Does not hold the texture instance itself, just the wrapper that can be used to load
        ///     the texture in on the fly.
        /// </remarks>
        private readonly ICache<uint, IDalamudTextureWrap> iconTexCache =
            new ConcurrentLruBuilder<uint, IDalamudTextureWrap>()
                .WithAtomicGetOrAdd()
                .WithCapacity(200)
                .WithExpireAfterWrite(TimeSpan.FromHours(1))
                .Build();

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
                this.iconTexCache.Clear();
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
                    var tex = SharedServices.TextureProvider.GetIcon(iconId);

                    if (tex is not null && tex.ImGuiHandle != nint.Zero)
                    {
                        this.iconTexCache.AddOrUpdate(iconId, tex);
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
        ///     Gets the icon texture for the given icon ID.
        /// </summary>
        /// <param name="iconId">The icon ID to get the texture for.</param>
        /// <returns>The icon texture, or null if it could not be loaded.</returns>
        public IDalamudTextureWrap? Get(uint iconId)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            var exists = this.iconTexCache.TryGet(iconId, out var value);
            if (exists)
            {
                return value;
            }

            this.LoadIconTexture(iconId);
            return null;
        }

        /// <summary>
        ///     Removes the icon with the giveen path from the cache.
        /// </summary>
        /// <remarks>
        ///     Should not be called when the icon is in use within ImGui or when the icon is being loaded.
        /// </remarks>
        /// <param name="iconId">The icon ID to remove from the cache</param>
        /// <returns>Whether or not the vlaue was removed</remarks>
        public bool Remove(uint iconId)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(IconCacheService));
            }

            return this.iconTexCache.TryRemove(iconId);
        }
    }
}
