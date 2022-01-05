using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptionsBuilderBase : IApizrGlobalProperOptionsBuilderBase, IApizrSharedOptionsBuilderBase
    {
    }

    public interface IApizrProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> :
        IApizrGlobalProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>,
        IApizrSharedOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>, 
        IApizrProperOptionsBuilderBase
        where TApizrProperOptions : IApizrProperOptionsBase
        where TApizrProperOptionsBuilder :
        IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }
}
