using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrOptionsBuilderBase : IApizrCommonOptionsBuilderBase, IApizrProperOptionsBuilderBase, IApizrGlobalOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrGlobalOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrCommonOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrProperOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>, 
        IApizrOptionsBuilderBase
        where TApizrOptions : IApizrOptionsBase
        where TApizrOptionsBuilder :
        IApizrOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
