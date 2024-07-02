using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BitFaster.Caching;
using BitFaster.Caching.Lru;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    /// <summary>
    ///     Provides a way to load and cache images.
    /// </summary>
    [SirenServiceClass]
    public sealed class RemoteBytesCacheService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        ///     HTTP Client instance.
        /// </summary>
        private readonly HttpClient httpClient = new()
        {
            DefaultRequestHeaders =
                {
                    { "User-Agent", $"Sirensong/{Assembly.GetExecutingAssembly().GetName().Version}"},
                },
        };

        /// <summary>
        ///     The cache of loaded images.
        /// </summary>
        private readonly ICache<string, byte[]> remoteBytesCache =
            new ConcurrentLruBuilder<string, byte[]>()
                .WithAtomicGetOrAdd()
                .WithCapacity(200)
                .WithExpireAfterWrite(TimeSpan.FromMinutes(5))
                .Build();


        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteBytesCacheService" /> class.
        /// </summary>
        private RemoteBytesCacheService()
        {

        }

        /// <summary>
        ///     Disposes of the RemoteBytesCacheService provider.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.httpClient.Dispose();
                this.disposedValue = true;
            }
        }

        /// <summary>
        ///     Fetches the bytes for the given URL in a Task.
        /// </summary>
        /// <param name="path"></param>
        private void GetRemoteBytes(string path)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(RemoteBytesCacheService));

            Task.Run(async () =>
            {
                try
                {
                    if (!path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                        path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) &&
                        path.EndsWith(".webp", StringComparison.OrdinalIgnoreCase) &&
                        path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        SirenLog.Warning($"Refusing to load image with invalid extension: {path}");
                        return;
                    }

                    if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
                    {
                        switch (uri.Scheme)
                        {
                            case "http":
                                SirenLog.Warning($"Refusing to load image over HTTP: {path}");
                                return;
                            case "https":
                                var bytes = await this.GetBytesFromUrl(path);
                                this.remoteBytesCache.AddOrUpdate(path, bytes);
                                break;
                            default:
                                SirenLog.Warning($"Invalid URI: {path}");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.remoteBytesCache.TryRemove(path);
                    SirenLog.Error($"Something went wrong while loading image at {path}: {ex.Message}");
                }
            });
        }

        /// <summary>
        ///     Loads bytes from a HTTP or HTTPS URL.
        /// </summary>
        /// <param name="url">The URL to load the image from.</param>
        /// <returns>The image texture.</returns>
        private async Task<byte[]> GetBytesFromUrl(string url)
        {
            using var response = await this.httpClient.GetAsync(url);
            return await response.Content.ReadAsByteArrayAsync();
        }

        /// <summary>
        ///     Gets the bytes for the given URL.
        /// </summary>
        /// <param name="url">The url to load bytes for.</param>
        /// <returns></returns>
        public byte[]? Get(string url)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(RemoteBytesCacheService));

            var exists = this.remoteBytesCache.TryGet(url, out var value);
            if (exists)
            {
                return value;
            }

            this.GetRemoteBytes(url);
            return null;
        }

        /// <summary>
        ///     Removes the bytes for the given URL from the cache.
        /// </summary>
        /// <param name="url">The URL to remove from the cache</param>
        /// <returns>Whether or not the value was removed</remarks>
        public bool Remove(string url)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(RemoteBytesCacheService));
            return this.remoteBytesCache.TryRemove(url);
        }
    }
}
