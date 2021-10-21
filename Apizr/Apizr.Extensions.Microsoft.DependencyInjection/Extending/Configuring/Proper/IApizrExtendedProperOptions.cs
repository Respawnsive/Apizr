using System;
using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;

namespace Apizr.Extending.Configuring.Proper
{
    public interface IApizrExtendedProperOptions : IApizrProperOptionsBase, IApizrExtendedSharedOptions
    {
        /// <summary>
        /// Type of the manager
        /// </summary>
        Type ApizrManagerType { get; }

        /// <summary>
        /// Base address factory
        /// </summary>
        Func<IServiceProvider, Uri> BaseAddressFactory { get; }
    }
}
