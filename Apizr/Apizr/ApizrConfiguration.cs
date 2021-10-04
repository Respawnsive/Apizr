using System;
using Apizr.Caching;

namespace Apizr
{
    public class ApizrConfiguration : ApizrConfigurationBase, IApizrConfiguration
    {
        public ApizrConfiguration()
        {
            CacheHandlerFactory = () => new VoidCacheHandler();
        }

        public Func<ICacheHandler> CacheHandlerFactory { get; set; }
    }
}
