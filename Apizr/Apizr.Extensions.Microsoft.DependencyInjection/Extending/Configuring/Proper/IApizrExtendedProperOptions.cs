using System;
using Apizr.Extending.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Proper
{
    public interface IApizrExtendedProperOptions : IApizrExtendedProperOptionsBase, IApizrExtendedSharedOptions
    {
        /// <summary>
        /// Type of the manager
        /// </summary>
        Type ApizrManagerType { get; }

        /// <summary>
        /// Base address factory
        /// </summary>
        Func<IServiceProvider, Uri> BaseAddressFactory { get; }

        /// <summary>
        /// The Logger factory
        /// </summary>
        Func<IServiceProvider, string, ILogger> LoggerFactory { get; }
    }
}
