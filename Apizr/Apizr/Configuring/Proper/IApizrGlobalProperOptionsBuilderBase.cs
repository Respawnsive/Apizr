using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    public interface IApizrGlobalProperOptionsBuilderBase : IApizrGlobalSharedOptionsBuilderBase
    {
    }

    public interface IApizrGlobalProperOptionsBuilderBase<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrGlobalProperOptionsBuilderBase,
        IApizrGlobalSharedOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    where TApizrProperOptions : IApizrProperOptionsBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }
}
