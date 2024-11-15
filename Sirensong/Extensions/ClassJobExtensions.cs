using Lumina.Excel.Sheets;
using Sirensong.Game.Enums;

namespace Sirensong.Extensions
{
    public static class ClassJobExtensions
    {
        /// <summary>
        ///     Get class job role from class job by casting <see cref="ClassJob.Role" /> to <see cref="ClassJobRole" />.
        /// </summary>
        /// <param name="classJob"></param>
        /// <returns></returns>
        public static ClassJobRole GetJobRole(this ClassJob classJob) => (ClassJobRole)classJob.Role;
    }
}
