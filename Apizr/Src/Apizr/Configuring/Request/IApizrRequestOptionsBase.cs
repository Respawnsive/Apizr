using Apizr.Configuring.Shared;
using Polly;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// Options available at request levels and for all (static and extended) registration types
    /// </summary>
    public interface IApizrRequestOptionsBase : IApizrGlobalSharedOptionsBase
    {
        /// <summary>
        /// The Polly Context to pass through it all
        /// </summary>
        Context Context { get; }
    }
}
