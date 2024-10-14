using Apizr.Logging;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Shared.Context;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at all (common, proper and request) levels and for all (static and extended) registration types
    /// </summary>
    public interface IApizrGlobalSharedOptionsBase
    {
        /// <summary>
        /// Http traffic tracing mode
        /// </summary>
        HttpTracerMode HttpTracerMode { get; }

        /// <summary>
        /// Http traffic tracing verbosity
        /// </summary>
        HttpMessageParts TrafficVerbosity { get; }

        /// <summary>
        /// Log levels while writing
        /// </summary>
        LogLevel[] LogLevels { get; }

        /// <summary>
        /// Catching potential exception if defined
        /// </summary>
        Func<ApizrException, Task<bool>> OnException { get; }

        /// <summary>
        /// Let throw potential exception even if it's handled (default: true)
        /// </summary>
        bool LetThrowOnHandledException { get; }

        /// <summary>
        /// Parameters passed through delegating handlers
        /// </summary>
        IDictionary<string, object> HandlersParameters { get; }

        /// <summary>
        /// The operation timeout (overall request tries)
        /// </summary>
        TimeSpan? OperationTimeout { get; }

        /// <summary>
        /// The request timeout (each request try)
        /// </summary>
        TimeSpan? RequestTimeout { get; }

        /// <summary>
        /// The <see cref="Func{T, R}"/> which determines whether to redact the HTTP header value before logging.
        /// </summary>
        public Func<string, bool> ShouldRedactHeaderValue { get; }

        /// <summary>
        /// Resilience pipeline keys from the registry
        /// </summary>
        IDictionary<ApizrConfigurationSource, ResiliencePipelineAttributeBase[]> ResiliencePipelineOptions { get; }

        /// <summary>
        /// The caching options to apply
        /// </summary>
        IDictionary<ApizrConfigurationSource, CacheAttributeBase> CacheOptions { get; }

        /// <summary>
        /// The resilience context options builder
        /// </summary>
        internal Action<IApizrResilienceContextOptionsBuilder> ContextOptionsBuilder { get; set; }

        internal IDictionary<string, Func<object>> ResiliencePropertiesFactories { get; }
    }
}
