namespace Sirensong.Caching.Internal.Interfaces
{
    /// <summary>
    /// An interface for a cache that can be cleaned up.
    /// </summary>
    internal interface ICache
    {
        /// <summary>
        /// Cleans up old entries that have expired.
        /// </summary>
        void HandleExpired();

        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        void Clear();
    }
}