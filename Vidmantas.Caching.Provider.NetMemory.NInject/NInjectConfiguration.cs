namespace Vidmantas.Caching.Provider.NetMemory.NInject
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using Interfaces;
    using NetMemory;
    using Ninject;

    public static class NInjectConfiguration
    {
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
    }
}
