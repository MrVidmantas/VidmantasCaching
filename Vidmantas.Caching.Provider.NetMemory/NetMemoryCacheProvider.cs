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

        public Task<bool> AddAsync(ICacheKey cacheKey, object value, TimeSpan? expiryInMinutes = null)
        {
            var successFlag = _cache.Add(cacheKey.ToString(), value, _policy);

            return Task.FromResult(successFlag);
        }

        public Task<bool> ExistsAsync(ICacheKey cacheKey)
        {
            var keyExists = _cache.Any(x => x.Key == cacheKey.ToString());

            return Task.FromResult(keyExists);
        }

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

        public Task<T> GetAsync<T>(ICacheKey cacheKey) where T : class
        {
            var cachedValue = _cache.Get(cacheKey.ToString());

            try
            {
                return Task.FromResult((T)cachedValue);
            }
            catch (Exception)
            {
            }

            return Task.FromResult(default(T));
        }

        public Task<bool> RemoveAsync(ICacheKey cacheKey)
        {
            var removedObject = _cache.Remove(cacheKey.ToString());
            var successFlag = removedObject != null;

            return Task.FromResult(successFlag);
        }

        public Task<bool> RemovePartialMatchesAsync(ICacheKey cacheKey)
        {
            var successFlag = true;

            var matchingKeys = _cache
                .Where(x => x.Key.Contains(cacheKey.ToString()))
                .Select(x => x.Key)
                .ToList();

            if (!matchingKeys.Any())
            {
                successFlag = false;
            }

            try
            {
                foreach (var matchingCacheKey in matchingKeys)
                {
                    _cache.Remove(matchingCacheKey);
                }
            }
            catch (Exception)
            {
                successFlag = false;
            }

            return Task.FromResult(successFlag);
        }

        #endregion
    }
}
