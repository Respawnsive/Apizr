using System;
using Apizr.Configuring.Proper;
using Apizr.Extending.Configuring.Shared;

namespace Apizr.Extending.Configuring.Proper
{
    public interface IApizrExtendedProperOptionsBuilder<out TApizrExtendedProperOptions, out TApizrExtendedProperOptionsBuilder> : IApizrExtendedProperOptionsBuilderBase,
        IApizrGlobalProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>,
        IApizrExtendedSharedOptionsBuilder<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
        where TApizrExtendedProperOptions : IApizrProperOptionsBase
        where TApizrExtendedProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase<TApizrExtendedProperOptions, TApizrExtendedProperOptionsBuilder>
    {
        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory);

        /// <summary>
        /// Define your web api base address (could be defined with WebApiAttribute)
        /// </summary>
        /// <param name="baseAddressFactory">Your web api base address factory</param>
        /// <returns></returns>
        TApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory);
    }

    public interface IApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder<IApizrExtendedProperOptions, IApizrExtendedProperOptionsBuilder>
    { }
}
