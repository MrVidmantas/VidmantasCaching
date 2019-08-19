namespace Vidmantas.Caching.Provider.NetMemory
{
    #region Usings

    using System;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading.Tasks;
    using Interfaces;

    #endregion

    public class NetMemoryCacheProvider : ICacheProvider
    {
        #region Fields

        private readonly ObjectCache _cache;

        private readonly CacheItemPolicy _policy;

        #endregion

        #region Constructors

        public NetMemoryCacheProvider(ObjectCache cache, CacheItemPolicy policy)
        {
            _cache = cache;
            _policy = policy;
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="value">The value to add to the cache</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        public Task<bool> AddAsync(ICacheKey cacheKey, object value)
        {
            var successFlag = _cache.Add(cacheKey.ToString(), value, _policy);

            return Task.FromResult(successFlag);
        }

        /// <summary>
        /// Does the key exist in the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        public async Task<bool> ExistsAsync(ICacheKey cacheKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        public Task<bool> FlushAsync()
        {
            var successFlag = true;

            try
            {
                foreach (var cacheKey in _cache.Select(x => x.Key))
                {
                    _cache.Remove(cacheKey);
                }
            }
            catch (Exception)
            {
                successFlag = false;
            }

            return Task.FromResult(successFlag);
        }

        /// <summary>
        /// Gets an item from the cache with the specified key
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        /// Type object if it exists in the cache, null otherwise. Also returns the underlying cache key used.
        /// </returns>
        public async Task<(T cachedValue, string cacheKey)> GetAsync<T>(ICacheKey cacheKey) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes an items with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        public async Task<bool> RemoveAsync(ICacheKey cacheKey)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
