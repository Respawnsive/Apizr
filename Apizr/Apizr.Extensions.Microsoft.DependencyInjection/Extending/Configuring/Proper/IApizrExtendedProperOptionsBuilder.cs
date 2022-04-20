using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;

namespace Apizr.Extending.Configuring.Proper
{
    public interface IApizrExtendedProperOptionsBuilder<out TApizrExtendedProperOptions, out TApizrExtendedProperOptionsBuilder> : IApizrExtendedProperOptionsBuilderBase,
        IApizrGlobalProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>,
        IApizrExtendedSharedOptionsBuilder<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
        where TApizrExtendedProperOptions : IApizrProperOptionsBase
        where TApizrExtendedProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
    {
    }

    public interface IApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder<IApizrExtendedProperOptions, IApizrExtendedProperOptionsBuilder>
    { }
}
