namespace Vidmantas.Caching.Provider.Redis
{
    #region Usings

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;

    #endregion

    public class RedisCacheProvider : ICacheProvider
    {
        #region Fields

        private readonly string _connectionString;
        private readonly TimeSpan _dataCacheTime;
        private readonly ILogger<RedisCacheProvider> _logger;
        private readonly ConnectionMultiplexer _redisConnections;
        private readonly ICacheObjectSerializer _objectSerializer;

        public RedisCacheProvider(ILogger<RedisCacheProvider> logger, ICacheObjectSerializer objectSerializer)
        {
            _logger = logger;
            _objectSerializer = objectSerializer;
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
        public async Task<bool> AddAsync(ICacheKey cacheKey, object value)
        {
            var successFlag = false;

            try
            {
                var serializedObject = await _objectSerializer.ToByteArrayAsync(value);

                var db = _redisConnections.GetDatabase();

                successFlag = await db.HashSetAsync(cacheKey.ParentKey, cacheKey.ValueKey, serializedObject);

                await db.KeyExpireAsync(cacheKey.ParentKey, _dataCacheTime);
            }
            catch (RedisException rex)
            {
                _logger.LogError($"Problem adding {cacheKey} in cache.", rex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error has occurred.", ex);
            }

            return successFlag;
        }

        /// <summary>
        /// Checks if the key exists in the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        public async Task<bool> ExistsAsync(ICacheKey cacheKey)
        {
            var successFlag = false;

            try
            {
                var db = _redisConnections.GetDatabase();

                successFlag = await db.HashExistsAsync(cacheKey.ParentKey, cacheKey.ValueKey);

                await db.KeyExpireAsync(cacheKey.ParentKey, _dataCacheTime);
            }
            catch (RedisException rex)
            {
                _logger.LogError($"Problem checking {cacheKey} in cache.", rex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error has occurred.", ex);
            }

            return successFlag;
        }

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        public async Task<bool> FlushAsync()
        {
            try
            {
                var connectionOptions = ConfigurationOptions.Parse(_connectionString);
                connectionOptions.AllowAdmin = true;

                using (var adminConnections = ConnectionMultiplexer.Connect(connectionOptions))
                {
                    foreach (var endpoint in adminConnections.GetEndPoints())
                    {
                        var server = adminConnections.GetServer(endpoint);
                        await server.FlushAllDatabasesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error whilst flushing Redis server database", ex);
                throw;
            }

            return true;
        }

        /// <summary>
        /// Gets an item from the cache with the specified key
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        /// Type object if it exists in the cache, null otherwise. Also returns the underlying cache key used.
        /// </returns>
        public async Task<T> GetAsync<T>(ICacheKey cacheKey) where T : class
        {
            try
            {
                var db = _redisConnections.GetDatabase();

                var redisValue = await db.HashGetAsync(cacheKey.ParentKey, cacheKey.ValueKey);

                if (redisValue.HasValue)
                {
                    return await _objectSerializer.FromByteArrayAsync<T>(redisValue);
                }
            }
            catch (RedisException rex)
            {
                _logger.LogError($"Problem getting {cacheKey} in cache.", rex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error has occurred.", ex);
            }

            return default;
        }

        /// <summary>
        /// Removes an item with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        public async Task<bool> RemoveAsync(ICacheKey cacheKey)
        {
            var successFlag = false;

            try
            {
                var db = _redisConnections.GetDatabase();

                successFlag = await db.HashDeleteAsync(cacheKey.ParentKey, cacheKey.ValueKey);
            }
            catch (RedisException rex)
            {
                _logger.LogError($"Problem removing {cacheKey} from cache.", rex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error has occurred.", ex);
            }

            return successFlag;
        }

        /// <summary>
        /// Removes items from cache that match the specified key at least partially
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        ///   <see cref="bool" />
        /// </returns>
        public async Task<bool> RemovePartialMatchesAsync(ICacheKey cacheKey)
        {
            var successFlag = false;

            try
            {
                var db = _redisConnections.GetDatabase();

                var matchingKeys = (await db.HashGetAllAsync(cacheKey.ParentKey)).Select(x => x.Name).ToArray();

                if (!string.IsNullOrEmpty(cacheKey.ValueKey))
                {
                    matchingKeys = matchingKeys.Where(x => x.StartsWith(cacheKey.ValueKey)).ToArray();
                }

                successFlag = await db.HashDeleteAsync(cacheKey.ParentKey, matchingKeys) > 0;
            }
            catch (RedisException rex)
            {
                _logger.LogError($"Problem removing {cacheKey} from cache.", rex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error has occurred.", ex);
            }

            return successFlag;
        }

        #endregion
    }
}
