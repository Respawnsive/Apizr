using System;
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
        /// Request tracing verbosity
        /// </summary>
        HttpMessageParts HttpTracerVerbosity { get; }

        /// <summary>
        /// Fusillade priority management activation
        /// </summary>
        bool IsPriorityManagementEnabled { get; }

        /// <summary>
        /// Policy keys from the registry
        /// </summary>
        string[] PolicyRegistryKeys { get; }
    }
}