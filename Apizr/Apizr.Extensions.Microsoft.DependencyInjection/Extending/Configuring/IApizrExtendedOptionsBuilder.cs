using Apizr.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    public interface IApizrExtendedOptionsBuilder<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> : IApizrExtendedOptionsBuilderBase,
        IApizrOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedCommonOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedProperOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
        where TApizrExtendedOptions : IApizrOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }

    public interface IApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilder<IApizrExtendedOptions, IApizrExtendedOptionsBuilder>
    { }
}
