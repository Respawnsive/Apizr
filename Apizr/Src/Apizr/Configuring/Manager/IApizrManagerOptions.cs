using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Manager
{
    /// <summary>
    /// Options available for static registrations
    /// </summary>
    public interface IApizrManagerOptions : IApizrManagerOptionsBase, IApizrCommonOptions, IApizrProperOptions
    {
    }

    /// <summary>
    /// Options available for static registrations
    /// </summary>
    /// <typeparam name="TWebApi"></typeparam>
    public interface IApizrManagerOptions<TWebApi> : IApizrManagerOptionsBase
    {
    }
}
