using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrDownloadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrDownloadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistry : IApizrEnumerableRegistry
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder,
        TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    public ApizrDownloadRegistryBuilder(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    /// <inheritdoc />
    public IApizrDownloadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
        TApizrCommonOptionsBuilder> AddFor<TDownloadApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi
    {
        _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>>(
            apizrManager => new ApizrDownloadManager<TDownloadApi>(apizrManager), optionsBuilder);

        return this;
    }

    /// <inheritdoc />
    public IApizrDownloadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
            TApizrCommonOptionsBuilder>
        AddFor<TDownloadApi, TDownloadParams>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>>(
            apizrManager => new ApizrDownloadManager<TDownloadApi, TDownloadParams>(apizrManager), optionsBuilder);

        return this;
    }
}