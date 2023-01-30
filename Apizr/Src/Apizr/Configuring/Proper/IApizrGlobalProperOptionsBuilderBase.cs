using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalProperOptionsBuilderBase : IApizrGlobalSharedRegistrationOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrGlobalProperOptionsBuilderBase,
        IApizrGlobalSharedRegistrationOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    where TApizrProperOptions : IApizrProperOptionsBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }
}
