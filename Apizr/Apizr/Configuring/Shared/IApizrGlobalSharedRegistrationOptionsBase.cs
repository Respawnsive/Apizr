using Polly;
using System;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level (not request) and for all (static and extended) registration types
    /// </summary>
    public interface IApizrGlobalSharedRegistrationOptionsBase : IApizrGlobalSharedOptionsBase
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

        Func<Context> ContextFactory { get; }
    }
}
