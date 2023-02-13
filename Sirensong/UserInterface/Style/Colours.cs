using System.Numerics;

namespace Sirensong.UserInterface.Style
{
    /// <summary>
    ///     Colour values for UI design.
    /// </summary>
    public static class Colours
    {
        /// <summary>
        ///     Colour for errors.
        /// </summary>
        public static readonly Vector4 Error = new(1f, 0.0f, 0.0f, 1f);

        /// <summary>
        ///     Colour for warnings.
        /// </summary>
        public static readonly Vector4 Warning = new(1f, 0.709f, 0.0f, 1f);

        /// <summary>
        ///     Colour for success.
        /// </summary>
        public static readonly Vector4 Success = new(0.117f, 1f, 0.0f, 1f);

        /// <summary>
        ///     Colour for info.
        /// </summary>
        public static readonly Vector4 Informational = new(0.0f, 0.6f, 1f, 1f);

        /// <summary>
        ///     Colour for important information.
        /// </summary>
        public static readonly Vector4 Important = new(0.77f, 0.7f, 0.965f, 1f);
    }
}
