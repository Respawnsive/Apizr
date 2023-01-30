using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Manager
{
    /// <summary>
    /// Builder options available for static registrations
    /// </summary>
    public interface IApizrManagerOptionsBuilder<out TApizrOptions, out TApizrOptionsBuilder> : 
        IApizrManagerOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrCommonOptionsBuilder<TApizrOptions, TApizrOptionsBuilder>,
        IApizrProperOptionsBuilder<TApizrOptions, TApizrOptionsBuilder> 
        where TApizrOptions : IApizrManagerOptionsBase
        where TApizrOptionsBuilder : IApizrManagerOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {}

    /// <summary>
    /// Builder options available for static registrations
    /// </summary>
    public interface IApizrManagerOptionsBuilder : IApizrManagerOptionsBuilder<IApizrManagerOptions, IApizrManagerOptionsBuilder>
    {
        internal IApizrManagerOptions ApizrOptions { get; }
    }
}