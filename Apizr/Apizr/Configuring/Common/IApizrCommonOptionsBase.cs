using Apizr.Configuring.Shared;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <summary>
    /// Common options
    /// </summary>
    public interface IApizrCommonOptionsBase : IApizrSharedOptionsBase
    {
        /// <summary>
        /// HttpContent serializer
        /// </summary>
        IHttpContentSerializer ContentSerializer { get; }
    }
}
