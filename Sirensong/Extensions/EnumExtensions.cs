namespace Sirensong.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     Converts a byte to an enum.
        /// </summary>
        public static T ToEnum<T>(this byte value) where T : struct => (T)(object)value;
    }
}
