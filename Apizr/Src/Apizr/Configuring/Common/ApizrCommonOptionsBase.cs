using Apizr.Configuring.Shared;
using Microsoft.Extensions.Configuration;
using Refit;

namespace Apizr.Configuring.Common
{
    /// <inheritdoc cref="IApizrCommonOptionsBase" />
    public abstract class ApizrCommonOptionsBase : ApizrGlobalSharedRegistrationOptionsBase, IApizrCommonOptionsBase
    {
        /// <inheritdoc />
        protected ApizrCommonOptionsBase()
        {
        }

        /// <inheritdoc />
        public RefitSettings RefitSettings { get; protected set; }

        /// <inheritdoc />
        public IConfigurationSection ApizrConfigurationSection { get; internal set; }
    }
}
