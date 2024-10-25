using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;

namespace Apizr.Extending.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for extended registrations
    /// </summary>
    public interface IApizrExtendedProperOptionsBuilder<out TApizrExtendedProperOptions, out TApizrExtendedProperOptionsBuilder> : IApizrExtendedProperOptionsBuilderBase,
        IApizrGlobalProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>,
        IApizrExtendedSharedOptionsBuilder<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
        where TApizrExtendedProperOptions : IApizrProperOptionsBase
        where TApizrExtendedProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
    {
    }

    /// <summary>
    /// Builder options available at proper level for extended registrations
    /// </summary>
    public interface IApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder<IApizrExtendedProperOptions, IApizrExtendedProperOptionsBuilder>
    {
        internal IApizrExtendedProperOptions ApizrOptions { get; }
    }
}
