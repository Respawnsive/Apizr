using System;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Extending.Configuring.Shared;

namespace Apizr.Extending.Configuring.Common
{
    public interface IApizrExtendedCommonOptionsBuilder<out TApizrExtendedCommonOptions, out TApizrExtendedCommonOptionsBuilder> : IApizrCommonOptionsBuilderBase<TApizrExtendedCommonOptions, TApizrExtendedCommonOptionsBuilder>,
        IApizrExtendedSharedOptionsBuilder<TApizrExtendedCommonOptions, TApizrExtendedCommonOptionsBuilder>
        where TApizrExtendedCommonOptions : IApizrCommonOptionsBase
        where TApizrExtendedCommonOptionsBuilder : IApizrCommonOptionsBuilderBase<TApizrExtendedCommonOptions, TApizrExtendedCommonOptionsBuilder>
    {
    }

    public interface IApizrExtendedCommonOptionsBuilder : IApizrExtendedCommonOptionsBuilder<IApizrExtendedCommonOptions, IApizrExtendedCommonOptionsBuilder>
    { }
}
