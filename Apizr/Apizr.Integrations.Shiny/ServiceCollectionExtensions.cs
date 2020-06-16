﻿using System;
using System.Linq;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Apizr
{
    public static class ServiceCollectionExtensions
    {
        public static bool UseApizr<TWebApi>(this IServiceCollection services,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizr(services, typeof(TWebApi), typeof(ApizrManager<TWebApi>),
                optionsBuilder);
        public static bool UseApizr(this IServiceCollection services, Type webApiType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null) =>
            UseApizr(services, webApiType, typeof(ApizrManager<>).MakeGenericType(webApiType),
                optionsBuilder);

        public static bool UseApizr(
            this IServiceCollection services, Type webApiType, Type apizrManagerType,
            Action<IApizrExtendedOptionsBuilder> optionsBuilder = null)
        {
            services.AddApizr(webApiType, apizrManagerType, optionsBuilder);

            var isVoidConnectivityHandlerRegistered = services.Any(x => x.ImplementationType == typeof(VoidConnectivityHandler));
            if(isVoidConnectivityHandlerRegistered)
                services.Replace(new ServiceDescriptor(typeof(IConnectivityHandler), typeof(ShinyConnectivityHandler), ServiceLifetime.Singleton));

            var isVoidCacheHandlerRegistered = services.Any(x => x.ImplementationType == typeof(VoidCacheHandler));
            if (isVoidCacheHandlerRegistered)
                services.Replace(new ServiceDescriptor(typeof(ICacheHandler), typeof(ShinyCacheHandler), ServiceLifetime.Singleton));

            var isDefaultLogHandlerRegistered = services.Any(x => x.ImplementationType == typeof(DefaultLogHandler));
            if (isDefaultLogHandlerRegistered)
                services.Replace(new ServiceDescriptor(typeof(ILogHandler), typeof(ShinyLogHandler), ServiceLifetime.Singleton));

            return true;
        }
    }
}
