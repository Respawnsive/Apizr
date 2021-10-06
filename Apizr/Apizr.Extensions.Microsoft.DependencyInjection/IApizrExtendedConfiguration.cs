using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Caching;

namespace Apizr
{
    public interface IApizrExtendedConfiguration : IApizrConfigurationBase
    {

        /// <summary>
        /// Cache handler factory
        /// </summary>
        Func<IServiceProvider, ICacheHandler> CacheHandlerFactory { get; }
    }
}
