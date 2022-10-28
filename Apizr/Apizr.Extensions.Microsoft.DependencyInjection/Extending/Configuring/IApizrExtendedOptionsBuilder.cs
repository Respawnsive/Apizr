using Apizr.Configuring;
using Apizr.Configuring.Shared;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedOptionsBuilder<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> : 
        IApizrExtendedOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedCommonOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedProperOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
        where TApizrExtendedOptions : IApizrExtendedOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }

    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface
        IApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilder<IApizrExtendedOptions, IApizrExtendedOptionsBuilder>
    {
        internal IApizrExtendedOptions ApizrOptions { get; }
    }
}
