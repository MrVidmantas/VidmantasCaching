namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface ICacheBustingService
    {
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
        Task BustAsync<T>(object cacheKeyModifier = null) where T : class;

        /// <summary>
        /// Provides a generic way of busting cache entries
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="cacheKeyModifier">
        /// The optional cache key modifier to identify the entry in the cache
        /// </param>
        Task BustAsync(Type type, object cacheKeyModifier = null);

        #endregion
    }
}
