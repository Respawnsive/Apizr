using Apizr.Logging;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Apizr.Configuring.Shared.Context;
using Apizr.Resiliencing;

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
        Action<ApizrException> OnException { get; }

        /// <summary>
        /// Let throw potential exception if there's no cached data to return
        /// </summary>
        bool LetThrowOnExceptionWithEmptyCache { get; }

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
        /// The resilience context options builder
        /// </summary>
        internal Action<IApizrResilienceContextOptionsBuilder> ContextOptionsBuilder { get; set; }

        internal IDictionary<string, Func<object>> ResiliencePropertiesFactories { get; }
    }
}
