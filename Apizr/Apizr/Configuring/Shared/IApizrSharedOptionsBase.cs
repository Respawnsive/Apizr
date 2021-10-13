using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    public interface IApizrSharedOptionsBase
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
        /// Log level while writing
        /// </summary>
        LogLevel LogLevel { get; }
    }
}
