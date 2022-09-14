using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalOptionsBuilderBase : IApizrGlobalCommonOptionsBuilderBase, IApizrGlobalProperOptionsBuilderBase
    { }

    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : IApizrGlobalOptionsBuilderBase,
        IApizrGlobalCommonOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrGlobalProperOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder> 
        where TApizrOptions : IApizrOptionsBase 
        where TApizrOptionsBuilder : IApizrGlobalOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
