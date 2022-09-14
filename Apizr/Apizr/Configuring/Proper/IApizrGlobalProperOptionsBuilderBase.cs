using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalProperOptionsBuilderBase : IApizrGlobalSharedOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrGlobalProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrGlobalProperOptionsBuilderBase,
        IApizrGlobalSharedOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    where TApizrProperOptions : IApizrProperOptionsBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }
}
