using System;
using System.Linq;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request;

/// <inheritdoc cref="IApizrRequestOptionsBuilder" />
public class ApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder, IApizrGlobalSharedVoidOptionsBuilderBase
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
        Options.HttpTracerMode = httpTracerMode;
        Options.TrafficVerbosity = trafficVerbosity;
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
        switch (strategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                Options.OnException ??= onException;
                break;
            case ApizrDuplicateStrategy.Replace:
                Options.OnException = onException;
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                if (Options.OnException == null)
                {
                    Options.OnException = onException;
                }
                else
                {
                    Options.OnException += onException.Invoke;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }
        
        Options.LetThrowOnExceptionWithEmptyCache = letThrowOnExceptionWithEmptyCache;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
        bool letThrowOnExceptionWithEmptyCache = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(ex => onException.Invoke((ApizrException<TResult>)ex), letThrowOnExceptionWithEmptyCache,
            strategy);

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithHandlerParameter(string key, object value)
    {
        Options.HandlersParameters[key] = value;

        return this;
    }

    /// <inheritdoc />
    public void SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);
}