namespace Vidmantas.Caching.Provider.Redis
{
    public class RedisCacheProviderOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cache connection string.
        /// </summary>
        /// <value>
        /// The cache connection string.
        /// </value>
        public string CacheConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the expiry minutes.
        /// </summary>
        /// <value>
        /// The expiry minutes.
        /// </value>
        public double ExpiryMinutes { get; set; } = 60;

        #endregion
    }
}
