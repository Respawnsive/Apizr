using Apizr.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    public interface IApizrExtendedOptions : IApizrExtendedOptionsBase, IApizrExtendedCommonOptions, IApizrExtendedProperOptions
    {
    }

    public interface IApizrExtendedOptions<TWebApi> : IApizrOptions<TWebApi>, IApizrExtendedOptionsBase
    {

    }
}
