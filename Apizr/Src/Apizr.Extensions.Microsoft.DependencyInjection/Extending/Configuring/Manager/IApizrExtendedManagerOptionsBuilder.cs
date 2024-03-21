using System;
using System.Collections.Generic;
using Apizr.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring.Manager
{
    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedManagerOptionsBuilder<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> : 
        IApizrExtendedManagerOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedCommonOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedProperOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
        where TApizrExtendedOptions : IApizrExtendedManagerOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrExtendedManagerOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }

    /// <summary>
    /// Builder options available for extended registrations
    /// </summary>
    public interface
        IApizrExtendedManagerOptionsBuilder : IApizrExtendedManagerOptionsBuilder<IApizrExtendedManagerOptions, IApizrExtendedManagerOptionsBuilder>
    {
        internal IApizrExtendedManagerOptions ApizrOptions { get; }

        internal void WithHeaders(IDictionary<ApizrLifetimeScope, Func<IList<string>>> headersFactories);
    }
}
