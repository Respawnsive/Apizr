using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptionsBuilder<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>,
        IApizrSharedOptionsBuilder<TApizrProperOptions, TApizrProperOptionsBuilder>
        where TApizrProperOptions : IApizrProperOptionsBase
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }

    public interface IApizrProperOptionsBuilder : IApizrProperOptionsBuilder<IApizrProperOptions, IApizrProperOptionsBuilder>
    { }
}
