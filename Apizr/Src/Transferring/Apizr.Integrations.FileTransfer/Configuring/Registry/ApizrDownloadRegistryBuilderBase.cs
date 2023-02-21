using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public abstract class ApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder
    ,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    where TApizrDownloadRegistryBuilder : IApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder,
        TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
{
    protected abstract TApizrDownloadRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrDownloadRegistryBuilder AddDownloadManager(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        => AddDownloadManagerFor<IDownloadApi>(optionsBuilder);

    /// <inheritdoc />
    public abstract TApizrDownloadRegistryBuilder AddDownloadManagerFor<TDownloadApi>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi;

    /// <inheritdoc />
    public abstract TApizrDownloadRegistryBuilder AddDownloadFor<TDownloadApi, TDownloadParams>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TDownloadApi : IDownloadApi<TDownloadParams>;
}