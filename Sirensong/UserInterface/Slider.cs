using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     A <see cref="ImGui.SliderInt(string, ref int, int, int)" /> element.
        /// </summary>
        /// <param name="label">The label of the slider.</param>
        /// <param name="value">The reference to the value of the slider.</param>
        /// <param name="min">The minimum value of the slider.</param>
        /// <param name="max">The maximum value of the slider.</param>
        /// <param name="debounced">Whether the slider should only update when the user releases the slider.</param>
        /// <returns>Whether the slider was updated.</returns>
        public static bool SliderInt(string label, ref int value, int min, int max, bool debounced = false)
        {
            var sliderChanged = ImGui.SliderInt(label, ref value, min, max);

            if (sliderChanged)
            {
                if (value < min)
                {
                    value = min;
                }
                else if (value > max)
                {
                    value = max;
                }
            }
            return debounced ? ImGui.IsItemDeactivated() : sliderChanged;
        }

        /// <summary>
        ///     A <see cref="ImGui.SliderFloat(string, ref float, float, float)" /> element.
        /// </summary>
        /// <param name="label">The label of the slider.</param>
        /// <param name="value">The reference to the value of the slider.</param>
        /// <param name="min">The minimum value of the slider.</param>
        /// <param name="max">The maximum value of the slider.</param>
        /// <param name="debounced">Whether the slider should only update when the user releases the slider.</param>
        /// <returns>Whether the slider was updated.</returns>
        public static bool SliderFloat(string label, ref float value, float min, float max, bool debounced = false)
        {
            var sliderChanged = ImGui.SliderFloat(label, ref value, min, max);

            if (sliderChanged)
            {
                if (value < min)
                {
                    value = min;
                }
                else if (value > max)
                {
                    value = max;
                }
            }
            return debounced ? ImGui.IsItemDeactivated() : sliderChanged;
        }
    }
}
