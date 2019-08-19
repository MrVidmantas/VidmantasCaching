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

        private readonly ILogger<CacheService> _logger;

        private readonly IPrimaryCacheProvider _primaryProvider;

        private readonly ISecondaryCacheProvider _secondaryProvider;

        #endregion

        #region Constructors

        public CacheService(IPrimaryCacheProvider primaryProvider, ISecondaryCacheProvider secondaryProvider, ILogger<CacheService> logger, ICacheKeyFactory cacheKeyFactory)
        {
            _primaryProvider = primaryProvider;
            _secondaryProvider = secondaryProvider;
            _logger = logger;
            _cacheKeyFactory = cacheKeyFactory;
        }

        public CacheService(IPrimaryCacheProvider primaryProvider, ILogger<CacheService> logger, ICacheKeyFactory cacheKeyFactory)
        {
            _primaryProvider = primaryProvider;
            _logger = logger;
            _cacheKeyFactory = cacheKeyFactory;
        }

        #endregion

        #region Properties

        private bool IsSecondaryProviderInUse => _secondaryProvider != null;

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

            (T primaryCachedValue, string primaryCacheKey) = await _primaryProvider.GetAsync<T>(cacheKey);

            if (primaryCachedValue != null)
            {
                _logger.LogInformation($"Fetched object from the primary cache for {primaryCacheKey}");

                return primaryCachedValue;
            }

            if (IsSecondaryProviderInUse)
            {
                (T secondaryCachedValue, string secondaryCacheKey) = await _secondaryProvider.GetAsync<T>(cacheKey);

                if (secondaryCachedValue != null)
                {
                    _logger.LogInformation($"Fetched object from the secondary cache for {secondaryCacheKey}");

                    return secondaryCachedValue;
                }
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

            var primaryAddResult = await _primaryProvider.AddAsync(cacheKey, result);

            if (primaryAddResult)
            {
                _logger.LogInformation($"Added an object to the primary cache for {cacheKey}");
            }
            else
            {
                _logger.LogWarning("There was an issue adding the object to the primary cache.");
            }

            if (IsSecondaryProviderInUse)
            {
                var secondaryAddResult = await _secondaryProvider.AddAsync(cacheKey, result);

                if (secondaryAddResult)
                {
                    _logger.LogInformation($"Added an object to the secondary cache for {cacheKey}");
                }
                else
                {
                    _logger.LogWarning("There was an issue adding the object to the secondary cache.");
                }
            }

            return result;
        }

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        public async Task<bool> FlushAsync()
        {
            var primaryResult = await _primaryProvider.FlushAsync();

            if (!IsSecondaryProviderInUse)
            {
                return primaryResult;
            }

            var secondaryResult = await _secondaryProvider.FlushAsync();

            return primaryResult && secondaryResult;
        }

        /// <summary>
        /// Removes the an item from cache by method name
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="cacheKeyModifier">The cache Key Modifier.</param>
        /// <param name="autoParentKey">The reflected caller path</param>
        public async Task RemoveAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null)
        {
            var cacheKey = await _cacheKeyFactory.CreateCacheKeyAsync(autoParentKey, memberName, cacheKeyModifier);

            var primaryRemoveResult = await _primaryProvider.RemoveAsync(cacheKey);

            _logger.LogInformation(primaryRemoveResult
                ? $"REMOVED an object from the primary cache for {cacheKey}"
                : $"DID NOT remove item from the primary cache for {cacheKey}");

            if (IsSecondaryProviderInUse)
            {
                var secondaryRemoveResult = await _secondaryProvider.RemoveAsync(cacheKey);

                _logger.LogInformation(secondaryRemoveResult
                    ? $"REMOVED an object from the primary cache for {cacheKey}"
                    : $"DID NOT remove item from the primary cache for {cacheKey}");
            }
        }

        #endregion
    }
}
