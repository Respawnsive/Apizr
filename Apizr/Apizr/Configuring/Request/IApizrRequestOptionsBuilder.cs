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
        TApizrOptionsBuilder WithCancellation(CancellationToken cancellationToken);

        TApizrOptionsBuilder WithClearing(bool clearCache);
    }

    public interface IApizrRequestOptionsBuilder :
        IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        internal IApizrRequestOptions ApizrOptions { get; }
    }
}
