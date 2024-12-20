﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Logging;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;
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
        Options.LogLevels = logLevels?.Length > 0 ? logLevels : Constants.DefaultLogLevels;

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
    [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
    public IApizrRequestOptionsBuilder WithExCatching(Action<ApizrException> onException,
        bool letThrowOnException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(new ApizrExceptionHandler(onException),
            letThrowOnException,
            strategy);

    /// <inheritdoc />
    [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
    public IApizrRequestOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
        bool letThrowOnException = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(
            new ApizrExceptionHandler<TResult>(onException),
            letThrowOnException,
            strategy);

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching(Func<ApizrException, bool> onException, bool letThrowOnHandledException = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(
            new ApizrExceptionHandler(onException),
            letThrowOnHandledException,
            strategy);

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching<TResult>(Func<ApizrException<TResult>, bool> onException,
        bool letThrowOnHandledException = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(
            new ApizrExceptionHandler<TResult>(onException),
            letThrowOnHandledException,
            strategy);

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching(Func<ApizrException, Task<bool>> onException, bool letThrowOnHandledException = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(new ApizrExceptionHandler(onException), letThrowOnHandledException,
            strategy);

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching<THandler>(THandler exceptionHandler, bool letThrowOnHandledException = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler
    {
        switch (strategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                if (Options.ExceptionHandlers.Count == 0)
                    Options.ExceptionHandlers.Add(exceptionHandler);
                break;
            case ApizrDuplicateStrategy.Replace:
                Options.ExceptionHandlers.Clear();
                Options.ExceptionHandlers.Add(exceptionHandler);
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                Options.ExceptionHandlers.Add(exceptionHandler);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
        }

        Options.LetThrowOnHandledException = letThrowOnHandledException;

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithExCatching<TResult>(Func<ApizrException<TResult>, Task<bool>> onException, bool letThrowOnHandledException = true,
        ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        => WithExCatching(
            new ApizrExceptionHandler<TResult>(onException),
            letThrowOnHandledException,
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
        ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
    {
        var methodScope = new[]{Options.RequestMethod};

        switch (duplicateStrategy)
        {
            case ApizrDuplicateStrategy.Ignore:
                if (Options.ResiliencePipelineOptions.Count == 0)
                    Options.ResiliencePipelineOptions[ApizrConfigurationSource.RequestOption] = methodScope
                        .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                        .Cast<ResiliencePipelineAttributeBase>()
                        .ToArray();
                break;
            case ApizrDuplicateStrategy.Add:
            case ApizrDuplicateStrategy.Merge:
                if (Options.ResiliencePipelineOptions.TryGetValue(ApizrConfigurationSource.RequestOption, out var attributes))
                {
                    foreach (var method in methodScope)
                    {
                        var attribute = attributes.FirstOrDefault(attribute => attribute.RequestMethod == method);
                        if (attribute != null)
                            attribute.RegistryKeys = attribute.RegistryKeys.Union(resiliencePipelineKeys).ToArray();
                        else
                            attributes = attributes.Concat([new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method }]).ToArray();
                    }

                    Options.ResiliencePipelineOptions[ApizrConfigurationSource.RequestOption] = attributes;
                }
                else
                {
                    Options.ResiliencePipelineOptions[ApizrConfigurationSource.RequestOption] = methodScope
                        .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                        .Cast<ResiliencePipelineAttributeBase>()
                        .ToArray();
                }

                break;
            case ApizrDuplicateStrategy.Replace:
                Options.ResiliencePipelineOptions.Clear();
                Options.ResiliencePipelineOptions[ApizrConfigurationSource.RequestOption] = methodScope
                    .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                    .Cast<ResiliencePipelineAttributeBase>()
                    .ToArray();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(duplicateStrategy), duplicateStrategy, null);
        }

        return this;
    }

    /// <inheritdoc />
    public IApizrRequestOptionsBuilder WithCaching(CacheMode mode = CacheMode.FetchOrGet, TimeSpan? lifeSpan = null,
        bool shouldInvalidateOnError = false)
    {
        Options.CacheOptions[ApizrConfigurationSource.RequestOption] = new CacheAttribute(mode, lifeSpan, shouldInvalidateOnError);

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

    /// <inheritdoc />
    IApizrRequestOptionsBuilder IApizrRequestOptionsBuilder<IApizrRequestOptions, IApizrRequestOptionsBuilder>.WithResiliencePipelineOptions(IDictionary<ApizrConfigurationSource, ResiliencePipelineAttributeBase[]> resiliencePipelineOptions)
    {
        Options.ResiliencePipelineOptions = resiliencePipelineOptions;

        return this;
    }

    #endregion
}