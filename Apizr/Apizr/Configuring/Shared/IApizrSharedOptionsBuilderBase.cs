using System;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Builder options available at both common and proper level for both static and extended registrations
    /// </summary>
    public interface IApizrSharedOptionsBuilderBase : IApizrGlobalSharedOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at both common and proper level for both static and extended registrations
    /// </summary>
    public interface IApizrSharedOptionsBuilderBase<out TApizrSharedOptions, out TApizrSharedOptionsBuilder> :
            IApizrGlobalSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>, 
            IApizrSharedOptionsBuilderBase
        where TApizrSharedOptions : IApizrSharedOptionsBase
        where TApizrSharedOptionsBuilder :
        IApizrSharedOptionsBuilderBase<TApizrSharedOptions, TApizrSharedOptionsBuilder>
    {
    }
}
