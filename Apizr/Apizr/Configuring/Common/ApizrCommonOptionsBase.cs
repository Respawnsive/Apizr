using Apizr.Configuring.Shared;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <inheritdoc cref="IApizrCommonOptionsBase" />
    public abstract class ApizrCommonOptionsBase : ApizrSharedOptionsBase, IApizrCommonOptionsBase
    {
        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }
    }
}
