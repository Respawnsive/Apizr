using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring.Manager;
using Apizr.Logging;
using Apizr.Resiliencing;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for static registrations
    /// </summary>
    public interface IApizrSharedRegistrationOptions : IApizrSharedRegistrationOptionsBase, IApizrGlobalSharedOptions
    {
        /// <summary>
        /// Base uri factory
        /// </summary>
        Func<Uri> BaseUriFactory { get; }

        /// <summary>
        /// Base address factory
        /// </summary>
        Func<string> BaseAddressFactory { get; }

        /// <summary>
        /// Base path factory
        /// </summary>
        Func<string> BasePathFactory { get; }

        /// <summary>
        /// Http traffic tracing mode factory
        /// </summary>
        Func<HttpTracerMode> HttpTracerModeFactory { get; }

        /// <summary>
        /// Http traffic tracing verbosity factory
        /// </summary>
        Func<HttpMessageParts> TrafficVerbosityFactory { get; }

        /// <summary>
        /// Log levels factory
        /// </summary>
        Func<LogLevel[]> LogLevelsFactory { get; }

        /// <summary>
        /// HttpClientHandler factory
        /// </summary>
        Func<HttpClientHandler> HttpClientHandlerFactory { get; }

        /// <summary>
        /// HttpClient configuration builder
        /// </summary>
        Action<HttpClient> HttpClientConfigurationBuilder { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IDictionary<Type, Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
        
        /// <summary>
        /// The operation timeout factory (overall request tries)
        /// </summary>
        Func<TimeSpan> OperationTimeoutFactory { get; }

        /// <summary>
        /// The request timeout factory (each request try)
        /// </summary>
        Func<TimeSpan> RequestTimeoutFactory { get; }
    }
}
