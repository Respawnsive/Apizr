using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;

namespace Apizr
{
    public interface IApizrConfigurationBuilderBase : IApizrOptionsBuilderBase
    {
    }

    public interface IApizrConfigurationBuilderBase<out TApizrConfiguration, out TApizrConfigurationBuilder> : IApizrConfigurationBuilderBase
    where TApizrConfiguration : IApizrConfigurationBase
    where TApizrConfigurationBuilder : IApizrConfigurationBuilderBase<TApizrConfiguration, TApizrConfigurationBuilder>
    {
        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandler">An <see cref="ICacheHandler"/> mapping implementation instance</param>
        /// <returns></returns>
        TApizrConfigurationBuilder WithCacheHandler(ICacheHandler cacheHandler);
    }
}
