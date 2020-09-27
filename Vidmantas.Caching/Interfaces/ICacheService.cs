namespace Vidmantas.Caching.Interfaces
{
    #region Usings

    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    #endregion

    public interface ICacheService
    {
        #region Public Methods

        Task<T> FetchOrAddAsync<T>(Func<Task<T>> itemToAddFunc, object cacheKeyModifier = null, TimeSpan? expiryInMinutes = null, bool useCache = true, [CallerFilePath]string autoParentKey = null, [CallerMemberName]string autoMemberName = null)
            where T : class;

        Task<bool> FlushAsync();

        Task RemoveAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null);

        Task RemovePartialMatchesAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null);

        #endregion
    }
}
