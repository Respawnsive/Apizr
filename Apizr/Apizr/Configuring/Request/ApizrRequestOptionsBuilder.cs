using System;
using System.Linq;
using System.Threading;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request;

public class ApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder
{
    protected readonly ApizrRequestOptions Options;

    public ApizrRequestOptionsBuilder(ApizrRequestOptions options)
    {
        Options = options;
    }

    /// <inheritdoc />
    IApizrRequestOptions IApizrRequestOptionsBuilder.ApizrOptions => Options;

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
        HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels)
    {
        if(Options.HttpTracerMode == HttpTracerMode.Unspecified)
            Options.HttpTracerMode = httpTracerMode;
        if(Options.TrafficVerbosity == HttpMessageParts.Unspecified)
            Options.TrafficVerbosity = trafficVerbosity;
        if(Options.LogLevels?.Length is null or 0)
            Options.LogLevels = logLevels;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithContext(Context context, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
    {
        switch (strategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                Options.Context ??= context;
                break;
            case ApizrDuplicateStrategy.Replace:
                Options.Context = context;
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                if (Options.Context == null)
                {
                    Options.Context = context;
                }
                else
                {
                    var operationKey = !string.IsNullOrWhiteSpace(context.OperationKey)
                        ? context.OperationKey
                        : Options.Context.OperationKey;

                    Options.Context = new Context(operationKey,
                        Options.Context.Concat(context.ToList()).ToDictionary(x => x.Key, x => x.Value));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithCancellation(CancellationToken cancellationToken)
    {
        Options.CancellationToken = cancellationToken;

        return this;
    }
    
    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithCacheClearing(bool clearCache)
    {
        Options.ClearCache = clearCache;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching(Action<ApizrException> onException,
        bool letThrowOnExceptionWithEmptyCache = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
    {
        Options.OnException = onException;
        Options.LetThrowOnExceptionWithEmptyCache = letThrowOnExceptionWithEmptyCache;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithHandlerParameter(string key, object value)
    {
        Options.HandlersParameters[key] = value;

        return this;
    }
}