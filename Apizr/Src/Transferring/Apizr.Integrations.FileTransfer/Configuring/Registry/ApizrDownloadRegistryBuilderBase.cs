using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public abstract class ApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    where TApizrDownloadRegistryBuilder : IApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    protected ApizrDownloadRegistryBuilderBase(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    protected abstract TApizrDownloadRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrDownloadRegistryBuilder AddFor<TDownloadApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi
    {
        _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>>(
            apizrManager => new ApizrDownloadManager<TDownloadApi>(apizrManager), optionsBuilder);

        return Builder;
    }

    /// <inheritdoc />
    public TApizrDownloadRegistryBuilder AddFor<TDownloadApi, TDownloadParams>(Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>>(
            apizrManager => new ApizrDownloadManager<TDownloadApi, TDownloadParams>(apizrManager), optionsBuilder);

        return Builder;
    }
}