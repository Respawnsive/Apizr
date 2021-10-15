using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Proper
{
    public interface IApizrProperOptionsBuilder<out TApizrProperOptions, out TApizrProperOptionsBuilder> : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>,
        IApizrSharedOptionsBuilder<TApizrProperOptions, TApizrProperOptionsBuilder>
        where TApizrProperOptions : IApizrProperOptionsBase
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase<TApizrProperOptions, TApizrProperOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrProperOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory);
    }

    public interface IApizrProperOptionsBuilder : IApizrProperOptionsBuilder<IApizrProperOptions, IApizrProperOptionsBuilder>
    { }
}
