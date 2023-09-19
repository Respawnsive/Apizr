using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring.Manager;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for static registrations
    /// </summary>
    public interface IApizrSharedRegistrationOptions : IApizrSharedRegistrationOptionsBase
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
        /// HttpClient factory
        /// </summary>
        Func<HttpMessageHandler, Uri, HttpClient> HttpClientFactory { get; }

        /// <summary>
        /// HttpClient configuration builder
        /// </summary>
        Action<HttpClient> HttpClientConfigurationBuilder { get; }

        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IDictionary<Type, Func<ILogger, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }

        /// <summary>
        /// Headers factory
        /// </summary>
        Func<IList<string>> HeadersFactory { get; }
    }
}
