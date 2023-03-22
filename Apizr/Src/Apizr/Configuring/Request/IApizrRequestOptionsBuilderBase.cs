using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptionsBuilderBase : IApizrGlobalSharedOptionsBuilderBase
    {

    }

    /// <summary>
    /// Builder options available at request level
    /// </summary>
    public interface IApizrRequestOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> :
        IApizrGlobalSharedOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrRequestOptionsBuilderBase
        where TApizrOptions : IApizrRequestOptionsBase
        where TApizrOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
