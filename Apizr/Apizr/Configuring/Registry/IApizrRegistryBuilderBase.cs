using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Connecting;
using Apizr.Mapping;
using Polly.Registry;
using System;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistryBuilderBase
    {
    }

    public interface IApizrRegistryBuilderBase<out TApizrRegistry, out TApizrRegistryBuilder> : IApizrCommonOptionsBuilderBase
        where TApizrRegistry : IApizrRegistryBase
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder>
    {
        /// <summary>
        /// Apizr registry
        /// </summary>
        TApizrRegistry ApizrRegistry { get; }
    }
}
