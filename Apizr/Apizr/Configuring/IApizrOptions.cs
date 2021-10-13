using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring
{
    /// <summary>
    /// Options
    /// </summary>
    public interface IApizrOptions : IApizrOptionsBase, IApizrCommonOptions, IApizrProperOptions
    {
    }

    public interface IApizrOptions<TWebApi> : IApizrOptionsBase
    {
        ILogger Logger { get; }
    }
}
