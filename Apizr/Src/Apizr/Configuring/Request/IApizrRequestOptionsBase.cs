using Apizr.Configuring.Shared;
using Polly;
using System;
using System.Collections.Generic;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// Options available at request levels and for all (static and extended) registration types
    /// </summary>
    public interface IApizrRequestOptionsBase : IApizrGlobalSharedOptionsBase
    {
        /// <summary>
        /// The Polly resilience context to pass through it all
        /// </summary>
        ResilienceContext ResilienceContext { get; }

        /// <summary>
        /// The Apizr request method
        /// </summary>
        ApizrRequestMethod RequestMethod { get; }
    }
}
