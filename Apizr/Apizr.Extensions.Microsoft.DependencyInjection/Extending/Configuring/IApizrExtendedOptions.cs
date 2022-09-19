using Apizr.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    /// <summary>
    /// Options available for extended registrations
    /// </summary>
    public interface IApizrExtendedOptions : IApizrExtendedOptionsBase, IApizrExtendedCommonOptions, IApizrExtendedProperOptions
    {
    }

    /// <summary>
    /// Options available for extended registrations
    /// </summary>
    public interface IApizrExtendedOptions<TWebApi> : IApizrOptions<TWebApi>, IApizrExtendedOptionsBase
    {

    }
}
