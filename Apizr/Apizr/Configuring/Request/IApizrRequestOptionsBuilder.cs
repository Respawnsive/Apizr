using System;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// Builder options available at request level
    /// </summary>
    public interface IApizrRequestOptionsBuilder<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrRequestOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
        where TApizrOptions : IApizrRequestOptions
        where TApizrOptionsBuilder : IApizrRequestOptionsBuilder<TApizrOptions, TApizrOptionsBuilder>
    {
        /// <summary>
        /// Set the Polly Context
        /// </summary>
        /// <param name="context">The Polly Context to pass through it all</param>
        /// <param name="strategy">The duplicate strategy if there's another one already (default: Merge)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithContext(Context context, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge);

        /// <summary>
        /// Set the cancellation token
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithCancellation(CancellationToken cancellationToken);

        /// <summary>
        /// Tells if you want to clear the potential cached data before requesting
        /// </summary>
        /// <param name="clearCache">Clear cache or not</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithCacheClearing(bool clearCache);
    }
    
    /// <inheritdoc />
    public interface IApizrRequestOptionsBuilder :
        IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        internal IApizrRequestOptions ApizrOptions { get; }
    }
}
