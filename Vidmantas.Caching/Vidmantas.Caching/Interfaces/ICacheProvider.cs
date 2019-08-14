namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System.Threading.Tasks;

    #endregion

    public interface ICacheProvider
    {
        #region Public Methods

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="parentKey">The parent key grouping related cache entries</param>
        /// <param name="valueKey">The value key to look up the entry value</param>
        /// <param name="valueKeyModifier">The value key modifier</param>
        /// <param name="value">The value to add to the cache</param>
        Task<ICachingResult> AddAsync(string parentKey, string valueKey, object valueKeyModifier, object value);

        /// <summary>
        /// Does the key exist in the cache
        /// </summary>
        /// <param name="parentKey">The parent key grouping related cache entries</param>
        /// <param name="valueKey">The value key to look up the entry value</param>
        /// <param name="valueKeyModifier">The value key modifier</param>
        /// <returns><see cref="ICachingResult"/></returns>
        Task<ICachingResult> ExistsAsync(string parentKey, string valueKey, object valueKeyModifier);

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        Task<bool> FlushAsync();

        /// <summary>
        /// Gets an item from the cache with the specified key
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="parentKey">The parent key grouping related cache entries</param>
        /// <param name="valueKey">The value key to look up the entry value</param>
        /// <param name="valueKeyModifier">The value key modifier</param>
        /// <returns>Type object if it exists in the cache, null otherwise. Also returns the underlying cache key used.</returns>
        Task<(T cachedValue, string cacheKey)> GetAsync<T>(string parentKey, string valueKey, object valueKeyModifier) where T : class;

        /// <summary>
        /// Removes an items with the specified key from the cache
        /// </summary>
        /// <param name="parentKey">The parent key grouping related cache entries</param>
        /// <param name="valueKey">The value key to look up the entry value</param>
        /// <param name="valueKeyModifier">The value key modifier</param>
        /// <returns><see cref="ICachingResult"/></returns>
        Task<ICachingResult> RemoveAsync(string parentKey, string valueKey, object valueKeyModifier);

        #endregion
    }
}
