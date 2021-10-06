using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;

namespace Apizr
{
    public interface IApizrExtendedConfigurationBuilder : IApizrConfigurationBuilderBase<IApizrExtendedConfiguration, IApizrExtendedConfigurationBuilder>
    {

        /// <summary>
        /// Provide a cache handler to cache data
        /// </summary>
        /// <param name="cacheHandlerFactory">An <see cref="ICacheHandler"/> mapping implementation instance factory</param>
        /// <returns></returns>
        IApizrExtendedConfigurationBuilder WithCacheHandler(Func<IServiceProvider, ICacheHandler> cacheHandlerFactory);
    }
}
