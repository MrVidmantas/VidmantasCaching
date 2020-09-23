namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System.Threading.Tasks;

    #endregion

    public interface ICacheProvider
    {
        #region Public Methods

        Task<bool> AddAsync(ICacheKey cacheKey, object value);

        Task<bool> ExistsAsync(ICacheKey cacheKey);

        Task<bool> FlushAsync();

        Task<T> GetAsync<T>(ICacheKey cacheKey) where T : class;

        Task<bool> RemoveAsync(ICacheKey cacheKey);

        Task<bool> RemovePartialMatchesAsync(ICacheKey cacheKey);

        #endregion
    }
}
