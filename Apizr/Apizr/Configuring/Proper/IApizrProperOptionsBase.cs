using System;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Common options
    /// </summary>
    public interface IApizrProperOptionsBase : IApizrSharedOptionsBase
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
        /// Policy keys from the registry
        /// </summary>
        string[] PolicyRegistryKeys { get; }

        /// <summary>
        /// The logger instance
        /// </summary>
        ILogger Logger { get; }
    }
}
