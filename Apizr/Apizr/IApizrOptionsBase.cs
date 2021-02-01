using System;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using HttpTracer;
using Polly.Registry;
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
        /// Request tracing verbosity
        /// </summary>
        HttpMessageParts HttpTracerVerbosity { get; }

        /// <summary>
        /// Apizr executions tracing verbosity
        /// </summary>
        ApizrLogLevel ApizrVerbosity { get; }

        /// <summary>
        /// Policy keys from the registry
        /// </summary>
        string[] PolicyRegistryKeys { get; }
    }
}