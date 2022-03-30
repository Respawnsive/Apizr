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
        /// Refit settings
        /// </summary>
        RefitSettings RefitSettings { get; }
    }
}
