using System;
using System.Threading.Tasks;
using Dalamud.Interface.Internal;
using Microsoft.Extensions.Caching.Memory;
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

        private static readonly TimeSpan SlidingExpiryTime = TimeSpan.FromMinutes(5);

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
                    var tex = SharedServices.TextureProvider.GetIcon(iconId);

                    if (tex is not null && tex.ImGuiHandle != nint.Zero)
                    {
                        var options = new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = SlidingExpiryTime
                        };

                        options.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
                        {
                            EvictionCallback = (key, value, reason, state) =>
                            {
                                if (value is not null and IDisposable disposable)
                                {
                                    disposable.Dispose();
                                }
                            },
                        });

                        this.iconTexCache.Set(iconId, tex, options);
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
    }
}
