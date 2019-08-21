// ReSharper disable ExplicitCallerInfoArgument
namespace Vidmantas.Caching.Core
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Helpers;
    using Interfaces;

    #endregion

    public sealed class CacheBustingService : ICacheBustingService
    {
        #region Fields

        private readonly ICacheService _cache;
        private readonly Dictionary<string, MethodInfo[]> _cachedRemovableMethods = new Dictionary<string, MethodInfo[]>();

        #endregion

        #region Constructors

        public CacheBustingService(ICacheService cache)
        {
            _cache = cache;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides a generic way of busting cache entries
        /// </summary>
        /// <typeparam name="T">
        /// The type
        /// </typeparam>
        /// <param name="cacheKeyModifier">
        /// The optional cache key modifier to identify the entry in the cache
        /// </param>
        public async Task BustAsync<T>(object cacheKeyModifier = null) where T : class
        {
            var type = typeof(T);

            await BustAsync(type, cacheKeyModifier);
        }

        /// <summary>
        /// Provides a generic way of busting cache entries
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="cacheKeyModifier">
        /// The optional cache key modifier to identify the entry in the cache
        /// </param>
        public async Task BustAsync(Type type, object cacheKeyModifier = null)
        {
            if (!_cachedRemovableMethods.TryGetValue(type.Name, out MethodInfo[] methodInfoArray))
            {
                // Public or private methods
                var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

                var methods = type.GetMethods(bindingFlags).Where(x => Attribute.IsDefined(x, typeof(CacheRemovableAttribute))).ToArray();
                _cachedRemovableMethods.Add(type.Name, methods);

                methodInfoArray = methods;
            }

            // Items that cannot be busted with an exact modifier as it can never be accurately recreated in all areas
            await methodInfoArray
                .Where(x => !x.GetCustomAttribute<CacheRemovableAttribute>().WithCacheKeyModifier)
                .ForEachAsync(async method =>
                {
                    await _cache.RemovePartialMatchesAsync(method.Name, null, type.Name);
                });

            if (cacheKeyModifier is null)
            {
                return;
            }

            // Exact items based on a exact modifier
            await methodInfoArray
                .Where(x => x.GetCustomAttribute<CacheRemovableAttribute>().WithCacheKeyModifier)
                .ForEachAsync(async method =>
                {
                    await _cache.RemoveAsync(method.Name, cacheKeyModifier, type.Name);
                });
        }

        #endregion
    }
}
