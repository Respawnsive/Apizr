using System;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptions : IApizrProperOptionsBase, IApizrSharedOptions
    {
        /// <summary>
        /// The Logger factory
        /// </summary>
        Func<ILoggerFactory, string, ILogger> LoggerFactory { get; }
    }
}
