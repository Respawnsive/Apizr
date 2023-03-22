using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrProperOptionsBuilderBase : IApizrGlobalProperOptionsBuilderBase, IApizrSharedRegistrationOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> :
        IApizrGlobalProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>,
        IApizrSharedRegistrationOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>, 
        IApizrProperOptionsBuilderBase
        where TApizrProperOptions : IApizrProperOptionsBase
        where TApizrProperOptionsBuilder :
        IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }
}
