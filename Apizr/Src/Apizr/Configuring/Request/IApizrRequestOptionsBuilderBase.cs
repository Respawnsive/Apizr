using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Request
{
    /// <summary>
    /// Builder options available at request level
    /// </summary>
    public interface IApizrRequestOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrGlobalSharedOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
        where TApizrOptions : IApizrRequestOptionsBase
        where TApizrOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
