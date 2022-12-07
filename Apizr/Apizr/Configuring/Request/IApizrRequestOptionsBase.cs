using Apizr.Configuring.Shared;
using Polly;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptionsBase : IApizrGlobalSharedOptionsBase
    {
        Context Context { get; }
    }
}
