using System;
using System.Net;
using HttpTracer;

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
        /// Web api base address
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// HttpClient decompression methods
        /// </summary>
        DecompressionMethods DecompressionMethods { get; }

        /// <summary>
        /// Request tracing verbosity
        /// </summary>
        HttpMessageParts HttpTracerVerbosity { get; }

        /// <summary>
        /// Policy keys from the registry
        /// </summary>
        string[] PolicyRegistryKeys { get; }
    }
}