namespace Vidmantas.Caching.DependencyInjection.Autofac
{
    #region Usings

    using Core;
    using Default;
    using global::Autofac;
    using Interfaces;
    using Microsoft.Extensions.Logging;

    #endregion

    public static class AutofacConfiguration
    {
        #region Public Methods

        /// <summary>
        /// Registers the caching services.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterCachingServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
            containerBuilder.RegisterType<DefaultCacheKeyFactory>().As<ICacheKeyFactory>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultCacheObjectSerializer>().As<ICacheObjectSerializer>().InstancePerDependency();
            containerBuilder.RegisterType<LoggerFactory>().As<ILoggerFactory>().InstancePerDependency();
            containerBuilder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).InstancePerDependency();

            return containerBuilder;
        }

        #endregion
    }
}
