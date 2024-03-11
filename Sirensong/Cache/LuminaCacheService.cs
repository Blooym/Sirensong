using System;
using BitFaster.Caching;
using BitFaster.Caching.Lru;
using Dalamud;
using Lumina.Excel;
using Sirensong.IoC.Internal;

namespace Sirensong.Cache
{
    /// <summary>
    ///     Caching for Lumina Rows and Subrows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SirenServiceClass]
    public sealed class LuminaCacheService<T> : IDisposable where T : ExcelRow
    {
        private bool disposedValue;

        /// <summary>
        ///     The <see cref="ExcelSheet{T}" /> associated with this cache.
        /// </summary>
        private static readonly ExcelSheet<T> Sheet = SharedServices.DataManager.GetExcelSheet<T>()!;

        /// <summary>
        ///     A cache of the rows and subrows.
        /// </summary>
        private readonly ICache<Tuple<uint, uint?>, T?> cache = new ConcurrentLruBuilder<Tuple<uint, uint?>, T?>()
                .WithAtomicGetOrAdd()
                .WithCapacity(200)
                .WithExpireAfterWrite(TimeSpan.FromMinutes(5))
                .Build();

        /// <summary>
        ///     Initializes a new instance of the <see cref="LuminaCacheService{T}" /> class.
        /// </summary>
        private LuminaCacheService()
        {

        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.disposedValue = true;
            }
        }

        /// <summary>
        ///     Gets the sheet for the current language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public ExcelSheet<T> OfLanguage(ClientLanguage language) => SharedServices.DataManager.GetExcelSheet<T>(language)!;

        /// <summary>
        ///     Gets a row from the sheet and caches it.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public T? GetRow(uint row)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(LuminaCacheService<T>));

            var targetRow = new Tuple<uint, uint?>(row, null);
            return this.cache.GetOrAdd(targetRow, value => Sheet.GetRow(row));
        }

        /// <summary>
        ///     Gets a row from the sheet, using the subrow and caches it.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="subRow"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public T? GetRow(uint row, uint subRow)
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(LuminaCacheService<T>));

            var targetRow = new Tuple<uint, uint?>(row, subRow);
            return this.cache.GetOrAdd(targetRow, value => Sheet.GetRow(row, subRow)!);
        }
    }
}
