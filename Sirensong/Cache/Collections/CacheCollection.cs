using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Sirensong.Cache.Collections
{
    /// <summary>
    ///     A collection of keys and values that expire after a certain amount of time.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class CacheCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDisposable where TKey : notnull
    {
        private bool disposedValue;

        /// <summary>
        ///     The underlying dictionary of keys to their expiry information.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, KeyExpiryInfo> accessTimes = new();

        /// <summary>
        ///     The underlying dictionary of keys to values for caching.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, TValue> cache = new();

        /// <summary>
        ///     The timer for expiring keys.
        /// </summary>
        private readonly Timer expiryTimer;

        /// <summary>
        ///     The <see cref="CacheOptions{TKey, TValue}" /> that this cache uses.
        /// </summary>
        private readonly CacheOptions<TKey, TValue> options;

        /// <summary>
        ///     Creates a new <see cref="CacheCollection{TKey,TValue}" /> with default options.
        /// </summary>
        public CacheCollection() : this(new CacheOptions<TKey, TValue>())
        {

        }

        /// <summary>
        ///     Creates a new <see cref="CacheCollection{TKey,TValue}" /> with the specified options.
        /// </summary>
        /// <param name="options">The <see cref="CacheOptions{TKey, TValue}" /> for this cache.</param>
        public CacheCollection(CacheOptions<TKey, TValue> options)
        {
            this.options = options;

            this.expiryTimer = new Timer(this.options.ExpireInterval.TotalMilliseconds);
            this.expiryTimer.Elapsed += this.ExpireKeys;
            this.expiryTimer.Start();
        }

        /// <summary>
        ///     Gets the given key from the cache.
        /// </summary>
        /// <param name="key">The key to get.</param>
        /// <returns>The value for the given key.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public TValue this[TKey key]
        {
            get
            {
                if (this.disposedValue)
                {
                    throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
                }

                if (this.cache.TryGetValue(key, out var value))
                {
                    if (this.IsExpired(key))
                    {
                        this.Expire(key);
                        throw new KeyNotFoundException();
                    }

                    if (this.accessTimes.TryGetValue(key, out var expiryInfo))
                    {
                        expiryInfo.Accessed();
                        this.accessTimes[key] = expiryInfo;
                    }

                    return value;
                }

                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        ///     All the keys in the cache.
        /// </summary>
        public ICollection<TKey> Keys => this.cache.Keys;

        /// <summary>
        ///     All the values in the cache.
        /// </summary>
        public ICollection<TValue> Values => this.cache.Values;

        /// <summary>
        ///     Disposes of the cache and all its values.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.expiryTimer.Elapsed -= this.ExpireKeys;
                this.expiryTimer.Stop();
                this.expiryTimer.Dispose();

                foreach (var key in this.cache.Keys.ToArray())
                {
                    this.RemoveKey(key, true);
                }

                GC.SuppressFinalize(this);

                this.disposedValue = true;
            }
        }

        /// <summary>
        ///     Gets the enumerator for the cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.cache.GetEnumerator();

        /// <summary>
        ///     Gets the enumerator for the cache.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => this.cache.GetEnumerator();

        /// <summary>
        ///     Removes a key from the cache and optionally disposes of it.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <param name="dispose">Whether or not to dispose of the value.</param>
        private void RemoveKey(TKey key, bool dispose)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            if (key is null)
            {
                return;
            }

            if (this.cache.TryGetValue(key, out var value))
            {
                this.cache.Remove(key, out _);
                this.accessTimes.Remove(key, out _);
                this.options.OnExpiry?.Invoke(key, value);

                if (dispose)
                {
                    if (key is IDisposable disposableKey)
                    {
                        disposableKey.Dispose();
                    }

                    if (value is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Checks to see if the given key has expired.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key has expired, false otherwise.</returns>
        public bool IsExpired(TKey key)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            if (!this.cache.ContainsKey(key) || !this.accessTimes.ContainsKey(key))
            {
                return false;
            }

            if (this.options.AbsoluteExpiry.HasValue)
            {
                if (DateTimeOffset.Now - this.accessTimes[key].LastUpdateTime > this.options.AbsoluteExpiry)
                {
                    return true;
                }
            }

            if (this.options.SlidingExpiry.HasValue)
            {
                if (DateTimeOffset.Now - this.accessTimes[key].LastAccessTime > this.options.SlidingExpiry)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Expires the given key.
        /// </summary>
        /// <param name="key"></param>
        private void Expire(TKey key)
        {
            this.RemoveKey(key, false);
            this.options.OnExpiry?.Invoke(key, this.cache[key]);
        }

        /// <summary>
        ///     Expires all keys in the cache that have expired.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpireKeys(object? sender, ElapsedEventArgs e)
        {
            var keys = new List<TKey>(this.cache.Keys);
            foreach (var key in keys.ToArray())
            {
                if (this.IsExpired(key))
                {
                    this.Expire(key);
                }
            }
        }

        /// <summary>
        ///     Gets a value or adds it to the cache.
        /// </summary>
        /// <param name="key">The key to get or add.</param>
        /// <param name="valueFactory">The factory to create the value if it doesn't exist.</param>
        /// <returns>The value for the given key.</returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            if (this.cache.TryGetValue(key, out var value))
            {
                if (this.IsExpired(key))
                {
                    this.Expire(key);
                }
                else
                {
                    if (this.accessTimes.TryGetValue(key, out var accessTime))
                    {
                        accessTime.Accessed();
                        this.accessTimes[key] = accessTime;
                    }

                    return value;
                }
            }

            value = valueFactory(key);
            this.cache.TryAdd(key, value);
            this.accessTimes.TryAdd(key, new KeyExpiryInfo());
            return value;
        }

        /// <summary>
        ///     Adds or updates a value in the cache.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="valueFactory">The factory to create/update the value.</param>
        public void AddOrUpdate(TKey key, Func<TKey, TValue> valueFactory)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            if (this.cache.TryGetValue(key, out _))
            {
                if (this.IsExpired(key))
                {
                    this.Expire(key);
                }
                else
                {
                    if (this.accessTimes.TryGetValue(key, out var accessTime))
                    {
                        accessTime.Updated();
                        this.accessTimes[key] = accessTime;
                    }

                    this.cache[key] = valueFactory(key);
                    return;
                }
            }

            var value = valueFactory(key);
            this.cache.TryAdd(key, value);
            this.accessTimes.TryAdd(key, new KeyExpiryInfo());
        }

        /// <summary>
        ///     Removes a key from the cache and disposes of it if it implements <see cref="IDisposable" />.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public void Remove(TKey key)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }
            this.RemoveKey(key, true);
        }

        /// <summary>
        ///     Removes everything from the cache and disposes keys/values if set to dispose and they implement
        ///     <see cref="IDisposable" />.
        /// </summary>
        /// <param name="dispose">Whether to dispose of the keys.</param>
        public void Clear(bool dispose = true)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            foreach (var key in this.cache.Keys.ToArray())
            {
                this.RemoveKey(key, dispose);
            }
        }

        /// <summary>
        ///     Represents expiry information for a key.
        /// </summary>
        private struct KeyExpiryInfo
        {
            /// <summary>
            ///     The last time the key was accessed.
            /// </summary>
            public DateTime LastAccessTime { get; private set; } = DateTime.Now;

            /// <summary>
            ///     The last time the key was updated.
            /// </summary>
            public DateTime LastUpdateTime { get; private set; } = DateTime.Now;

            /// <summary>
            ///     Creates a new instance of <see cref="KeyExpiryInfo" /> with the current time.
            /// </summary>
            public KeyExpiryInfo()
            {

            }

            /// <summary>
            ///     Creates a new instance of <see cref="KeyExpiryInfo" /> with the given times.
            /// </summary>
            /// <param name="lastAccessTime"></param>
            /// <param name="lastUpdateTime"></param>
            public KeyExpiryInfo(DateTime lastAccessTime, DateTime lastUpdateTime)
            {
                this.LastAccessTime = lastAccessTime;
                this.LastUpdateTime = lastUpdateTime;
            }

            /// <summary>
            ///     Updates the last access time to the current time.
            /// </summary>
            public void Accessed() => this.LastAccessTime = DateTime.Now;

            /// <summary>
            ///     Updates the last update time to the current time.
            /// </summary>
            public void Updated()
            {
                this.Accessed();
                this.LastUpdateTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    ///     Represents options for a timed cache.
    /// </summary>
    public readonly struct CacheOptions<TKey, TValue>
    {
        /// <summary>
        ///     The sliding expiry time for the cache. Items in the cache will expire after this amount of time since they were
        ///     last accessed.
        ///     Default: null.
        /// </summary>
        public readonly TimeSpan? SlidingExpiry { get; init; } = null;

        /// <summary>
        ///     The absolute expiry time for the cache. Items in the cache will expire after this amount of time since they were
        ///     last updated.
        ///     Default: 1 hour.
        /// </summary>
        public readonly TimeSpan? AbsoluteExpiry { get; init; } = TimeSpan.FromHours(1);

        /// <summary>
        ///     Called when an item is expired from the cache.
        ///     Default: null.
        /// </summary>
        public readonly Action<TKey, TValue>? OnExpiry { get; init; }

        /// <summary>
        ///     The interval to check for expired items, only used if <see cref="UseBuiltInExpire" /> is true.
        ///     Defaults to 1 minute.
        /// </summary>
        public readonly TimeSpan ExpireInterval { get; init; } = TimeSpan.FromMinutes(1);

        /// <summary>
        ///     Creates a new instance of <see cref="CacheOptions{TKey, TValue}" />.
        /// </summary>
        public CacheOptions()
        {

        }
    }
}
