using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;
using System;

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
        /// Define tracer mode, http traffic tracing verbosity and log levels (could be defined with LogAttribute)
        /// </summary>
        /// <param name="httpTracerMode"></param>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: All)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: Information)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels);

        /// <summary>
        /// Add some headers to the request
        /// </summary>
        /// <param name="headers">Headers to add to the request</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithHeaders(params string[] headers);

        /// <summary>
        /// Set a timeout to the request
        /// </summary>
        /// <param name="timeout">The request timeout</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithTimeout(TimeSpan timeout);
    }
}
