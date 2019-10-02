namespace Vidmantas.Caching.Provider.NetMemory.NInject
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using Interfaces;
    using Ninject;

    #endregion

    public static class NInjectConfiguration
    {
        #region Public Methods

        /// <summary>
        /// Uses the net memory provider.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="cacheExpiry">The cache expiry.</param>
        public static void UseNetMemoryProvider(this IKernel kernel, int cacheExpiry = 5)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = TimeSpan.FromMinutes(cacheExpiry),
            };

            kernel.Bind<ICacheProvider>().To<NetMemoryCacheProvider>().InSingletonScope();
            kernel.Bind<ObjectCache>().To<MemoryCache>().InSingletonScope()
                .WithConstructorArgument("NetMemoryCache")
                .WithConstructorArgument(new NameValueCollection());
            kernel.Bind<CacheItemPolicy>().ToConstant(cacheItemPolicy).InSingletonScope();
        }

        #endregion
    }
}
