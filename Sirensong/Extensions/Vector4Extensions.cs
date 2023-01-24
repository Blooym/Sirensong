using FFXIVClientStructs.FFXIV.Common.Math;

namespace Sirensong.Extensions
{
    public static class Vector4Extensions
    {
        public static uint ToUint(this Vector4 vector) => ((uint)(vector.X * 255) << 24) | ((uint)(vector.Y * 255) << 16) | ((uint)(vector.Z * 255) << 8) | (uint)(vector.W * 255);
    }
}