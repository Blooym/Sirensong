using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Sirensong.Caching.Internal.Interfaces;

namespace Sirensong.Caching.Collections
{
    /// <summary>
    /// Provides a cache for a given Key, Value pair, built ontop of a <see cref="ConcurrentDictionary{TKey,TValue}"/>, implements an automatic cache cleaner system for expiring items.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     When an item is accessed its access time is updated, and it will be removed from the cache when the SlidingExpiry is reached.
    /// </para>
    /// <para>
    ///     When an item is created or updated its update time is updated, and it will be removed from the cache when the AbsoluteExpiry is reached.
    /// </para>
    /// <para>
    ///     If both <see cref="CacheOptions{TKey, TValue}.SlidingExpiry"/> and <see cref="CacheOptions{TKey, TValue}.AbsoluteExpiry"/> are set, the item will be removed from the cache when either is reached.
    /// </para>
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ICache, IDisposable where TKey : notnull where TValue : notnull
    {
        private bool disposedValue;

        /// <summary>
        /// The dictionary of values for the cache.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, TValue> cache = new();

        /// <summary>
        /// The dictionary of Keys to <see cref="KeyExpiryInfo"/>.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, KeyExpiryInfo> accessTimes = new();

        /// <summary>
        /// The timer for cleaning the cache.
        /// </summary>
        private Timer? cacheCleanTimer;

        /// <summary>
        /// The lock object for the cache.
        /// </summary>
        private readonly object lockObject = new();

        /// <summary>
        /// The <see cref="CacheOptions{TKey, TValue}"/> for this cache.
        /// </summary>
        private readonly CacheOptions<TKey, TValue> options;

        /// <summary>
        /// All the keys in the cache.
        /// </summary>
        public IReadOnlyCollection<TKey> Keys => this.cache.Keys.ToList().AsReadOnly();

        /// <summary>
        /// All the values in the cache.
        /// </summary>
        public IReadOnlyCollection<TValue> Values => this.cache.Values.ToList().AsReadOnly();

        /// <summary>
        /// Gets the enumerator for the cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.cache.GetEnumerator();

        /// <summary>
        /// Gets the enumerator for the cache.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => this.cache.GetEnumerator();

        /// <summary>
        /// Creates a new <see cref="CacheCollection{TKey,TValue}" /> with default options.
        /// </summary>
        public CacheCollection() : this(new CacheOptions<TKey, TValue>()) => this.SetupTimer();

        /// <summary>
        /// Creates a new <see cref="CacheCollection{TKey,TValue}" /> with the specified options.
        /// </summary>
        /// <param name="options">The <see cref="CacheOptions{TKey, TValue}"/> for this cache.</param>
        public CacheCollection(CacheOptions<TKey, TValue> options)
        {
            this.options = options;
            this.SetupTimer();
        }

        /// <summary>
        /// Sets up the timer for the cache cleaner.
        /// </summary>
        private void SetupTimer()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            if (!this.options.UseBuiltInExpire || (!this.options.AbsoluteExpiry.HasValue && !this.options.SlidingExpiry.HasValue))
            {
                return;
            }

            this.cacheCleanTimer?.Stop();
            this.cacheCleanTimer?.Dispose();
            this.cacheCleanTimer = new Timer(this.options.ExpireInterval.HasValue ? this.options.ExpireInterval.Value.TotalMilliseconds : 5000);

            this.cacheCleanTimer.Elapsed += (sender, args) => this.HandleExpired();
            this.cacheCleanTimer.Start();
        }

        /// <summary>
        /// Disposes of the cache and all its keys and values if they implement <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            if (this.disposedValue)
            {
                if (this.cacheCleanTimer != null)
                {
                    this.cacheCleanTimer.Stop();
                    this.cacheCleanTimer.Dispose();
                    this.cacheCleanTimer.Elapsed -= (sender, args) => this.HandleExpired();
                }

                // Dispose of the keys and values.
                this.RemoveAllKV();

                this.disposedValue = true;

                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Checks if the given key is expired and removes it if it is.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is expired (and was removed from the cache), false otherwise.</returns>
        private bool HandleExpired(TKey key)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                if (this.accessTimes.TryGetValue(key, out var expiryInfo))
                {
                    // We have to access the cache directly to avoid updating the access time.
                    var value = this.cache.GetValueOrDefault(key);

                    if (this.options.AbsoluteExpiry.HasValue && expiryInfo.LastUpdateTime + this.options.AbsoluteExpiry.Value < DateTimeOffset.Now)
                    {
                        this.cache.TryRemove(key, out _);
                        this.accessTimes.TryRemove(key, out _);
                        this.options.OnExpiry?.Invoke(key, value!);
                        SirenLog.Verbose($"Cache key {key} expired due to absolute expiry (AbsoluteExpiry: {this.options.AbsoluteExpiry.Value}).");
                        return true;
                    }
                    else if (this.options.SlidingExpiry.HasValue && expiryInfo.LastAccessTime + this.options.SlidingExpiry.Value < DateTimeOffset.Now)
                    {
                        this.cache.TryRemove(key, out _);
                        this.accessTimes.TryRemove(key, out _);
                        this.options.OnExpiry?.Invoke(key, value!);
                        SirenLog.Verbose($"Cache key {key} expired due to sliding expiry (SlidingExpiry: {this.options.SlidingExpiry.Value}).");
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Removes of all keys and values in the cache and disposes if they implement <see cref="IDisposable"/>.
        /// </summary>
        private void RemoveAllKV()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                foreach (var kv in this.cache)
                {
                    if (kv.Key is IDisposable disposableKey)
                    {
                        disposableKey.Dispose();
                    }

                    if (kv.Value is IDisposable disposableValue)
                    {
                        disposableValue.Dispose();
                    }
                }

                this.cache.Clear();
                this.accessTimes.Clear();
            }
        }

        /// <summary>
        /// Updates the access time for the given key.
        /// </summary>
        /// <param name="key">The key to update.</param>
        private void UpdateAccessedTime(TKey key)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                var expiryInfo = this.accessTimes.GetOrAdd(key, new KeyExpiryInfo());

                this.accessTimes.AddOrUpdate(key, expiryInfo, (k, v) =>
                {
                    expiryInfo.Accessed();
                    return expiryInfo;
                });
            }
        }

        /// <summary>
        /// Updates the update time for the given key.
        /// </summary>
        /// <param name="key">The key to update.</param>
        private void UpdateUpdatedTime(TKey key)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                var expiryInfo = this.accessTimes.GetOrAdd(key, new KeyExpiryInfo());

                this.accessTimes.AddOrUpdate(key, expiryInfo, (k, v) =>
                {
                    expiryInfo.Updated();
                    return expiryInfo;
                });
            }
        }

        /// <summary>
        /// Gets or sets the value for the given key.
        /// </summary>
        /// <param name="key">The key to get or set.</param>
        /// <returns>The value for the given key or null if the key is not found.</returns>
        public TValue this[TKey key]
        {
            get
            {
                if (this.disposedValue)
                {
                    throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
                }

                lock (this.lockObject)
                {
                    if (this.HandleExpired(key))
                    {
                        return default!;
                    }

                    this.UpdateAccessedTime(key);
                    return this.cache[key];
                }
            }
            set
            {
                if (this.disposedValue)
                {
                    throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
                }

                lock (this.lockObject)
                {
                    this.UpdateUpdatedTime(key);
                    this.cache[key] = value;
                }
            }
        }

        /// <summary>
        /// Gets the value for the given sealedkey.
        /// </summary>
        /// <param name="key">The key to get.</param>
        /// <param name="value">The value for the given key.</param>
        /// <returns>True if the key was found, false otherwise.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                if (this.HandleExpired(key))
                {
                    value = default!;
                    return false;
                }

                this.UpdateAccessedTime(key);
                return this.cache.TryGetValue(key, out value!);
            }
        }

        /// <summary>
        /// Gets or creates the value for the given key.
        /// </summary>
        /// <param name="key">The key to get or create.</param>
        /// <param name="valueFactory">The factory to create the value if it doesn't exist.</param>
        /// <returns>The value for the given key.</returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                if (this.HandleExpired(key))
                {
                    this.cache.TryRemove(key, out _);
                }

                this.UpdateAccessedTime(key);
                return this.cache.GetOrAdd(key, valueFactory);
            }
        }

        /// <summary>
        /// Gets or creates the value for the given key.
        /// </summary>
        /// <param name="key">The key to get or create.</param>
        /// <param name="value">The value to create if it doesn't exist.</param>
        /// <returns>The value for the given key.</returns>
        public TValue GetOrAdd(TKey key, TValue value)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                if (this.HandleExpired(key))
                {
                    this.cache.TryRemove(key, out _);
                }

                this.UpdateAccessedTime(key);
                return this.cache.GetOrAdd(key, value);
            }
        }

        /// <summary>
        /// Adds or updates the value for the given key.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to add or update.</param>
        public void AddOrUpdate(TKey key, TValue value)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                this.cache.AddOrUpdate(key, value, (k, v) => value);
                this.UpdateUpdatedTime(key);
            }
        }

        /// <summary>
        ///Adds or updates the value for the given key.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to add or update.</param>
        /// <param name="updateValueFactory">The factory to update the value if it already exists.</param>
        public void AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                this.cache.AddOrUpdate(key, value, updateValueFactory);
                this.UpdateUpdatedTime(key);
            }
        }

        /// <summary>
        /// Attempts to remove the key and value from the cache.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <param name="value"></param>
        /// <returns>True if removed, false otherwise.</returns>
        public bool TryRemove(TKey key, out TValue value)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                this.accessTimes.TryRemove(key, out _);
                return this.cache.TryRemove(key, out value!);
            }
        }

        /// <summary>
        /// Attempts to remove the key and value from the cache.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <param name="value"></param>
        /// <returns>True when a value is found in the dictionary with the specified key; false when the dictionary cannot find a value associated with the specified key.</returns>
        public bool Remove(TKey key, out TValue value)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                this.accessTimes.Remove(key, out _);
                return this.cache.Remove(key, out value!);
            }
        }

        /// <summary>
        /// Checks if the given key exists.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists, false otherwise.</returns>
        public bool ContainsKey(TKey key)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                if (this.HandleExpired(key))
                {
                    this.cache.TryRemove(key, out _);
                    return false;
                }

                this.UpdateAccessedTime(key);
                return this.cache.ContainsKey(key);
            }
        }

        /// <inheritdoc/>
        public void HandleExpired()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CacheCollection<TKey, TValue>));
            }

            lock (this.lockObject)
            {
                foreach (var key in this.Keys)
                {
                    this.HandleExpired(key);
                }
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear() => this.RemoveAllKV();

        /// <summary>
        /// Represents expiry information for a key.
        /// </summary>
        private struct KeyExpiryInfo
        {
            /// <summary>
            /// The last time the key was accessed.
            /// </summary>
            public DateTime LastAccessTime { get; private set; } = DateTime.Now;

            /// <summary>
            /// The last time the key was updated.
            /// </summary>
            public DateTime LastUpdateTime { get; private set; } = DateTime.Now;

            /// <summary>
            /// Creates a new instance of <see cref="KeyExpiryInfo"/> with the current time.
            /// </summary>
            public KeyExpiryInfo()
            {

            }

            /// <summary>
            /// Creates a new instance of <see cref="KeyExpiryInfo"/> with the given times.
            /// </summary>
            /// <param name="lastAccessTime"></param>
            /// <param name="lastUpdateTime"></param>
            public KeyExpiryInfo(DateTime lastAccessTime, DateTime lastUpdateTime)
            {
                this.LastAccessTime = lastAccessTime;
                this.LastUpdateTime = lastUpdateTime;
            }

            /// <summary>
            /// Updates the last access time to the current time.
            /// </summary>
            public void Accessed() => this.LastAccessTime = DateTime.Now;

            /// <summary>
            /// Updates the last update time to the current time.
            /// </summary>
            public void Updated()
            {
                this.Accessed();
                this.LastUpdateTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// Represents options for a timed cache.
    /// </summary>
    public struct CacheOptions<TKey, TValue>
    {
        /// <summary>
        /// The sliding expiry time for the cache. Items in the cache will expire after this amount of time since they were last accessed.
        /// Default: null.
        /// </summary>
        public TimeSpan? SlidingExpiry { get; set; } = null;

        /// <summary>
        /// The absolute expiry time for the cache. Items in the cache will expire after this amount of time since they were last updated.
        /// Default: 1 hour.
        /// </summary>
        public TimeSpan? AbsoluteExpiry { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        ///Called when an item is expired from the cache.
        ///Default: null.
        /// </summary>
        public Action<TKey, TValue>? OnExpiry { get; set; }

        /// <summary>
        /// If true, the cache will use the built in expiry system to expire items automatically.
        /// If false, the cache will not expire items automatically, and items will be expired on access or when <see cref="CacheCollection{TKey, TValue}.HandleExpired()"/> is called.
        /// Default: true.
        /// </summary>
        public bool UseBuiltInExpire { get; set; } = true;

        /// <summary>
        /// The interval to check for expired items, only used if <see cref="UseBuiltInExpire"/> is true.
        /// Defaults to 1 minute.
        /// </summary>
        public TimeSpan? ExpireInterval { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Creates a new instance of <see cref="CacheOptions{TKey, TValue}"/>.
        /// </summary>
        public CacheOptions()
        {

        }
    }
}
