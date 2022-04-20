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
        /// The Logger factory
        /// </summary>
        Func<IServiceProvider, string, ILogger> LoggerFactory { get; }
    }
}
