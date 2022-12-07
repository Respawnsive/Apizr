using System;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptionsBuilder<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrRequestOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
        where TApizrOptions : IApizrRequestOptions
        where TApizrOptionsBuilder : IApizrRequestOptionsBuilder<TApizrOptions, TApizrOptionsBuilder>
    {
        TApizrOptionsBuilder WithContext(Context context, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge);

        TApizrOptionsBuilder WithCancellation(CancellationToken cancellationToken);

        TApizrOptionsBuilder WithCacheClearing(bool clearCache);
    }

    public interface IApizrRequestOptionsBuilder :
        IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        internal IApizrRequestOptions ApizrOptions { get; }
    }
}
