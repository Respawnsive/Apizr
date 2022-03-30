using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    public interface IApizrOptions : IApizrOptionsBase, IApizrCommonOptions, IApizrProperOptions
    {
    }

    /// <summary>
    /// Options dedicated to <typeparamref name="TWebApi"/>
    /// </summary>
    /// <typeparam name="TWebApi"></typeparam>
    public interface IApizrOptions<TWebApi> : IApizrOptionsBase
    {
    }
}
