using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    public interface IApizrGlobalOptionsBuilderBase : IApizrGlobalCommonOptionsBuilderBase, IApizrGlobalProperOptionsBuilderBase
    { }

    public interface IApizrGlobalOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> : IApizrGlobalOptionsBuilderBase,
        IApizrGlobalCommonOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrGlobalProperOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder> 
        where TApizrOptions : IApizrOptionsBase 
        where TApizrOptionsBuilder : IApizrGlobalOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
