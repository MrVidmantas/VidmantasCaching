namespace Vidmantas.Caching.Interfaces
{
    using System.Threading.Tasks;

    public interface ICacheKeyFactory
    {
        #region Public Methods

        Task<ICacheKey> CreateCacheKeyAsync(string parentKey, string valueKey, object valueKeyModifier);

        #endregion
    }
}
