namespace Vidmantas.Caching.Core
{
    #region Usings

    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;

    #endregion

    public class CacheService : ICacheService
    {
        #region Fields

        private readonly ICacheKeyFactory _cacheKeyFactory;

        private readonly ICacheProvider _cacheProvider;

        private readonly ILogger<CacheService> _logger;

        #endregion

        #region Constructors

        public CacheService(ILogger<CacheService> logger, ICacheKeyFactory cacheKeyFactory, ICacheProvider cacheProvider)
        {
            _logger = logger;
            _cacheKeyFactory = cacheKeyFactory;
            _cacheProvider = cacheProvider;
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        /// Fetches the object T or adds.
        /// </summary>
        /// <typeparam name="T">The type of item in the cache</typeparam>
        /// <param name="itemToAddFunc">The item to add function.</param>
        /// <param name="cacheKeyModifier">The cache Modifier.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="autoParentKey">The caller File Path.</param>
        /// <param name="autoMemberName">The caller Member Name.</param>
        /// <returns>
        /// The result
        /// </returns>
        public async Task<T> FetchOrAddAsync<T>(Func<Task<T>> itemToAddFunc, object cacheKeyModifier = null, bool useCache = true, [CallerFilePath]string autoParentKey = null, [CallerMemberName]string autoMemberName = null)
            where T : class
        {
            if (!useCache)
            {
                return await itemToAddFunc();
            }

            var cacheKey = await _cacheKeyFactory.CreateCacheKeyAsync(autoParentKey, autoMemberName, cacheKeyModifier);

            T cachedValue = await _cacheProvider.GetAsync<T>(cacheKey);

            if (cachedValue != null)
            {
                _logger.LogInformation($"Fetched object from the cache for {cacheKey}");

                return cachedValue;
            }

            var result = await itemToAddFunc();

            switch (result)
            {
                case null:
                    _logger.LogDebug("Fetch function result was null.");
                    return null;
                case IEnumerable enumerableResult when !enumerableResult.GetEnumerator().MoveNext():
                    _logger.LogWarning("Fetch function result was IEnumerable and will not be cached.");
                    return result;
            }

            var cacheAddResult = await _cacheProvider.AddAsync(cacheKey, result);

            if (cacheAddResult)
            {
                _logger.LogInformation($"Added an object to the cache for {cacheKey}");
            }
            else
            {
                _logger.LogWarning("There was an issue adding the object to the cache.");
            }

            return result;
        }

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        public async Task<bool> FlushAsync()
        {
            return await _cacheProvider.FlushAsync();
        }

        /// <summary>
        /// Removes the an item from cache.
        /// Does nothing if all 3 parameters are null.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="cacheKeyModifier">The cache Key Modifier.</param>
        /// <param name="autoParentKey">The reflected caller path</param>
        public async Task RemoveAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null)
        {
            if (memberName == null && cacheKeyModifier == null && autoParentKey == null)
            {
                return;
            }

            var cacheKey = await _cacheKeyFactory.CreateCacheKeyAsync(autoParentKey, memberName, cacheKeyModifier);

            var removeResult = await _cacheProvider.RemoveAsync(cacheKey);

            _logger.LogInformation(removeResult
                ? $"REMOVED an object from the cache for {cacheKey}"
                : $"DID NOT remove item from the cache for {cacheKey}");
        }

        /// <summary>
        /// Removes items from cache that match the key at least partially
        /// Does nothing if all 3 parameters are null.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="cacheKeyModifier">The cache Key Modifier.</param>
        /// <param name="autoParentKey">The reflected caller path</param>
        public async Task RemovePartialMatchesAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null)
        {
            if (memberName == null && cacheKeyModifier == null && autoParentKey == null)
            {
                return;
            }

            var cacheKey = await _cacheKeyFactory.CreateCacheKeyAsync(autoParentKey, memberName, cacheKeyModifier);

            var removeResult = await _cacheProvider.RemovePartialMatchesAsync(cacheKey);

            _logger.LogInformation(removeResult
                ? $"REMOVED all partial matches from the cache for {cacheKey}"
                : $"DID NOT remove any items from the cache for {cacheKey}");
        }

        #endregion
    }
}
