using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using Apizr.Configuring.Shared.Context;
using Apizr.Resiliencing;
using System.Collections.Generic;
using Apizr.Caching;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Builder options available at all (common, proper and request) levels and for all (static and extended) registration types
    /// </summary>
    public interface IApizrGlobalSharedOptionsBuilderBase
    {
    }

    /// <inheritdoc />
    public interface IApizrGlobalSharedOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : IApizrGlobalSharedOptionsBuilderBase
        where TApizrOptions : IApizrGlobalSharedOptionsBase
        where TApizrOptionsBuilder : IApizrGlobalSharedOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="onException">The exception callback</param>
        /// <param name="letThrowOnExceptionWithEmptyCache">Let throw potential exception if there's no cached data to return (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithExCatching(Action<ApizrException> onException, bool letThrowOnExceptionWithEmptyCache = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Catch potential exceptions
        /// </summary>
        /// <param name="onException">The exception callback</param>
        /// <param name="letThrowOnExceptionWithEmptyCache">Let throw potential exception if there's no cached data to return (default: true)</param>
        /// <param name="strategy">The duplicate strategy if there's another callback already (default: Replace)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException, bool letThrowOnExceptionWithEmptyCache = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        /// <summary>
        /// Set some parameters passed through all delegating handlers
        /// </summary>
        /// <param name="key">The parameter's key</param>
        /// <param name="value">The parameter's value</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHandlerParameter(string key, object value);

        /// <summary>
        /// Set some resilience properties to the resilience context
        /// </summary>
        /// <param name="key">The resilience property's key</param>
        /// <param name="value">The resilience property's value</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value);

        /// <summary>
        /// Define tracer mode, http traffic tracing verbosity and log levels (could be defined with LogAttribute)
        /// </summary>
        /// <param name="httpTracerMode"></param>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: All)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: Information)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels);

        /// <summary>
        /// Set a timeout to the operation (overall request tries)
        /// </summary>
        /// <param name="timeout">The operation timeout</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithOperationTimeout(TimeSpan timeout);

        /// <summary>
        /// Set a timeout to the request (each request try)
        /// </summary>
        /// <param name="timeout">The request timeout</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithRequestTimeout(TimeSpan timeout);

        /// <summary>
        /// Set some options to the resilience context
        /// </summary>
        /// <param name="contextOptionsBuilder">The resilience context options builder</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder);

        /// <summary>
        /// Sets the collection of HTTP headers names for which values should be redacted before logging.
        /// </summary>
        /// <param name="redactedLoggedHeaderNames">The collection of HTTP headers names for which values should be redacted before logging.</param>
        /// <param name="strategy">The duplicate strategy if there's any other names already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add);

        /// <summary>
        /// Sets the <see cref="Func{T, R}"/> which determines whether to redact the HTTP header value before logging.
        /// </summary>
        /// <param name="shouldRedactHeaderValue">The <see cref="Func{T, R}"/> which determines whether to redact the HTTP header value before logging</param>
        /// <param name="strategy">The duplicate strategy if there's any other names already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLoggedHeadersRedactionRule(Func<string, bool> shouldRedactHeaderValue, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add);

        /// <summary>
        /// Apply some resilience strategies by getting pipelines from registry with key matching.
        /// </summary>
        /// <param name="resiliencePipelineKeys">Resilience pipeline keys from the registry.</param>
        /// <param name="strategy">The duplicate strategy if there's any other names already (default: Add)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add);

        /// <summary>
        /// Cache data.
        /// </summary>
        /// <param name="mode">GetAndFetch returns fresh data when request succeed otherwise cached one, where GetOrFetch returns cached data if we get some otherwise fresh one</param>
        /// <param name="lifeSpan">This specific caching lifetime (Default: null = cache handler lifetime</param>
        /// <param name="shouldInvalidateOnError">Should invalidate on error (Default: false)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithCaching(CacheMode mode = CacheMode.GetAndFetch, TimeSpan? lifeSpan = null, bool shouldInvalidateOnError = false);
    }
}
