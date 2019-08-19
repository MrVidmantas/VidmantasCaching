namespace Vidmantas.Caching.Interfaces
{
    using System.Threading.Tasks;

    public interface ICacheKeyFactory
    {
        #region Public Methods

        /// <summary>
        /// Creates the cache key.
        /// </summary>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="valueKey">The value key.</param>
        /// <param name="valueKeyModifier">The value key modifier.</param>
        /// <returns><see cref="Task{ICacheKey}"/></returns>
        Task<ICacheKey> CreateCacheKeyAsync(string parentKey, string valueKey, object valueKeyModifier);

        #endregion
    }
}
