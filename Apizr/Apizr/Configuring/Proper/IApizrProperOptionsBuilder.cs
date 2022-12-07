using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for static registrations
    /// </summary>
    public interface IApizrProperOptionsBuilder<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>,
        IApizrSharedRegistrationOptionsBuilder<TApizrProperOptions, TApizrProperOptionsBuilder>
        where TApizrProperOptions : IApizrProperOptionsBase
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
    }

    /// <inheritdoc />
    public interface IApizrProperOptionsBuilder : IApizrProperOptionsBuilder<IApizrProperOptions, IApizrProperOptionsBuilder>
    {
        internal IApizrProperOptions ApizrOptions { get; }
    }
}
