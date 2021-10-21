using System;
using System.Linq.Expressions;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Mapping;
using Refit;

namespace Apizr.Extending.Configuring
{
    public interface IApizrExtendedOptionsBuilder<out TApizrExtendedOptions, out TApizrExtendedOptionsBuilder> : IApizrOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedCommonOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>,
        IApizrExtendedProperOptionsBuilder<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
        where TApizrExtendedOptions : IApizrOptionsBase
        where TApizrExtendedOptionsBuilder : IApizrOptionsBuilderBase<TApizrExtendedOptions, TApizrExtendedOptionsBuilder>
    {
    }

    public interface IApizrExtendedOptionsBuilder : IApizrExtendedOptionsBuilder<IApizrExtendedOptions, IApizrExtendedOptionsBuilder>
    { }
}
