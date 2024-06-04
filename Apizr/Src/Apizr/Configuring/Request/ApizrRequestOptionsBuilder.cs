using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request;

/// <inheritdoc cref="IApizrRequestOptionsBuilder" />
public class ApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder, IApizrInternalOptionsBuilder
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
    public IApizrRequestOptionsBuilder WithHeaders(IList<string> headers, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
    {
        switch (strategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                Options.Headers ??= headers;
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                if (Options.Headers.Count > 0)
                {
                    headers?.ToList().ForEach(header => Options.Headers.Add(header));
                }
                else
                {
                    Options.Headers = headers;
                }
                break;
            case ApizrDuplicateStrategy.Replace:
                Options.Headers = headers;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithOperationTimeout(TimeSpan timeout)
    {
        Options.OperationTimeout = timeout;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithRequestTimeout(TimeSpan timeout)
    {
        Options.RequestTimeout = timeout;

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
    
    void IApizrInternalOptionsBuilder.SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue valueFactory)
    {
        ((IApizrGlobalSharedOptionsBase)Options).ResiliencePropertiesFactories[key.Key] = () => valueFactory;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
    {
        var options = Options as IApizrGlobalSharedOptionsBase;
        if (options.ContextOptionsBuilder == null)
        {
            options.ContextOptionsBuilder = contextOptionsBuilder;
        }
        else
        {
            options.ContextOptionsBuilder += contextOptionsBuilder.Invoke;
        }

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
    {
        var sensitiveHeaders = new HashSet<string>(redactedLoggedHeaderNames, StringComparer.OrdinalIgnoreCase);

        return WithLoggedHeadersRedactionRule(header => sensitiveHeaders.Contains(header), strategy);
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithLoggedHeadersRedactionRule(Func<string, bool> shouldRedactHeaderValue,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
    {
        switch (strategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                Options.ShouldRedactHeaderValue ??= shouldRedactHeaderValue;
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                if (Options.ShouldRedactHeaderValue == null)
                {
                    Options.ShouldRedactHeaderValue = shouldRedactHeaderValue;
                }
                else
                {
                    var previous = Options.ShouldRedactHeaderValue;
                    Options.ShouldRedactHeaderValue = header => previous(header) || shouldRedactHeaderValue(header);
                }

                break;
            case ApizrDuplicateStrategy.Replace:
                Options.ShouldRedactHeaderValue = shouldRedactHeaderValue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
    {
        switch (strategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                if (Options.ResiliencePipelineKeys.Count == 0)
                    Options.ResiliencePipelineKeys[ApizrConfigurationSource.RequestOptions] = resiliencePipelineKeys;
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                if (Options.ResiliencePipelineKeys.TryGetValue(ApizrConfigurationSource.RequestOptions, out var keys))
                {
                    Options.ResiliencePipelineKeys[ApizrConfigurationSource.RequestOptions] = keys.Union(resiliencePipelineKeys).ToArray();
                }
                else
                {
                    Options.ResiliencePipelineKeys[ApizrConfigurationSource.RequestOptions] = resiliencePipelineKeys;
                }
                break;
            case ApizrDuplicateStrategy.Replace:
                Options.ResiliencePipelineKeys.Clear();
                Options.ResiliencePipelineKeys[ApizrConfigurationSource.RequestOptions] = resiliencePipelineKeys;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }

        return this;
    }

    #region Internals

    /// <inheritdoc />
    IApizrRequestOptionsBuilder IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>.WithHeaders(IList<string> headers, ApizrRegistrationMode mode)
    {
        if (mode == ApizrRegistrationMode.Set)
        {
            // Set headers right the way
            if (Options.Headers.Count > 0)
            {
                headers?.ToList().ForEach(header => Options.Headers.Add(header));
            }
            else
            {
                Options.Headers = headers;
            } 
        }
        else
        {
            // Store headers for further attribute key match use
            var headersStore = ((IApizrRequestOptions) Options).HeadersStore;
            headers?.ToList().ForEach(header => headersStore.Add(header));
        }

        return this;
    }

    /// <inheritdoc />
    IApizrRequestOptionsBuilder IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>.WithOriginalExpression(Expression originalExpression)
    {
        ((IApizrRequestOptions)Options).OriginalExpression = originalExpression;

        return this;
    }

    /// <inheritdoc />
    IApizrRequestOptionsBuilder IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>.WithResilienceContextOptions(IApizrResilienceContextOptions options)
    {
        Options.ResilienceContextOptions = options;

        return this;
    }

    /// <inheritdoc />
    IApizrRequestOptionsBuilder IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>.WithContext(ResilienceContext context)
    {
        Options.ResilienceContext = context;

        return this;
    } 

    #endregion
}