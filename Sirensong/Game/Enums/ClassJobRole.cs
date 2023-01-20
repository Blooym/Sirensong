using FFXIVClientStructs.FFXIV.Common.Math;
using Lumina.Excel.GeneratedSheets;
using Sirensong.UserInterface;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Sirensong.Game.Enums
{
    /// <summary>
    ///     A mapping of <see cref="ClassJob"/> role IDs to their names.
    /// </summary>
    public enum ClassJobRole : uint
    {
        Misc = 0,
        Tank = 1,
        MeleeDPS = 2,
        RangedDPS = 3,
        Healer = 4,
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    ///     Extensions for <see cref="ClassJobRole" />
    /// </summary>
    public static class ClassJobRoleExtensions
    {
        public static Vector4 GetColourForRole(this ClassJobRole roleID) => SiUI.GetColourForRole((uint)roleID);
    }
}
