using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    public interface IApizrOptionsBuilderBase : IApizrCommonOptionsBuilderBase, IApizrProperOptionsBuilderBase
    {}

    public interface IApizrOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : IApizrOptionsBuilderBase,
        IApizrCommonOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrProperOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder> 
        where TApizrOptions : IApizrOptionsBase 
        where TApizrOptionsBuilder : IApizrOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
