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
        Task<T> FetchOrAddAsync<T>(Func<Task<T>> itemToAddFunc, object cacheKeyModifier = null, bool useCache = true, [CallerFilePath]string autoParentKey = null, [CallerMemberName]string autoMemberName = null)
            where T : class;

        /// <summary>
        /// Expires all items in the cache
        /// </summary>
        /// <returns>True if the cache was successfully flushed, false otherwise</returns>
        Task<bool> FlushAsync();

        /// <summary>
        /// Removes the an item from cache
        /// Could remove partial matches based on the ICacheProvider implementation used.
        /// Does nothing if all 3 parameters are null.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="cacheKeyModifier">The cache Key Modifier.</param>
        /// <param name="autoParentKey">The reflected caller path</param>
        Task RemoveAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null);

        /// <summary>
        /// Removes items from cache that match the key at least partially
        /// Does nothing if all 3 parameters are null.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="cacheKeyModifier">The cache Key Modifier.</param>
        /// <param name="autoParentKey">The reflected caller path</param>
        Task RemovePartialMatchesAsync(string memberName = null, object cacheKeyModifier = null, [CallerFilePath]string autoParentKey = null);

        #endregion
    }
}
