namespace Vidmantas.Caching.Provider.Redis
{
    public class RedisCacheProviderOptions
    {
        #region Properties

        public string CacheConnectionString { get; set; }

        public double ExpiryMinutes { get; set; } = 60;

        #endregion
    }
}
