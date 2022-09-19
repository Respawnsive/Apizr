using System;
using Apizr.Extending.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Configuring.Proper
{
    /// <summary>
    /// Options available at proper level for extended registrations
    /// </summary>
    public interface IApizrExtendedProperOptions : IApizrExtendedProperOptionsBase, IApizrExtendedSharedOptions
    {
        /// <summary>
        /// Type of the manager
        /// </summary>
        Type ApizrManagerType { get; }

        /// <summary>
        /// The Logger factory
        /// </summary>
        Func<IServiceProvider, string, ILogger> LoggerFactory { get; }
    }
}
