using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrUploadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrUploadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistry : IApizrEnumerableRegistry
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder,
        TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    public ApizrUploadRegistryBuilder(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    /// <inheritdoc />
    public IApizrUploadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
        TApizrCommonOptionsBuilder> AddFor<TWebApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TWebApi : IUploadApi
    {
        _internalBuilder?.AddWrappingManagerFor<TWebApi, IApizrUploadManager<TWebApi>>(
            apizrManager => new ApizrUploadManager<TWebApi>(apizrManager), optionsBuilder);

        return this;
    }
}