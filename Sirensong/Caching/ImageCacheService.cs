using System;
using System.Net.Http;
using System.Threading.Tasks;
using ImGuiScene;
using Sirensong.Caching.Collections;
using Sirensong.Caching.Internal.Interfaces;
using Sirensong.IoC.Internal;

namespace Sirensong.Caching
{
    /// <summary>
    /// Provides a way to load and cache images.
    /// </summary>
    [SirenServiceClass]
    public sealed class ImageCacheService : IDisposable, ICache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCacheService" /> class.
        /// </summary>
        internal ImageCacheService()
        {

        }

        /// <summary>
        /// The dictionary of cached images and their last access time.
        /// </summary>
        private readonly CacheCollection<string, TextureWrap> imageTexCache = new(new CacheOptions<string, TextureWrap>()
        {
            SlidingExpiry = TimeSpan.FromMinutes(5),
            AbsoluteExpiry = TimeSpan.FromMinutes(30),
            ExpireInterval = TimeSpan.FromMinutes(1),
            UseBuiltInExpire = true,
            OnExpiry = (key, value) => value.Dispose(),
        });

        /// <summary>
        /// HTTP Client instance.
        /// </summary>
        private readonly HttpClient httpClient = new();

        /// <summary>
        /// Disposes of the image provider.
        /// </summary>
        public void Dispose()
        {
            this.imageTexCache.Dispose();
            this.httpClient.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Clear() => this.imageTexCache.Clear();

        /// <inheritdoc />
        public void HandleExpired() => this.imageTexCache.HandleExpired();

        /// <summary>
        /// Loads the image at the given path or URL in a Task.
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
                        if (tex != null && tex.ImGuiHandle != IntPtr.Zero)
                        {
                            this.imageTexCache[path] = tex;
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
        /// Loads an image from a HTTP or HTTPS URL.
        /// </summary>
        /// <param name="url">The URL to load the image from.</param>
        /// <returns>The image texture.</returns>
        private async Task<byte[]> GetBytesFromUrl(string url)
        {
            using var response = await this.httpClient.GetAsync(url);
            return await response.Content.ReadAsByteArrayAsync();
        }

        /// <summary>
        /// Disposes of the image at the given path.
        /// </summary>
        /// <param name="path">The path or URL to the image.</param>
        /// <param name="remove">If true, the image will be removed from the cache.</param>
        private void DisposeImage(string path, bool remove = false)
        {
            this.imageTexCache[path]?.Dispose();
            this.imageTexCache[path] = null!;

            if (remove)
            {
                this.imageTexCache.TryRemove(path, out _);
            }

            SirenLog.Verbose($"Disposed of image at {path}");
        }

        /// <summary>
        /// Gets the image at the given path.
        /// </summary>
        /// <param name="path">The path or URL to the image.</param>
        /// <returns></returns>
        public TextureWrap? GetImage(string path)
        {

            if (this.imageTexCache.TryGetValue(path, out var tex))
            {
                return tex;
            }

            this.imageTexCache[path] = null!;
            this.LoadImage(path);
            return this.imageTexCache[path];
        }

        /// <summary>
        /// Clears the image cache.
        /// </summary>
        /// <remarks>
        /// Should not be called when any images are in use within ImGui or when any images are being loaded.
        /// </remarks>
        public void ClearCache()
        {
            foreach (var texture in this.imageTexCache.Keys)
            {
                this.DisposeImage(texture, true);
            }
        }

        /// <summary>
        /// Clears the image at the given path from the cache.
        /// </summary>
        /// <remarks>
        /// Should not be called when the image is in use within ImGui or when the image is being loaded.
        /// </remarks>
        /// <param name="path"></param>
        public void ClearFromCache(string path)
        {
            if (this.imageTexCache.ContainsKey(path))
            {
                this.DisposeImage(path, true);
            }
        }
    }
}
