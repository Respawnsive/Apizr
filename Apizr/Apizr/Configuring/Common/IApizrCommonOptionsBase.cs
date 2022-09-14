using Apizr.Configuring.Shared;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <summary>
    /// Options available at common for both static and extended registrations
    /// </summary>
    public interface IApizrCommonOptionsBase : IApizrSharedOptionsBase
    {
        /// <summary>
        /// Refit settings
        /// </summary>
        RefitSettings RefitSettings { get; }
    }
}
