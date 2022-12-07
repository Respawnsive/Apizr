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
        TApizrOptionsBuilder WithExCatching(Action<ApizrException> onException, bool letThrowOnExceptionWithEmptyCache = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace);

        TApizrOptionsBuilder WithHandlerParameter(string key, object value);

        /// <summary>
        /// Configure logging level for the api
        /// </summary>
        /// <param name="httpTracerMode"></param>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: All)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: Information)</param>
        /// <returns></returns>
        TApizrOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels);
    }
}
