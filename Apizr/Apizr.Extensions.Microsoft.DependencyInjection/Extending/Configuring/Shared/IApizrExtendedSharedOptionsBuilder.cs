using Apizr.Configuring.Shared;

namespace Apizr.Extending.Configuring.Shared
{
    public interface IApizrExtendedSharedOptionsBuilder<out TApizrExtendedSharedOptions, out TApizrExtendedSharedOptionsBuilder> : IApizrSharedOptionsBuilderBase<TApizrExtendedSharedOptions, TApizrExtendedSharedOptionsBuilder>
        where TApizrExtendedSharedOptions : IApizrSharedOptionsBase
        where TApizrExtendedSharedOptionsBuilder : IApizrSharedOptionsBuilderBase<TApizrExtendedSharedOptions, TApizrExtendedSharedOptionsBuilder>
    {
    }
}
