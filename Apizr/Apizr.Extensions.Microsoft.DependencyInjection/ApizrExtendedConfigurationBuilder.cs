using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;

namespace Apizr
{
    public class ApizrExtendedConfigurationBuilder : IApizrExtendedConfigurationBuilder
    {
        protected readonly ApizrExtendedConfiguration Configuration;

        internal ApizrExtendedConfigurationBuilder(ApizrExtendedConfiguration apizrConfiguration)
        {
            Configuration = apizrConfiguration;
        }

        public IApizrExtendedConfiguration ApizrConfiguration => Configuration;

        public IApizrExtendedConfigurationBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(_ => cacheHandler);

        public IApizrExtendedConfigurationBuilder WithCacheHandler(Func<IServiceProvider, ICacheHandler> cacheHandlerFactory)
        {
            Configuration.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }
    }
}
