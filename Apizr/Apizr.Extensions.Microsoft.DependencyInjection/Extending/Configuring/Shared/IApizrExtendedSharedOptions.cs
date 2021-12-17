using System;
using System.Net.Http;
using Apizr.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Shared
{
    public interface IApizrExtendedSharedOptions : IApizrExtendedSharedOptionsBase
    {
        /// <summary>
        /// Http traffic tracing mode factory
        /// </summary>
        Func<IServiceProvider, HttpTracerMode> HttpTracerModeFactory { get; }

        /// <summary>
        /// Http traffic tracing verbosity factory
        /// </summary>
        Func<IServiceProvider, HttpMessageParts> TrafficVerbosityFactory { get; }

        /// <summary>
        /// Log level factory
        /// </summary>
        Func<IServiceProvider, LogLevel> LogLevelFactory { get; }

        /// <summary>
        /// HttpClientHandler factory
        /// </summary>
        Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; }

        /// <summary>
        /// HttpClient builder
        /// </summary>
        Action<IHttpClientBuilder> HttpClientBuilder { get; }
    }
}
