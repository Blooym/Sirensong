using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;

namespace Sirensong.Game.Extensions
{
    /// <summary>
    ///     Extensions for <see cref="ClassJob"/>.
    /// </summary>
    public static class ClassJobExtensions
    {
        /// <summary>
        ///     Gets the role name for the given <see cref="ClassJob"/>.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static string GetRoleName(this ClassJob job) => job switch
        {
            { Role: (byte)ClassJobRole.Misc } => "Misc",
            { Role: (byte)ClassJobRole.Tank } => "Tank",
            { Role: (byte)ClassJobRole.MeleeDPS } => "Melee DPS",
            { Role: (byte)ClassJobRole.RangedDPS } => "Ranged DPS",
            { Role: (byte)ClassJobRole.Healer } => "Healer",
            _ => "Unknown",
        };
    }
}
