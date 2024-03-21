using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for extended registrations
    /// </summary>
    public interface IApizrExtendedSharedOptions : IApizrExtendedSharedOptionsBase
    {
        /// <summary>
        /// Base Uri factory
        /// </summary>
        Func<IServiceProvider, Uri> BaseUriFactory { get; }

        /// <summary>
        /// Base address factory
        /// </summary>
        Func<IServiceProvider, string> BaseAddressFactory { get; }

        /// <summary>
        /// Base path factory
        /// </summary>
        Func<IServiceProvider, string> BasePathFactory { get; }

        /// <summary>
        /// Http traffic tracing mode factory
        /// </summary>
        Func<IServiceProvider, HttpTracerMode> HttpTracerModeFactory { get; }

        /// <summary>
        /// Http traffic tracing verbosity factory
        /// </summary>
        Func<IServiceProvider, HttpMessageParts> TrafficVerbosityFactory { get; }

        /// <summary>
        /// Log levels factory
        /// </summary>
        Func<IServiceProvider, LogLevel[]> LogLevelsFactory { get; }

        /// <summary>
        /// HttpClientHandler factory
        /// </summary>
        Func<IServiceProvider, HttpClientHandler> HttpClientHandlerFactory { get; }

        /// <summary>
        /// HttpClient builder
        /// </summary>
        Action<IHttpClientBuilder> HttpClientBuilder { get; }

        /// <summary>
        /// The operation timeout factory (overall request tries)
        /// </summary>
        Func<IServiceProvider, TimeSpan> OperationTimeoutFactory { get; }

        /// <summary>
        /// The request timeout factory (each request try)
        /// </summary>
        Func<IServiceProvider, TimeSpan> RequestTimeoutFactory { get; }

        /// <summary>
        /// Headers factories
        /// </summary>
        IDictionary<ApizrLifetimeScope, Func<IServiceProvider, Func<IList<string>>>> HeadersExtendedFactories { get; }

        internal IDictionary<string, Func<IServiceProvider, object>> ResiliencePropertiesExtendedFactories { get; }
    }
}