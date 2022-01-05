using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    public interface IApizrOptionsBuilderBase : IApizrCommonOptionsBuilderBase, IApizrProperOptionsBuilderBase, IApizrGlobalOptionsBuilderBase
    {
    }

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
