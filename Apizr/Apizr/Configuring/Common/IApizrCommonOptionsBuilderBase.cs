using System;
using Apizr.Caching;
using Apizr.Configuring.Shared;
using Apizr.Connecting;
using Apizr.Mapping;
using Refit;

namespace Apizr.Configuring.Common
{
    public interface IApizrCommonOptionsBuilderBase
    {
    }

    public interface IApizrCommonOptionsBuilderBase<out TApizrCommonOptions, out TApizrCommonOptionsBuilder> : IApizrCommonOptionsBuilderBase, 
        IApizrSharedOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>
    where TApizrCommonOptions : IApizrCommonOptionsBase
    where TApizrCommonOptionsBuilder : IApizrCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>
    {
        /// <summary>
        /// Provide some Refit specific settings
        /// </summary>
        /// <param name="refitSettings">A <see cref="RefitSettings"/> instance</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithRefitSettings(RefitSettings refitSettings);

        /// <summary>
        /// Provide a connectivity handler to check connectivity before sending a request
        /// </summary>
        /// <param name="connectivityHandler">An <see cref="IConnectivityHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler);

        /// <summary>
        /// Provide a function to invoke while checking connectivity
        /// </summary>
        /// <param name="connectivityCheckingFunction">A function to invoke while checking connectivity</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction);

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandler">An <see cref="ICacheHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler);

        /// <summary>
        /// Provide a mapping handler to map entities
        /// </summary>
        /// <param name="mappingHandler">An <see cref="IMappingHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        TApizrCommonOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler);
    }
}
