namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface ICacheBustingService
    {
        #region Public Methods

        Task BustAsync<T>(object cacheKeyModifier = null) where T : class;

        Task BustAsync(Type type, object cacheKeyModifier = null);

        #endregion
    }
}
