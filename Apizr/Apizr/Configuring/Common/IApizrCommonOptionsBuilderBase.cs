using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Common
{
    public interface IApizrCommonOptionsBuilderBase : IApizrGlobalCommonOptionsBuilderBase, IApizrSharedOptionsBuilderBase
    {
    }

    public interface IApizrCommonOptionsBuilderBase<out TApizrCommonOptions, out TApizrCommonOptionsBuilder> :
        IApizrGlobalCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>,
        IApizrSharedOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>,
        IApizrCommonOptionsBuilderBase
        where TApizrCommonOptions : IApizrCommonOptionsBase
        where TApizrCommonOptionsBuilder :
        IApizrCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>
    {
    }
}
