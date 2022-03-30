using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring
{
    /// <summary>
    /// The options builder
    /// </summary>
    public interface IApizrOptionsBuilder<out TApizrOptions, out TApizrOptionsBuilder> : 
        IApizrOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>,
        IApizrCommonOptionsBuilder<TApizrOptions, TApizrOptionsBuilder>,
        IApizrProperOptionsBuilder<TApizrOptions, TApizrOptionsBuilder> 
        where TApizrOptions : IApizrOptionsBase
        where TApizrOptionsBuilder : IApizrOptionsBuilderBase<TApizrOptions, TApizrOptionsBuilder>
    {}

    public interface IApizrOptionsBuilder : IApizrOptionsBuilder<IApizrOptions, IApizrOptionsBuilder>
    {}
}