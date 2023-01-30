using Apizr.Configuring.Shared;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <summary>
    /// Options available at common level for both static and extended registrations
    /// </summary>
    public interface IApizrCommonOptionsBase : IApizrSharedRegistrationOptionsBase
    {
        /// <summary>
        /// Refit settings
        /// </summary>
        RefitSettings RefitSettings { get; }
    }
}
