using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    public interface IApizrExtendedOptionsBuilderBase : IApizrExtendedCommonOptionsBuilderBase, IApizrExtendedProperOptionsBuilderBase, IApizrGlobalOptionsBuilderBase
    {
    }

    public interface IApizrExtendedOptionsBuilderBase<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> :
        IApizrGlobalOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedOptionsBuilderBase
        where TApizrExtendedOptions : IApizrExtendedOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }
}
