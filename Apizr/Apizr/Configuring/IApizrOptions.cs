using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    /// <summary>
    /// Options available for static registrations
    /// </summary>
    public interface IApizrOptions : IApizrOptionsBase, IApizrCommonOptions, IApizrProperOptions
    {
    }

    /// <summary>
    /// Options available for static registrations
    /// </summary>
    /// <typeparam name="TWebApi"></typeparam>
    public interface IApizrOptions<TWebApi> : IApizrOptionsBase
    {
    }
}
