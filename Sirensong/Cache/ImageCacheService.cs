using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BitFaster.Caching;
using BitFaster.Caching.Lru;
using Dalamud.Interface.Internal;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    /// <summary>
    ///     Provides a way to load and cache images.
    /// </summary>
    [SirenServiceClass]
    public sealed class ImageCacheService : IDisposable
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
        private readonly ICache<string, IDalamudTextureWrap> imageTexCache =
            new ConcurrentLruBuilder<string, IDalamudTextureWrap>()
                .WithAtomicGetOrAdd()
                .WithCapacity(200)
                .WithExpireAfterWrite(TimeSpan.FromMinutes(5))
                .Build();


        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageCacheService" /> class.
        /// </summary>
        private ImageCacheService()
        {

        }

        /// <summary>
        ///     Disposes of the image provider.
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
        ///     Loads the image at the given path or URL in a Task.
        /// </summary>
        /// <param name="path"></param>
        private void LoadImage(string path)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(ImageCacheService));

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

                    IDalamudTextureWrap? tex = null;

                    if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
                    {
                        switch (uri.Scheme)
                        {
                            case "http":
                                SirenLog.Warning($"Refusing to load image over HTTP: {path}");
                                return;
                            case "https":
                                var bytes = await this.GetBytesFromUrl(path);
                                tex = await SharedServices.UiBuilder.LoadImageAsync(bytes);
                                break;
                            case "file":
                                tex = await SharedServices.UiBuilder.LoadImageAsync(uri.LocalPath);
                                break;
                            default:
                                SirenLog.Warning($"Invalid URI: {path}");
                                break;
                        }

                        // If the texture is valid, add it to the cache
                        if (tex != null && tex.ImGuiHandle != nint.Zero)
                        {
                            this.imageTexCache.AddOrUpdate(path, tex);
                            SirenLog.Verbose($"Loaded image at {path}");
                        }
                        else
                        {
                            this.imageTexCache.TryRemove(path);
                            SirenLog.Warning($"Image at {path} is not a valid image.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.imageTexCache.TryRemove(path);
                    SirenLog.Error($"Something went wrong while loading image at {path}: {ex.Message}");
                }
            });
        }

        /// <summary>
        ///     Loads an image from a HTTP or HTTPS URL.
        /// </summary>
        /// <param name="url">The URL to load the image from.</param>
        /// <returns>The image texture.</returns>
        private async Task<byte[]> GetBytesFromUrl(string url)
        {
            using var response = await this.httpClient.GetAsync(url);
            return await response.Content.ReadAsByteArrayAsync();
        }

        /// <summary>
        ///     Gets the image at the given path.
        /// </summary>
        /// <param name="path">The path or URL to the image.</param>
        /// <returns></returns>
        public IDalamudTextureWrap? Get(string path)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(ImageCacheService));

            var exists = this.imageTexCache.TryGet(path, out var value);
            if (exists)
            {
                return value;
            }

            this.LoadImage(path);
            return null;
        }

        /// <summary>
        ///     Removes the image at the given path from the cache.
        /// </summary>
        /// <remarks>
        ///     Should not be called when the image is in use within ImGui or when the image is being loaded.
        /// </remarks>
        /// <param name="path">The image path to remove from the cache</param>
        /// <returns>Whether or not the vlaue was removed</remarks>
        public bool Remove(string path)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(ImageCacheService));


            return this.imageTexCache.TryRemove(path);
        }
    }
}
