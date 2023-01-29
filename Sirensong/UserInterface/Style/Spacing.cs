using System.Numerics;

namespace Sirensong.UserInterface.Style
{
    /// <summary>
    /// Spacing values for UI design.
    /// </summary>
    public static class Spacing
    {
        /// <summary>
        /// The amount of space between sidebar elements.
        /// </summary>
        public static readonly Vector2 SidebarElementSpacing = new(0, 15);

        /// <summary>
        /// The amount of space between sections.
        /// </summary>
        public static readonly Vector2 SectionSpacing = new(0, 10);

        /// <summary>
        /// The amount of space under a text header.
        /// </summary>
        public static readonly Vector2 HeaderSpacing = new(0, 2);

        /// <summary>
        /// The amount of space above a text footer.
        /// </summary>
        public static readonly Vector2 FooterSpacing = new(0, 5);

        /// <summary>
        /// The amount of space under a collapsible header.
        /// </summary>
        public static readonly Vector2 CollapsibleHeaderSpacing = new(0, 5);
    }
}