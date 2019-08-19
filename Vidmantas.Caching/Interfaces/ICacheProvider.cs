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
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="value">The value to add to the cache</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        Task<bool> AddAsync(ICacheKey cacheKey, object value);

        /// <summary>
        /// Does the key exist in the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        Task<bool> ExistsAsync(ICacheKey cacheKey);

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        Task<bool> FlushAsync();

        /// <summary>
        /// Gets an item from the cache with the specified key
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        /// Type object if it exists in the cache, null otherwise. Also returns the underlying cache key used.
        /// </returns>
        Task<(T cachedValue, string cacheKey)> GetAsync<T>(ICacheKey cacheKey) where T : class;

        /// <summary>
        /// Removes an items with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        Task<bool> RemoveAsync(ICacheKey cacheKey);

        #endregion
    }
}
