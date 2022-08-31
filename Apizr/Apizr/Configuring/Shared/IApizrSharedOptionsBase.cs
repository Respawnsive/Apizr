using System;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    public interface IApizrSharedOptionsBase
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
    }
}
