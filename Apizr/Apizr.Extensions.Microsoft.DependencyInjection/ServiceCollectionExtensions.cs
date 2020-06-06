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
            AddApizr(services, typeof(TWebApi), typeof(VoidConnectivityProvider), typeof(VoidCacheProvider),
                typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        public static IServiceCollection AddApizr<TWebApi, TConnectivityProvider>(this IServiceCollection services,
            Action<IApizrOptionsBuilder> optionsBuilder = null)
            where TConnectivityProvider : IConnectivityProvider =>
            AddApizr(services, typeof(TWebApi), typeof(TConnectivityProvider), typeof(VoidCacheProvider),
                typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        public static IServiceCollection AddApizr<TWebApi, TConnectivityProvider, TCacheProvider>(
            this IServiceCollection services, Action<IApizrOptionsBuilder> optionsBuilder = null)
            where TConnectivityProvider : IConnectivityProvider
            where TCacheProvider : ICacheProvider =>
            AddApizr(services, typeof(TWebApi), typeof(TConnectivityProvider), typeof(TCacheProvider),
                typeof(ApizrManager<TWebApi>),
                optionsBuilder);

        public static IServiceCollection AddApizr<TWebApi, TConnectivityProvider, TCacheProvider, TApizrManager>(
            this IServiceCollection services, Action<IApizrOptionsBuilder> optionsBuilder = null)
            where TConnectivityProvider : IConnectivityProvider
            where TCacheProvider : ICacheProvider
            where TApizrManager : IApizrManager<TWebApi> =>
            AddApizr(services, typeof(TWebApi), typeof(TConnectivityProvider), typeof(TCacheProvider),
                typeof(TApizrManager),
                optionsBuilder);

        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, webApiType, typeof(VoidConnectivityProvider), typeof(VoidCacheProvider),
                typeof(ApizrManager<>).MakeGenericType(webApiType), optionsBuilder);

        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Type connectivityProviderType, Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, webApiType, connectivityProviderType, typeof(VoidCacheProvider),
                typeof(ApizrManager<>).MakeGenericType(webApiType), optionsBuilder);

        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Type connectivityProviderType, Type cacheProviderType,
            Action<IApizrOptionsBuilder> optionsBuilder = null) =>
            AddApizr(services, webApiType, connectivityProviderType, cacheProviderType,
                typeof(ApizrManager<>).MakeGenericType(webApiType), optionsBuilder);

        public static IServiceCollection AddApizr(
            this IServiceCollection services, Type webApiType, Type connectivityProviderType, Type cacheProviderType, Type apizrManagerType, Action<IApizrOptionsBuilder> optionsBuilder = null)
        {
            if (!typeof(IConnectivityProvider).IsAssignableFrom(connectivityProviderType))
                throw new ArgumentException(
                    $"Your connectivity provider class must inherit from {nameof(IConnectivityProvider)} interface or derived");

            if (!typeof(ICacheProvider).IsAssignableFrom(cacheProviderType))
                throw new ArgumentException(
                    $"Your cache provider class must inherit from {nameof(ICacheProvider)} interface or derived");

            if (!typeof(IApizrManager<>).MakeGenericType(webApiType).IsAssignableFrom(apizrManagerType))
                throw new ArgumentException(
                    $"Your Apizr manager class must inherit from IApizrManager generic interface or derived");

            //todo: init here

            return services;
        }
    }
}
