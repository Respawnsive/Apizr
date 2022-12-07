using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Common
{
    /// <summary>
    /// Builder options available at common level for both static and extended registrations
    /// </summary>
    public interface IApizrCommonOptionsBuilderBase : IApizrGlobalCommonOptionsBuilderBase, IApizrSharedRegistrationOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available at common level for both static and extended registrations
    /// </summary>
    public interface IApizrCommonOptionsBuilderBase<out TApizrCommonOptions, out TApizrCommonOptionsBuilder> :
        IApizrGlobalCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>,
        IApizrSharedRegistrationOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>,
        IApizrCommonOptionsBuilderBase
        where TApizrCommonOptions : IApizrCommonOptionsBase
        where TApizrCommonOptionsBuilder :
        IApizrCommonOptionsBuilderBase<TApizrCommonOptions, TApizrCommonOptionsBuilder>
    {
    }
}
