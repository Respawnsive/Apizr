using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;

namespace Apizr.Extending.Configuring.Manager
{
    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedManagerOptionsBuilderBase : IApizrExtendedCommonOptionsBuilderBase, IApizrExtendedProperOptionsBuilderBase, IApizrGlobalManagerOptionsBuilderBase
    {
    }

    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedManagerOptionsBuilderBase<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> :
        IApizrGlobalManagerOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedManagerOptionsBuilderBase
        where TApizrExtendedOptions : IApizrExtendedManagerOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrExtendedManagerOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }
}
