using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;

namespace Apizr.Extending.Configuring.Proper
{
    public interface IApizrExtendedProperOptionsBuilder<out TApizrExtendedProperOptions, out TApizrExtendedProperOptionsBuilder> : IApizrProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>,
        IApizrExtendedSharedOptionsBuilder<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
        where TApizrExtendedProperOptions : IApizrProperOptionsBase
        where TApizrExtendedProperOptionsBuilder : IApizrProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
    {
    }

    public interface IApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder<IApizrExtendedProperOptions, IApizrExtendedProperOptionsBuilder>
    { }
}
