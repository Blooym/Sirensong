using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     Draw an image with the given texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="scalingMode"></param>
        /// <param name="size"></param>
        public static void Image(IDalamudTextureWrap texture, ScalingMode scalingMode = ScalingMode.None, Vector2? size = null) => ImGui.Image(texture.ImGuiHandle, scalingMode.ApplyTo(texture, size));

        /// <summary>
        ///     Draw an image with the texture from the given uri.
        /// </summary>
        /// <remarks>
        ///     Implements caching with an internal <see cref="ImageCache" /> service.
        /// </remarks>
        /// <param name="uri"></param>
        /// <param name="scalingMode"></param>
        /// <param name="size"></param>
        public static void Image(string uri, ScalingMode scalingMode = ScalingMode.None, Vector2? size = null)
        {
            var texture = SharedServices.ImageCache.GetImage(uri);
            if (texture == null)
            {
                ImGui.Dummy(size ?? Vector2.Zero);
                return;
            }
            Image(texture, scalingMode, size);
        }

        /// <summary>
        ///     Draw an icon with the texture from the given icon id.
        /// </summary>
        /// <remarks>
        ///     Implements caching with an internal <see cref="IconCache" /> service.
        /// </remarks>
        /// <param name="iconId"></param>
        /// <param name="scalingMode"></param>
        /// <param name="size"></param>
        public static void Icon(uint iconId, ScalingMode scalingMode = ScalingMode.None, Vector2? size = null)
        {
            var texture = SharedServices.IconCache.GetIcon(iconId);
            if (texture == null)
            {
                ImGui.Dummy(size ?? Vector2.Zero);
                return;
            }
            Image(texture, scalingMode, size);
        }

        /// <summary>
        ///     Draw an image button with the given texture and scaling mode.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="scalingMode"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool ImageButton(IDalamudTextureWrap texture, ScalingMode scalingMode = ScalingMode.None, Vector2? size = null) => ImGui.ImageButton(texture.ImGuiHandle, scalingMode.ApplyTo(texture, size));

        /// <summary>
        ///     Draw an image button with the texture from the given uri.
        /// </summary>
        /// <remarks>
        ///     Implements caching with an internal <see cref="ImageCache" /> service.
        /// </remarks>
        /// <param name="uri"></param>
        /// <param name="scalingMode"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool ImageButton(string uri, ScalingMode scalingMode = ScalingMode.None, Vector2? size = null)
        {
            var texture = SharedServices.ImageCache.GetImage(uri);
            if (texture == null)
            {
                ImGui.Dummy(size ?? Vector2.Zero);
                return false;
            }
            return ImageButton(texture, scalingMode, size);
        }

        /// <summary>
        ///     Draw a button with the texture from the given icon id.
        /// </summary>
        /// <remarks>
        ///     Implements caching with an internal <see cref="IconCache" /> service.
        /// </remarks>
        /// <param name="iconId"></param>
        /// <param name="scalingMode"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool IconButton(uint iconId, ScalingMode scalingMode = ScalingMode.None, Vector2? size = null)
        {
            var texture = SharedServices.IconCache.GetIcon(iconId);
            if (texture == null)
            {
                ImGui.Dummy(size ?? Vector2.Zero);
                return false;
            }
            return ImageButton(texture, scalingMode, size);
        }
    }

    /// <summary>
    ///     Scaling modes for images.
    /// </summary>
    public enum ScalingMode
    {
        /// <summary>
        ///     Apply no scaling to the image, may result in image being bigger than the window.
        /// </summary>
        None,

        /// <summary>
        ///     Scale the image to fit the window while maintaining aspect ratio and image size.
        /// </summary>
        Contain,
    }

    /// <summary>
    ///     Extension methods for <see cref="ScalingMode" />.
    /// </summary>
    public static class ScalingModeExtensions
    {
        /// <summary>
        ///     Converts a <see cref="ScalingMode" /> to a <see cref="Vector2" /> scale.
        /// </summary>
        /// <param name="scalingMode"></param>
        /// <param name="texture"></param>
        /// <param name="appliedSizing"></param>
        /// <returns></returns>
        public static Vector2 ApplyTo(this ScalingMode scalingMode, IDalamudTextureWrap texture, Vector2? appliedSizing = null)
        {
            var windowSize = ImGui.GetContentRegionAvail();

            switch (scalingMode)
            {
                case ScalingMode.None:
                    return appliedSizing ?? new Vector2(texture.Width, texture.Height);
                case ScalingMode.Contain:
                    var size = new Vector2(Math.Min(windowSize.X, appliedSizing?.X ?? windowSize.X), Math.Min(windowSize.Y, appliedSizing?.Y ?? windowSize.Y));
                    var aspectRatio = texture.Width / texture.Height;
                    return new Vector2(size.X, size.X / aspectRatio);
                default:
                    throw new ArgumentOutOfRangeException(nameof(scalingMode), scalingMode, null);
            }
        }
    }
}
