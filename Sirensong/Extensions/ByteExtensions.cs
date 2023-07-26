namespace Sirensong.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        ///     Converts a byte the specified enum type <typeparamref name="T"/>.
        /// </summary>
        public static T ToEnum<T>(this byte value) where T : struct => (T)(object)value;
    }
}
