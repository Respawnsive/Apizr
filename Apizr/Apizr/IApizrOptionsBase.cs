using System;
using HttpTracer;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Options
    /// </summary>
    public interface IApizrOptionsBase
    {
        /// <summary>
        /// Web api interface type
        /// </summary>
        Type WebApiType { get; }

        /// <summary>
        /// Base address
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// Http traffic tracing verbosity
        /// </summary>
        HttpMessageParts TrafficVerbosity { get; }

        /// <summary>
        /// Http traffic tracing log level
        /// </summary>
        LogLevel TrafficLogLevel { get; }

        /// <summary>
        /// Policy keys from the registry
        /// </summary>
        string[] PolicyRegistryKeys { get; }

        /// <summary>
        /// HttpContent serializer
        /// </summary>
        IHttpContentSerializer ContentSerializer { get; }
    }
}