namespace Vidmantas.Caching.Provider.Redis.Autofac
{
    #region Usings

    using global::Autofac;
    using Interfaces;

    #endregion

    public static class AutofacConfiguration
    {
        #region Public Methods

        public static void UseNetMemoryProvider(this ContainerBuilder containerBuilder, int cacheExpiry = 5)
        {
            containerBuilder.RegisterType<RedisCacheProvider>().As<ICacheProvider>().SingleInstance();
        }

        #endregion
    }
}
