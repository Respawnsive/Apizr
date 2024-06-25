using Apizr.Configuring.Shared;
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// Configuration section for Apizr
        /// </summary>
        IConfigurationSection ApizrConfigurationSection { get; }
    }
}
