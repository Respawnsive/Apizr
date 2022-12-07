using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrGlobalSharedOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
        where TApizrOptions : IApizrRequestOptionsBase
        where TApizrOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
