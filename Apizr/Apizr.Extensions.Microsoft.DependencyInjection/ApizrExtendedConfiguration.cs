using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;

namespace Apizr
{
    public class ApizrExtendedConfiguration : ApizrCommonOptionsBase, IApizrExtendedConfiguration
    {
        public ApizrExtendedConfiguration()
        {
            CacheHandlerFactory = _ => new VoidCacheHandler();
        }

        public Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; set; }
    }
}
