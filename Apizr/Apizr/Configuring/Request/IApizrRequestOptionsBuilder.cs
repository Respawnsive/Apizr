using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Apizr.Configuring.Shared;
using Polly;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptions ApizrOptions { get; }

        TApizrRequestOptionsBuilder WithContext(Context context);

        TApizrRequestOptionsBuilder WithCancellationToken(CancellationToken cancellationToken);

        TApizrRequestOptionsBuilder WithCacheCleared(bool clearCache);

        TApizrRequestOptionsBuilder WithExceptionCatcher(Action<Exception> onException);
    }

    public interface IApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {

    }
}
