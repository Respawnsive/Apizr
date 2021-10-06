using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;

namespace Apizr
{
    public class ApizrExtendedConfiguration : ApizrConfigurationBase, IApizrExtendedConfiguration
    {
        public ApizrExtendedConfiguration()
        {
            CacheHandlerFactory = _ => new VoidCacheHandler();
        }

        public Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; set; }
    }
}
