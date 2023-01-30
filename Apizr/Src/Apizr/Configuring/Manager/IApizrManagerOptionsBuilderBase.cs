using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Manager
{
    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrManagerOptionsBuilderBase : IApizrCommonOptionsBuilderBase, IApizrProperOptionsBuilderBase, IApizrGlobalManagerOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrManagerOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrGlobalManagerOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrCommonOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrProperOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>, 
        IApizrManagerOptionsBuilderBase
        where TApizrOptions : IApizrManagerOptionsBase
        where TApizrOptionsBuilder :
        IApizrManagerOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
