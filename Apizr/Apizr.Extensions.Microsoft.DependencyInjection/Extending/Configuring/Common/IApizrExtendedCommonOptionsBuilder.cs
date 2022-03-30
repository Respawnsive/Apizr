using System;
using System.Linq.Expressions;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Shared;
using Apizr.Mapping;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    public interface IApizrExtendedCommonOptionsBuilder<out TApizrExtendedCommonOptions, out TApizrExtendedCommonOptionsBuilder> : IApizrExtendedCommonOptionsBuilderBase,
        IApizrGlobalCommonOptionsBuilderBase<TApizrExtendedCommonOptions, TApizrExtendedCommonOptionsBuilder>,
        IApizrExtendedSharedOptionsBuilder<TApizrExtendedCommonOptions, TApizrExtendedCommonOptionsBuilder>
        where TApizrExtendedCommonOptions : IApizrCommonOptionsBase
        where TApizrExtendedCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase<TApizrExtendedCommonOptions, TApizrExtendedCommonOptionsBuilder>
    {

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettingsFactory">A <see cref="RefitSettings"/> instance factory</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithRefitSettings(Func<IServiceProvider, RefitSettings> refitSettingsFactory);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <param name="connectivityHandlerFactory">A <see cref="IConnectivityHandler"/> mapping implementation factory</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithConnectivityHandler(Func<IServiceProvider, IConnectivityHandler> connectivityHandlerFactory);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <typeparam name="TConnectivityHandler">Your connectivity checking service</typeparam>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithConnectivityHandler<TConnectivityHandler>(Expression<Func<TConnectivityHandler, bool>> connectivityProperty);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <typeparam name="TConnectivityHandler">Your <see cref="IConnectivityHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithConnectivityHandler<TConnectivityHandler>() where TConnectivityHandler : class, IConnectivityHandler;

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <param name="connectivityHandlerType">Type of your <see cref="IConnectivityHandler"/> mapping implementation</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithConnectivityHandler(Type connectivityHandlerType);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerFactory">A <see cref="ICacheHandler"/> mapping implementation factory</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithCacheHandler(Func<IServiceProvider, ICacheHandler> cacheHandlerFactory);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <typeparam name="TCacheHandler">Your <see cref="ICacheHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithCacheHandler<TCacheHandler>() where TCacheHandler : class, ICacheHandler;

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerType">Type of your <see cref="ICacheHandler"/> mapping implementation</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithCacheHandler(Type cacheHandlerType);

        /// <summary>
        /// Provide a mapping handler to auto map entities during mediation
        /// </summary>
        /// <param name="mappingHandlerFactory">A <see cref="IMappingHandler"/> mapping implementation factory</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithMappingHandler(Func<IServiceProvider, IMappingHandler> mappingHandlerFactory);

        /// <summary>
        /// Provide a mapping handler to auto map entities during mediation
        /// </summary>
        /// <typeparam name="TMappingHandler">Your <see cref="IMappingHandler"/> mapping implementation</typeparam>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithMappingHandler<TMappingHandler>() where TMappingHandler : class, IMappingHandler;

        /// <summary>
        /// Provide a mapping handler to auto map entities during mediation
        /// </summary>
        /// <param name="mappingHandlerType">Type of your <see cref="IMappingHandler"/> mapping implementation</param>
        /// <returns></returns>
        TApizrExtendedCommonOptionsBuilder WithMappingHandler(Type mappingHandlerType);
    }

    public interface IApizrExtendedCommonOptionsBuilder : IApizrExtendedCommonOptionsBuilder<IApizrExtendedCommonOptions, IApizrExtendedCommonOptionsBuilder>
    { }
}
