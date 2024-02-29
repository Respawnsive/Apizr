using System;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrProperOptionsBase : IApizrSharedRegistrationOptionsBase
    {
        /// <summary>
        /// Web api interface type
        /// </summary>
        Type WebApiType { get; }

        /// <summary>
        /// Resilience pipeline keys from the registry
        /// </summary>
        string[] ResiliencePipelineRegistryKeys { get; }

        /// <summary>
        /// The logger instance
        /// </summary>
        ILogger Logger { get; }
    }
}
