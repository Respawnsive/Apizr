using Apizr.Logging;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Polly;

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

        /// <inheritdoc />
        Context Context { get; }

        /// <inheritdoc />
        Action<ApizrException> OnException { get; }

        /// <inheritdoc />
        bool LetThrowOnExceptionWithEmptyCache { get; }

        /// <inheritdoc />
        IDictionary<string, object> HandlersParameters { get; }
    }
}
