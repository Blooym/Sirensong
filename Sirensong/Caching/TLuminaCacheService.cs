using System;
using Dalamud;
using Lumina.Excel;
using Sirensong.Caching.Collections;
using Sirensong.Caching.Internal.Interfaces;
using Sirensong.IoC.Internal;

namespace Sirensong.Caching
{
    /// <summary>
    /// Caching for Lumina Rows and Subrows.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SirenServiceClass]
    public sealed class LuminaCacheService<T> : IDisposable, ICache where T : ExcelRow
    {
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LuminaCacheService{T}"/> class.
        /// </summary>
        private LuminaCacheService()
        {

        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.cache.Dispose();
                this.subRowCache.Dispose();

                this.disposedValue = true;

                GC.SuppressFinalize(this);
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(LuminaCacheService<T>));
            }

            this.cache.Clear();
            this.subRowCache.Clear();
        }

        /// <inheritdoc />
        public void HandleExpired()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(LuminaCacheService<T>));
            }

            this.cache.HandleExpired();
            this.subRowCache.HandleExpired();
        }

        /// <summary>
        /// The <see cref="ExcelSheet{T}" /> associated with this cache.
        /// </summary>
        private static readonly ExcelSheet<T> Sheet = SharedServices.DataManager.GetExcelSheet<T>()!;

        /// <summary>
        /// A timed cache of the rows.
        /// </summary>
        private readonly CacheCollection<uint, T> cache = new();

        /// <summary>
        /// The dictionary of subrow caches.
        /// </summary>
        private readonly CacheCollection<Tuple<uint, uint>, T> subRowCache = new(new CacheOptions<Tuple<uint, uint>, T>()
        {
            SlidingExpiry = TimeSpan.FromMinutes(10),
            AbsoluteExpiry = null,
            ExpireInterval = TimeSpan.FromMinutes(1),
            UseBuiltInExpire = true,
        });

        /// <summary>
        /// Gets the sheet for the current language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public ExcelSheet<T> OfLanguage(ClientLanguage language) => SharedServices.DataManager.GetExcelSheet<T>(language)!;

        /// <summary>
        /// Gets a row from the sheet and caches it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public T? GetRow(uint id)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(LuminaCacheService<T>));
            }

            return this.cache.GetOrAdd(id, value => LuminaCacheService<T>.Sheet.GetRow(id)!);
        }

        /// <summary>
        /// Gets a row from the sheet, using the subrow and caches it.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="subRow"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public T? GetRow(uint row, uint subRow)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(LuminaCacheService<T>));
            }

            var targetRow = new Tuple<uint, uint>(row, subRow);
            return this.subRowCache.GetOrAdd(targetRow, value => LuminaCacheService<T>.Sheet.GetRow(row, subRow)!);
        }
    }
}
