using Dalamud.Bindings.ImGui;

namespace Sirensong.UserInterface
{
    public static partial class SiGui
    {
        /// <summary>
        ///     A <see cref="ImGui.Checkbox(string, ref bool)" /> element.
        /// </summary>
        /// <param name="label">The label of the checkbox.</param>
        /// <param name="value">The reference to the value of the checkbox.</param>
        /// <returns>Whether the checkbox was updated.</returns>
        public static bool Checkbox(string label, ref bool value)
        {
            var checkbox = ImGui.Checkbox(label, ref value);
            return checkbox;
        }

        /// <summary>
        ///     A <see cref="ImGui.Checkbox(string, ref bool)" /> element.
        /// </summary>
        /// <param name="label">The label of the checkbox.</param>
        /// <param name="description">The description of the checkbox.</param>
        /// <param name="value">The reference to the value of the checkbox.</param>
        /// <returns>Whether the checkbox was updated.</returns>
        public static bool Checkbox(string label, string description, ref bool value)
        {
            var checkbox = ImGui.Checkbox(label, ref value);
            TextDisabledWrapped(description);
            return checkbox;
        }
    }
}
