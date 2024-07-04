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
        /// Type of the crud api entity key if any
        /// </summary>
        Type CrudApiEntityKeyType { get; }

        /// <summary>
        /// Type of the crud api read all result if any
        /// </summary>
        Type CrudApiReadAllResultType { get; }

        /// <summary>
        /// Type of the crud api read all parameters if any
        /// </summary>
        Type CrudApiReadAllParamsType { get; }

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
