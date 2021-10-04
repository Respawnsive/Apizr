using System;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Specific options
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
        /// Http traffic tracing mode
        /// </summary>
        HttpTracerMode HttpTracerMode { get; }

        /// <summary>
        /// Http traffic tracing verbosity
        /// </summary>
        HttpMessageParts TrafficVerbosity { get; }

        /// <summary>
        /// Log level while writing
        /// </summary>
        LogLevel LogLevel { get; }

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