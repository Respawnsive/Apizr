using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    public interface IApizrSharedOptions : IApizrSharedOptionsBase
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
        /// Delegating handlers factories
        /// </summary>
        IList<Func<ILogger, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}
