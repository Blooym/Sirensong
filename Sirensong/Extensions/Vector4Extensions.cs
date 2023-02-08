using System.Numerics;
using ImGuiNET;

namespace Sirensong.Extensions
{
    public static class Vector4Extensions
    {
        /// <summary>
        /// Converts a <see cref="Vector4"/> to a <see cref="uint"/>.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static uint ToUint(this Vector4 vector) => ImGui.GetColorU32(vector);
    }
}