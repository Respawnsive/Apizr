using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Common;

namespace Apizr
{
    public interface IApizrExtendedConfiguration : IApizrCommonOptionsBase
    {

        /// <summary>
        /// Cache handler factory
        /// </summary>
        Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; }
    }
}
