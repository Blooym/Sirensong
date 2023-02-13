using System.Numerics;

namespace Sirensong.UserInterface.Style
{
    /// <summary>
    ///     Spacing values for UI design.
    /// </summary>
    public static class Spacing
    {
        /// <summary>
        ///     The amount of space between sidebar sections.
        /// </summary>
        public static readonly Vector2 SidebarSectionSpacing = new(0, 15);

        /// <summary>
        ///     The amount of space between regular sections.
        /// </summary>
        public static readonly Vector2 SectionSpacing = new(0, 10);

        /// <summary>
        ///     The amount of spacing to make individual items more readable.
        /// </summary>
        public static readonly Vector2 ReadableSpacing = new(0, 5);

        /// <summary>
        ///     The amount of space under a text header.
        /// </summary>
        public static readonly Vector2 HeaderSpacing = new(0, 2);

        /// <summary>
        ///     The amount of space above a text footer.
        /// </summary>
        public static readonly Vector2 FooterSpacing = new(0, 5);

        /// <summary>
        ///     The amount of space under a collapsible header.
        /// </summary>
        public static readonly Vector2 InnerCollapsibleHeaderSpacing = new(0, 5);
    }
}
