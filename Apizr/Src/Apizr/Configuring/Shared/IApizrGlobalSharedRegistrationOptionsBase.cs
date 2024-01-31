using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Net.Http;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level (not request) and for all (static and extended) registration types
    /// </summary>
    public interface IApizrGlobalSharedRegistrationOptionsBase : IApizrGlobalSharedOptionsBase
    {
        /// <summary>
        /// Base address
        /// </summary>
        Uri BaseUri { get; }

        /// <summary>
        /// Base address
        /// </summary>
        string BaseAddress { get; }

        /// <summary>
        /// Base path
        /// </summary>
        string BasePath { get; }

        /// <summary>
        /// The Polly Context to pass through it all
        /// </summary>
        Func<Context> ContextFactory { get; }

        /// <summary>
        /// The Resilience Context to pass through it all
        /// </summary>
        Func<ResilienceContext> ResilienceContextFactory { get; }

        /// <summary>
        /// The primary Http message handler factory (set internally)
        /// </summary>
        Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> PrimaryHandlerFactory { get; }
    }
}
