using System.Numerics;
using ImGuiNET;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        /// Draws a text input field with a label.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="value">The reference to the string value.</param>
        /// <param name="maxLength">The max length of the text input field </param>
        /// <param name="debounced">Whether or not the input should be debounced.</param>
        /// <param name="flags">The <see cref="ImGuiInputTextFlags"/> to use.</param>
        /// <returns>True when either the input is typed into or when it is deactivated if debounced.</returns>
        public static bool InputText(string label, ref string value, uint maxLength, bool debounced = false, ImGuiInputTextFlags flags = default)
        {
            var inputBool = ImGui.InputText(label, ref value, maxLength, flags);
            return debounced ? ImGui.IsItemDeactivated() : inputBool;
        }

        /// <summary>
        /// Draws a text input field with a label and a hint.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="hint">The hint to display.</param>
        /// <param name="value">The reference to the string value.</param>
        /// <param name="maxLength">The max length of the text input field</param>
        /// <param name="debounced">Whether or not the input should be debounced.</param>
        /// <param name="flags">The <see cref="ImGuiInputTextFlags"/> to use.</param>
        /// <returns>True when either the input is typed into or when it is deactivated if debounced.</returns>
        public static bool InputTextHint(string label, string hint, ref string value, uint maxLength, bool debounced = false, ImGuiInputTextFlags flags = default)
        {
            var inputBool = ImGui.InputTextWithHint(label, hint, ref value, maxLength, flags);
            return debounced ? ImGui.IsItemDeactivated() : inputBool;
        }

        /// <summary>
        /// Draws a text input field with a label and a hint.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="value">The reference to the string value.</param>
        /// <param name="maxLength">The max length of the text input field </param>
        /// <param name="size">The size of the text input field.</param>
        /// <param name="debounced">Whether or not the input should be debounced.</param>
        /// <param name="flags">The <see cref="ImGuiInputTextFlags"/> to use.</param>
        /// <returns>True when either the input is typed into or when it is deactivated if debounced.</returns>
        public static bool InputTextMultiline(string label, ref string value, uint maxLength, Vector2 size, bool debounced = false, ImGuiInputTextFlags flags = default)
        {
            var inputBool = ImGui.InputTextMultiline(label, ref value, maxLength, size, flags);
            return debounced ? ImGui.IsItemDeactivated() : inputBool;
        }

        /// <summary>
        /// Draws a integer input field with a label.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="value">The reference to the integer value.</param>
        /// <param name="step">The step to increment/decrement the value by.</param>
        /// <param name="stepFast">The step to increment/decrement the value by when going fast.</param>
        /// <param name="debounced">Whether or not the input should be debounced</param>
        /// <param name="flags">The <see cref="ImGuiInputTextFlags"/> to use.</param>
        /// <returns>True when either the input is typed into or when it is deactivated if debounced.</returns>
        public static bool InputInt(string label, ref int value, int step, int stepFast, bool debounced = false, ImGuiInputTextFlags flags = default)
        {
            var inputBool = ImGui.InputInt(label, ref value, step, stepFast, flags);
            return debounced ? ImGui.IsItemDeactivated() : inputBool;
        }

        /// <summary>
        /// Draws a float input field with a label.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="value">The reference to the integer value.</param>
        /// <param name="step">The step to increment/decrement the value by.</param>
        /// <param name="stepFast">The step to increment/decrement the value by when going fast.</param>
        /// <param name="format">The format to display the value in.</param>
        /// <param name="debounced">Whether or not the input should be debounced</param>
        /// <param name="flags">The <see cref="ImGuiInputTextFlags"/> to use.</param>
        /// <returns>True when either the input is typed into or when it is deactivated if debounced.</returns>
        public static bool InputFloat(string label, ref float value, float step, float stepFast, string format, bool debounced = false, ImGuiInputTextFlags flags = default)
        {
            var inputBool = ImGui.InputFloat(label, ref value, step, stepFast, format, flags);
            return debounced ? ImGui.IsItemDeactivated() : inputBool;
        }
    }
}
