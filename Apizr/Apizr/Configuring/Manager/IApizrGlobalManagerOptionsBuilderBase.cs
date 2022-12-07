using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Manager
{
    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalManagerOptionsBuilderBase : IApizrGlobalCommonOptionsBuilderBase, IApizrGlobalProperOptionsBuilderBase
    { }

    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalManagerOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : IApizrGlobalManagerOptionsBuilderBase,
        IApizrGlobalCommonOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrGlobalProperOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder> 
        where TApizrOptions : IApizrManagerOptionsBase 
        where TApizrOptionsBuilder : IApizrGlobalManagerOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
