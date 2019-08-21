namespace Vidmantas.Caching.DependencyInjection.NInject
{
    using System;
    using Core;
    using Default;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Ninject;

    public static class NInjectConfiguration
    {
        /// <summary>
        /// Registers the caching services.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <returns></returns>
        public static IKernel RegisterCachingServices(this IKernel kernel)
        {
            var defaultLoggerFactory = new LoggerFactory();

            kernel.Bind<ICacheService>().To<CacheService>().InSingletonScope();
            kernel.Bind<ICacheKeyFactory>().To<DefaultCacheKeyFactory>().InTransientScope();
            kernel.Bind<ICacheObjectSerializer>().To<DefaultCacheObjectSerializer>().InTransientScope();
            kernel.Bind(typeof(ILogger<>)).ToMethod(ctx =>
                {
                    var loggerCreateMethod = typeof(LoggerFactoryExtensions).GetMethod("CreateLogger", new[] { typeof(ILoggerFactory) });
                    var genericMethod = loggerCreateMethod?.MakeGenericMethod(ctx.GenericArguments ?? throw new InvalidOperationException());
                    var loggerInstance = genericMethod?.Invoke(null, new object[] { defaultLoggerFactory });
                    return loggerInstance;
                })
                .InTransientScope();

            return kernel;
        }
    }
}
