using System;
using Apizr.Configuring.Shared;

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
    }
}
