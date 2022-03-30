namespace Apizr.Configuring.Shared
{
    public interface IApizrSharedOptionsBuilderBase : IApizrGlobalSharedOptionsBuilderBase
    {
    }

    public interface IApizrSharedOptionsBuilderBase<out TApizrSharedOptions, out TApizrSharedOptionsBuilder> :
            IApizrGlobalSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>, 
            IApizrSharedOptionsBuilderBase
        where TApizrSharedOptions : IApizrSharedOptionsBase
        where TApizrSharedOptionsBuilder :
        IApizrSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>
    {
    }
}
