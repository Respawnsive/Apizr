using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedOptionsBuilderBase : IApizrExtendedCommonOptionsBuilderBase, IApizrExtendedProperOptionsBuilderBase, IApizrGlobalOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedOptionsBuilderBase<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> :
        IApizrGlobalOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedOptionsBuilderBase
        where TApizrExtendedOptions : IApizrExtendedOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }
}
