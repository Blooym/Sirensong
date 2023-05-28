using System;
using System.Net.Http;
using System.Threading.Tasks;
using ImGuiScene;
using Sirensong.Caching.Collections;
using Sirensong.IoC.Internal;

namespace Sirensong.Caching
{
    /// <summary>
    ///     Provides a way to load and cache images.
    /// </summary>
    [SirenServiceClass]
    public sealed class ImageCacheService : IDisposable
    {
        /// <summary>
        ///     HTTP Client instance.
        /// </summary>
        private readonly HttpClient httpClient = new();

        /// <summary>
        ///     The dictionary of cached images and their last access time.
        /// </summary>
        private readonly CacheCollection<string, TextureWrap> imageTexCache = new(new CacheOptions<string, TextureWrap>
        {
            SlidingExpiry = TimeSpan.FromMinutes(10),
            AbsoluteExpiry = TimeSpan.FromMinutes(60),
            ExpireInterval = TimeSpan.FromMinutes(5),
            OnExpiry = (key, value) => value.Dispose(),
        });

        private bool disposedValue;

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
                this.imageTexCache.Dispose();
                this.httpClient.Dispose();

                this.disposedValue = true;
            }
        }

        /// <summary>
        ///     Loads the image at the given path or URL in a Task.
        /// </summary>
        /// <param name="path"></param>
        private void LoadImage(string path) =>
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

                    TextureWrap? tex = null;

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
                            this.imageTexCache.AddOrUpdate(path, value => tex);
                            SirenLog.Verbose($"Loaded image at {path}");
                        }
                        else
                        {
                            this.DisposeImage(path);
                            SirenLog.Warning($"Image at {path} is not a valid image.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.DisposeImage(path);
                    SirenLog.Error($"Something went wrong while loading image at {path}: {ex.Message}");
                }
            });

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
        ///     Disposes of the image at the given path.
        /// </summary>
        /// <param name="path">The path or URL to the image.</param>
        private void DisposeImage(string path)
        {
            this.imageTexCache[path]?.Dispose();
            this.imageTexCache.Remove(path);
        }

        /// <summary>
        ///     Gets the image at the given path.
        /// </summary>
        /// <param name="path">The path or URL to the image.</param>
        /// <returns></returns>
        public TextureWrap? GetImage(string path)
            => this.imageTexCache.GetOrAdd(path, value =>
            {
                this.LoadImage(path);
                return null!;
            });

        /// <summary>
        ///     Clears the image at the given path from the cache.
        /// </summary>
        /// <remarks>
        ///     Should not be called when the image is in use within ImGui or when the image is being loaded.
        /// </remarks>
        /// <param name="path"></param>
        public void ClearFromCache(string path)
        {
            if (this.imageTexCache[path] != null)
            {
                this.DisposeImage(path);
            }
        }
    }
}
