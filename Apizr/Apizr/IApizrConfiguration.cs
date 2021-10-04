using System;
using Apizr.Caching;

namespace Apizr
{
    public interface IApizrConfiguration : IApizrConfigurationBase
    {
        /// <summary>
        /// Cache handler factory
        /// </summary>
        Func<ICacheHandler> CacheHandlerFactory { get; }
    }
}
