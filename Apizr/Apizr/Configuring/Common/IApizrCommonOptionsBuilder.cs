using System;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Apizr.Connecting;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Refit;

namespace Apizr.Configuring.Common
{
    public interface IApizrCommonOptionsBuilder<out TApizrCommonOptions, out TApizrCommonOptionsBuilder> : IApizrCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>, 
        IApizrSharedOptionsBuilder<TApizrCommonOptions, TApizrCommonOptionsBuilder>
        where TApizrCommonOptions : IApizrCommonOptionsBase
        where TApizrCommonOptionsBuilder : IApizrCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>
    {
        /// <summary>
        /// Provide a policy registry
        /// </summary>
        /// <param name="policyRegistryFactory">A policy registry instance factory</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithPolicyRegistry(Func<IReadOnlyPolicyRegistry<string>> policyRegistryFactory);

        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettingsFactory">A <see cref="RefitSettings"/> instance factory</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithRefitSettings(Func<RefitSettings> refitSettingsFactory);

        /// <summary>
        /// Provide a connectivity handler
        /// </summary>
        /// <param name="connectivityHandlerFactory">An <see cref="IConnectivityHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithConnectivityHandler(Func<IConnectivityHandler> connectivityHandlerFactory);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerFactory">An <see cref="ICacheHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithCacheHandler(Func<ICacheHandler> cacheHandlerFactory);

        /// <summary>
        /// Provide a logger factory
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithLoggerFactory(Func<ILoggerFactory> loggerFactory);

        /// <summary>
        /// Provide a mapping handler to map entities
        /// </summary>
        /// <param name="mappingHandlerFactory">An <see cref="IMappingHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithMappingHandler(Func<IMappingHandler> mappingHandlerFactory);
    }

    public interface IApizrCommonOptionsBuilder : IApizrCommonOptionsBuilder<IApizrCommonOptions, IApizrCommonOptionsBuilder>
    {}
}
