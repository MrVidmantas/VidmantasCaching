namespace Vidmantas.Caching.Interfaces
{
    public interface ICachingResult
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        string CacheKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICachingResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        bool Success { get; set; }

        #endregion
    }
}
