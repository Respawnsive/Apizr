using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;

namespace Apizr
{
    public class ApizrConfigurationBuilder : IApizrConfigurationBuilder
    {
        protected readonly ApizrConfiguration Configuration;

        internal ApizrConfigurationBuilder(ApizrConfiguration apizrConfiguration)
        {
            Configuration = apizrConfiguration;
        }

        public IApizrConfiguration ApizrConfiguration => Configuration;
        
        public IApizrConfigurationBuilder WithCacheHandler(ICacheHandler cacheHandler)
            => WithCacheHandler(() => cacheHandler);

        public IApizrConfigurationBuilder WithCacheHandler(Func<ICacheHandler> cacheHandlerFactory)
        {
            Configuration.CacheHandlerFactory = cacheHandlerFactory;

            return this;
        }
    }
}
