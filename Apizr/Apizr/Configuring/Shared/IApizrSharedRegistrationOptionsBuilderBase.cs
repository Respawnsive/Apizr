using System;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Builder options available at both common and proper level for both static and extended registrations
    /// </summary>
    public interface IApizrSharedRegistrationOptionsBuilderBase : IApizrGlobalSharedRegistrationOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at both common and proper level for both static and extended registrations
    /// </summary>
    public interface IApizrSharedRegistrationOptionsBuilderBase<out TApizrOptions, out TApizrOptionsBuilder> :
            IApizrGlobalSharedRegistrationOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>, 
            IApizrSharedRegistrationOptionsBuilderBase
        where TApizrOptions : IApizrSharedRegistrationOptionsBase
        where TApizrOptionsBuilder :
        IApizrSharedRegistrationOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {
    }
}
