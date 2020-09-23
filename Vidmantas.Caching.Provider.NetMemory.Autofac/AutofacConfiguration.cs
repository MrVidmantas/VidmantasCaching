namespace Vidmantas.Caching.Provider.NetMemory.Autofac
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using global::Autofac;
    using global::Autofac.Core;
    using Interfaces;

    #endregion

    public static class AutofacConfiguration
    {
        #region Public Methods

        public static void UseNetMemoryProvider(this ContainerBuilder containerBuilder, int cacheExpiry = 5)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = TimeSpan.FromMinutes(cacheExpiry),
            };

            containerBuilder.RegisterType<NetMemoryCacheProvider>().As<ICacheProvider>().SingleInstance();
            containerBuilder.RegisterType<MemoryCache>().As<ObjectCache>().SingleInstance()
                .WithParameters(
                    new List<Parameter> {
                        new NamedParameter("name", "NetMemoryCache"),
                        new NamedParameter("config", new NameValueCollection())
                    });

            containerBuilder.RegisterInstance(cacheItemPolicy).As<CacheItemPolicy>().SingleInstance();
        }

        #endregion
    }
}
