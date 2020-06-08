using System;
using Apizr.Caching;
using Apizr.Connecting;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApizr<TWebApi>(this IServiceCollection services,
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Type apizrManagerType, Action<IApizrOptionsBuilder> optionsBuilder = null)
        {
            //if (!typeof(IConnectivityProvider).IsAssignableFrom(connectivityProviderType))
            //    throw new ArgumentException(
            //        $"Your connectivity provider class must inherit from {nameof(IConnectivityProvider)} interface or derived");

            //if (!typeof(ICacheProvider).IsAssignableFrom(cacheProviderType))
            //    throw new ArgumentException(
            //        $"Your cache provider class must inherit from {nameof(ICacheProvider)} interface or derived");

            if (!typeof(IApizrManager<>).MakeGenericType(webApiType).IsAssignableFrom(apizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");

            //todo: init here

            return services;
        }
    }
}
